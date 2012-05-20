unit UMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OverbyteIcsWndControl, OverbyteIcsWSocket,
  OverbyteIcsWSocketS, OverbyteIcsHttpSrv;

type
  TConfiguration = class
  private
    FSettings: TStrings;
  public
    class procedure Load;
    class function Port: string;
    class procedure Unload;
    class function NamePartToUpper(const NameValue: string): string;

    property Settings: TStrings read FSettings write FSettings;
  end;

  TForm1 = class(TForm)
    log: TMemo;
    Listen: TButton;
    serv: THttpServer;
    procedure ListenClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure servClientConnect(Sender, Client: TObject; Error: Word);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

uses
  StrUtils;

var
  gConfiguration: TConfiguration;

{ TForm }
procedure TForm1.FormCreate(Sender: TObject);
begin
  TConfiguration.Load;
end;

procedure TForm1.ListenClick(Sender: TObject);
begin
  log.Lines.Add(Format('Listening port: %s', [TConfiguration.Port]));
  serv.Port := TConfiguration.Port;
  try
    serv.Start;
  except
    on E: ESocketException do
      log.Lines.Add(E.Message);
  end;
end;

procedure TForm1.servClientConnect(Sender, Client: TObject; Error: Word);
begin
  log.Lines.Add('Client connected');
end;

{ TConfiguration }
class procedure TConfiguration.Load;
const
  INI_FILENAME = 'Activator.Settings';
var
  i: Integer;
begin
  if not Assigned(gConfiguration) then
  begin
    gConfiguration := TConfiguration.Create;
    gConfiguration.Settings := TStringList.Create;
  end;

  gConfiguration.Settings.LoadFromFile(
    IncludeTrailingPathDelimiter(ExtractFilePath(ParamStr(0))) + INI_FILENAME);
  for i := 0 to gConfiguration.Settings.Count - 1 do
    gConfiguration.Settings[i] := NamePartToUpper(gConfiguration.Settings[i]);
end;

class function TConfiguration.Port: string;
var
  p: string;
begin
  p := Trim(gConfiguration.Settings.Values['PORT']);
  if p = '' then
    p := '80';
  Result := p;
end;

class procedure TConfiguration.Unload;
var
  ptr: TStrings;
begin
  if Assigned(gConfiguration) then
  begin
    ptr := gConfiguration.Settings;
    FreeAndNil(ptr);
  end;
  FreeAndNil(gConfiguration);
end;

class function TConfiguration.NamePartToUpper(const NameValue: string): string;
var
  name: string;
begin
  name := Trim(AnsiUpperCase(Copy(NameValue, 1, Pos('=', NameValue) - 1)));
  result := name
    + IfThen(name <> '', '=')
    + Trim(Copy(NameValue, Pos('=', NameValue) + 1, Length(NameValue) - Pos('=', NameValue)));
end;

initialization
  gConfiguration := nil;

finalization
  TConfiguration.Unload;

end.
