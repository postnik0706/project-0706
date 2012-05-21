unit USecurityModule;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OverbyteIcsWndControl, OverbyteIcsWSocket,
  OverbyteIcsWSocketS, OverbyteIcsHttpSrv, ExtCtrls, inifiles,
  Generics.Collections;

type
  TSensorMatrix = class;
  TSensorFileReader = class;
  TControlThread = class;

  TfrmSecurityModule = class(TForm)
    log: TMemo;
    consoleCallTimer: TTimer;                   // generating 'report to the console' event
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure consoleCallTimerTimer(Sender: TObject);
    procedure FormCloseQuery(Sender: TObject; var CanClose: Boolean);
  private
    FSensorMatrix: TSensorMatrix;               // Hoding data of all sensors of this bldg
    FSensorFileReader: TSensorFileReader;       // Stub for emulating sensor input
    FControlThread: TControlThread;             // Thread hosting the activation/deactivation web service
    procedure LogMessage(const Message: string);
  public
    procedure ControlConsoleClient;             // is used for controlling the calls to
  end;

  { Provides Activation/Deactivation updates }
  TControlThread = class(TThread)
  private
    FServer: THttpServer;                       // Hosts the web server for activation/deactivation
    FReadSoFar: integer;                        // utility fields as HTTP data may be coming in in chunks
    FRequest: AnsiString;
    procedure HandlePostedData(Sender, Client: TObject; Error: Word);
    procedure HandlePostDocument(Sender, Client: TObject;
      var Flags: THttpGetFlag);
    procedure HandleBeforeProcessRequest(Sender, Client: TObject);
  public
    constructor Create;
    destructor Destroy; override;
  protected
    procedure Execute; override;
  end;

  { Reads configuration data; is used by the main form and the thread }
  TConfiguration = class
  private
    FSettings: TStrings;              // uses a TStringList object to load and hold settings
    FCritSec: TRTLCriticalSection;
  public
    class procedure Load;             // Loads settings
    class function BuildingID: string;// ID of this building
    class function Port: string;      // Port used for the activation/deactivation server hosted in this exe
    class function URI: string;       // URI of the central location
    class procedure Unload;
    class function NamePartToUpper(const NameValue: string): string;
    constructor Create;
    destructor Destroy; override;

    property Settings: TStrings read FSettings write FSettings;
  end;

  { Holds data of one sensor }
  TSensor = class
  private
    FSensorID: integer;
    FSensorType: string;
    FSensorAlarmed: Boolean;
  public
    constructor Create(SensorID: integer; SensorType: string);
    property SensorID: integer read FSensorID write FSensorID;
    property SensorType: string read FSensorType write FSensorType;
    property SensorAlarmed: Boolean read FSensorAlarmed write FSensorAlarmed;
  end;

  ISensorReader = interface                  // Interface used for reading data
    procedure ReadStatus(Sensor: TSensor);
  end;

  { We implement the ISensorReader interface to emulate actual sensor readings }
  TSensorFileReader = class(TInterfacedObject, ISensorReader)
  private                                    // see also TSensorMatrix.Poll
    FIni: TIniFile;
  public
    constructor Create;
    destructor Destroy; override;
    procedure ReadStatus(Sensor: TSensor);   // Reads the status of one sensor
  end;

  { Reads all censors status in a particular building }
  TSensorMatrix = class
  private
    FSensors: TList<TSensor>;                // List of sensors
  public
    constructor Create;
    destructor Destroy; override;
    procedure Poll(SensorReader: ISensorReader);  // Uses generic interface to employ emulation of sensor reading
    procedure Load;                          // Loads types from the 'settings' file - this is not sensor reading but supposed to be static data

    property Sensors: TList<TSensor>read FSensors;
  end;

var
  frmSecurityModule: TfrmSecurityModule;

const
  TERMINATION_POLL_TIMEOUT = 1000;      // Check for thread termination with this period, msec

implementation

{$R *.dfm}

uses
  strutils, UWebSecurityConsole, SOAPHttpTrans, SOAPHTTPClient, XSBuiltIns;

var
  gConfiguration: TConfiguration;
  gPostString: string;
  gPostCritSec: TRTLCriticalSection; // TODO: make a singleton class

{ TConfiguration }
constructor TConfiguration.Create;
begin
  inherited Create;
  InitializeCriticalSection(FCritSec);
  FSettings := TStringList.Create;
end;

destructor TConfiguration.Destroy;
begin
  if Assigned(FSettings) then
    FreeAndNil(FSettings);
  DeleteCriticalSection(FCritSec);
  inherited Destroy;
end;

class procedure TConfiguration.Load;
var
  i: integer;
const
  INI_FILENAME = 'ConsoleLink.settings';
begin
  if not Assigned(gConfiguration) then
  begin
    gConfiguration := TConfiguration.Create;
  end;

  gConfiguration.Settings.LoadFromFile(IncludeTrailingPathDelimiter
      (ExtractFilePath(ParamStr(0))) + INI_FILENAME);
  for i := 0 to gConfiguration.Settings.Count - 1 do
    gConfiguration.Settings[i] := NamePartToUpper(gConfiguration.Settings[i]);
end;

class function TConfiguration.Port: string;
var
  p: string;
begin
  EnterCriticalSection(gConfiguration.FCritSec);
  try
    p := Trim(gConfiguration.Settings.Values['PORT']);
    if p = '' then
      p := '80';
    Result := p;
  finally
    LeaveCriticalSection(gConfiguration.FCritSec);
  end;
end;

class function TConfiguration.URI: string;
begin
  EnterCriticalSection(gConfiguration.FCritSec);
  try
    Result := Trim(gConfiguration.Settings.Values['URI']);
  finally
    LeaveCriticalSection(gConfiguration.FCritSec);
  end;
end;

class function TConfiguration.BuildingID: string;
begin
  EnterCriticalSection(gConfiguration.FCritSec);
  try
    Result := Trim(gConfiguration.Settings.Values['BuildingID']);
  finally
    LeaveCriticalSection(gConfiguration.FCritSec);
  end;
end;

class procedure TConfiguration.Unload;
begin
  if Assigned(gConfiguration) then
    FreeAndNil(gConfiguration);
end;

class function TConfiguration.NamePartToUpper(const NameValue: string): string;
var
  name: string;
begin
  name := Trim(AnsiUpperCase(Copy(NameValue, 1, Pos('=', NameValue) - 1)));
  Result := name + IfThen(name <> '', '=') + Trim
    (Copy(NameValue, Pos('=', NameValue) + 1, Length(NameValue) - Pos
        ('=', NameValue)));
end;

{ TfrmSecurityModule }
procedure TfrmSecurityModule.FormCloseQuery
  (Sender: TObject; var CanClose: Boolean);
begin
  if Assigned(FControlThread) then
  begin
    FControlThread.Terminate;
    FControlThread.WaitFor;
  end;
  CanClose := True;
end;

procedure TfrmSecurityModule.FormCreate(Sender: TObject);
begin
  TConfiguration.Load;
  FControlThread := TControlThread.Create;
  FSensorMatrix := TSensorMatrix.Create;
  FSensorFileReader := TSensorFileReader.Create;
  FSensorMatrix.Load;
  LogMessage('Service started');
end;

procedure TfrmSecurityModule.FormDestroy(Sender: TObject);
begin
  FSensorFileReader := nil;             // interfaced object
  if Assigned(FSensorMatrix) then
    FreeAndNil(FSensorMatrix);

  FreeAndNil(FSensorMatrix);
  TConfiguration.Unload;
end;

procedure TfrmSecurityModule.ControlConsoleClient;
var
  Settings: TStrings;
  bActivate: Boolean;
  i: Integer;
  msg: string;
begin
  gPostString := ReplaceStr(gPostString, #10, #13#10);

  consoleCallTimer.Enabled := False;
  Settings := TStringList.Create;
  try
    Settings.Text := gPostString;
    msg := '';
    for i := 0 to Settings.Count - 1 do
      if Pos('SECRET', Settings[i]) = 0  then
        msg := msg + IfThen(i>0, '/') + Settings[i];
    LogMessage(Format('Web Service command event: %s', [msg]));

    bActivate := Settings.Values['ACTIVATE'] = '1';
    LogMessage(IfThen(bActivate, 'Activating the sensors...',
        'Deactivating the sensors...'));
    consoleCallTimer.Interval := StrToInt(Settings.Values['POLLING']) * 1000;

    if bActivate then // First run
      consoleCallTimerTimer(consoleCallTimer);

    consoleCallTimer.Enabled := bActivate;
    LogMessage('Done');
  finally
    FreeAndNil(Settings);
  end;
end;

procedure TfrmSecurityModule.LogMessage(const Message: string);
begin
  log.Lines.Add(Message);
end;

{ TControlThread }
constructor TControlThread.Create;
begin
  inherited Create(True);
  FreeOnTerminate := False;

  FServer := THttpServer.Create(nil);
  FServer.Port := TConfiguration.Port;
  FServer.OnPostDocument := HandlePostDocument;
  FServer.OnPostedData := HandlePostedData;
  FServer.OnBeforeProcessRequest := HandleBeforeProcessRequest;
  FServer.Start;
  Resume;
end;

destructor TControlThread.Destroy;
begin
  if Assigned(FServer) then
    FreeAndNil(FServer);
  inherited Destroy;
end;

procedure TControlThread.Execute;
var
  DummyHandle: THandle;
begin
  FServer.Start;
  DummyHandle := INVALID_HANDLE_VALUE;
  while True do
  begin
    if Terminated then
      break;
    WaitForSingleObject(DummyHandle, TERMINATION_POLL_TIMEOUT);
  end;
end;

procedure TControlThread.HandleBeforeProcessRequest(Sender, Client: TObject);
begin
  FReadSoFar := 0;
  FRequest := '';
end;

procedure TControlThread.HandlePostedData(Sender, Client: TObject; Error: Word);
var
  buf: PAnsiChar;
  ClientCnx: THttpConnection;
  dataLen: integer;
  Dummy: THttpGetFlag;
begin
  ClientCnx := THttpConnection(Client);

  { Information might be received in chunks }
  buf := AnsiStrAlloc(ClientCnx.RequestContentLength + 1);
  try
    dataLen := ClientCnx.Receive(buf, ClientCnx.RequestContentLength);
    FReadSoFar := FReadSoFar + dataLen;
    buf[FReadSoFar] := #0;
    FRequest := FRequest + StrPas(buf);
  finally
    StrDispose(buf);
  end;
  if FReadSoFar >= ClientCnx.RequestContentLength then
  begin
    FReadSoFar := 0;

    EnterCriticalSection(gPostCritSec);
    try
      gPostString := string(FRequest);
    finally
      LeaveCriticalSection(gPostCritSec);
    end;
    Synchronize(frmSecurityModule.ControlConsoleClient);

    ClientCnx.AnswerString(Dummy, '', '', '', 'OK');
  end;
end;

procedure TControlThread.HandlePostDocument(Sender, Client: TObject;
  var Flags: THttpGetFlag);
begin
  Flags := hgAcceptData;
end;

procedure TfrmSecurityModule.consoleCallTimerTimer(Sender: TObject);
var
  par: ReportStatus;
  arr: ArrayOfSensorData;
  i: integer;
begin
  par := ReportStatus.Create;
  try
    LogMessage('Timer event - requesting the sensors status...');
    FSensorMatrix.Poll(TSensorFileReader.Create);

    par.StatusToReport := BuildingSensorsResponse.Create;
    SetLength(arr, FSensorMatrix.Sensors.Count);
    par.StatusToReport.BuildingSensorData := arr;
    for i := 0 to FSensorMatrix.Sensors.Count - 1 do
    begin
      arr[i] := SensorData.Create;
      arr[i].BuildingID := TConfiguration.BuildingID;
      arr[i].SensorID := FSensorMatrix.Sensors[i].SensorID;
      arr[i].IsAlarmed := FSensorMatrix.Sensors[i].SensorAlarmed;
      arr[i].SensorType := FSensorMatrix.Sensors[i].SensorType;
      arr[i].MeasureDateTime := TXSDateTime.Create;
      arr[i].MeasureDateTime.AsDateTime := Now;
    end;

    try
      // WCF service call
      GetISecurityConsole(False, TConfiguration.URI).ReportStatus(par);
    except
      on E: Exception do
      begin
        LogMessage(Format('Error sending status to the security console: %s',
            [E.Message]));
      end;
    end;
  finally
    for i := 0 to FSensorMatrix.Sensors.Count - 1 do
    begin
      { NOTE: arr[i] takes care of this
        ptr := arr[i].MeasureDateTime;
        if Assigned(ptr) then
        FreeAndNil(ptr); }
      if Assigned(arr[i]) then
        FreeAndNil(arr[i]);
    end;
    { NOTE: resp releases at ReportStatus destructor
      resp := par.StatusToReport;
      if Assigned(resp) then
      FreeAndNil(resp); }
    if Assigned(par) then
      FreeAndNil(par);
  end;
end;

{ TSensorFileReader }
constructor TSensorFileReader.Create;
const
  INI_FILENAME = 'SensorsStatus.settings';
begin
  inherited Create;
  FIni := TIniFile.Create(IncludeTrailingPathDelimiter
      (ExtractFilePath(ParamStr(0))) + INI_FILENAME);
end;

destructor TSensorFileReader.Destroy;
begin
  FreeAndNil(FIni);
  inherited Destroy;
end;

procedure TSensorFileReader.ReadStatus(Sensor: TSensor);
begin
  Sensor.SensorAlarmed := FIni.ReadString('S' + IntToStr(Sensor.SensorID),
    'Status', '1') = '1';
end;

{ TSensor }
constructor TSensor.Create(SensorID: integer; SensorType: string);
begin
  inherited Create;
  FSensorID := SensorID;
  FSensorType := SensorType;
end;

{ TSensorMatrix }
constructor TSensorMatrix.Create;
begin
  inherited Create;
  FSensors := TList<TSensor>.Create;
end;

destructor TSensorMatrix.Destroy;
begin
  if Assigned(FSensors) then
    FreeAndNil(FSensors);
  inherited Destroy;
end;

procedure TSensorMatrix.Poll(SensorReader: ISensorReader);
var
  i: TSensor;
begin
  for i in FSensors do
    SensorReader.ReadStatus(i);
end;

procedure TSensorMatrix.Load;
const
  INI_FILENAME = 'Sensors.Settings';
var
  ini: TIniFile;
  sections: TStrings;
  i: string;
begin
  ini := TIniFile.Create(IncludeTrailingPathDelimiter
      (ExtractFilePath(ParamStr(0))) + INI_FILENAME);
  try
    sections := TStringList.Create;
    ini.ReadSections(sections);
    for i in sections do
      FSensors.Add(TSensor.Create(StrToInt(Copy(i, 2, Length(i) - 1)),
          ini.ReadString(i, 'Type', '')));
  finally
    FreeAndNil(sections);
    FreeAndNil(ini);
  end;
end;

initialization

InitializeCriticalSection(gPostCritSec);

finalization

DeleteCriticalSection(gPostCritSec);

end.
