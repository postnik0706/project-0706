// ************************************************************************ //
// The types declared in this file were generated from data read from the
// WSDL File described below:
// WSDL     : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?wsdl
//  >Import : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?wsdl=wsdl0
//  >Import : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?wsdl=wsdl0>0
//  >Import : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?xsd=xsd0
//  >Import : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?xsd=xsd2
//  >Import : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?xsd=xsd1
// Encoding : utf-8
// Codegen  : [wfUnwindLiteralParameters-]
// Version  : 1.0
// (5/20/2012 8:41:03 PM - - $Rev: 24171 $)
// ************************************************************************ //

unit UWebSecurityConsole;

interface

uses InvokeRegistry, SOAPHTTPClient, Types, XSBuiltIns;

const
  IS_OPTN = $0001;
  IS_UNBD = $0002;
  IS_NLBL = $0004;
  IS_REF  = $0080;


type

  // ************************************************************************ //
  // The following types, referred to in the WSDL document are not being represented
  // in this file. They are either aliases[@] of other types represented or were referred
  // to but never[!] declared in the document. The types from the latter category
  // typically map to predefined/known XML or Embarcadero types; however, they could also 
  // indicate incorrect WSDL documents that failed to declare or import a schema type.
  // ************************************************************************ //
  // !:string          - "http://www.w3.org/2001/XMLSchema"[Gbl]
  // !:boolean         - "http://www.w3.org/2001/XMLSchema"[Gbl]
  // !:dateTime        - "http://www.w3.org/2001/XMLSchema"[Gbl]
  // !:int             - "http://www.w3.org/2001/XMLSchema"[Gbl]

  BuildingSensorsResponse = class;              { "http://schemas.datacontract.org/2004/07/ConsoleApplication"[GblCplx] }
  SensorData           = class;                 { "http://schemas.datacontract.org/2004/07/ConsoleApplication"[GblCplx] }
  BuildingSensorsResponse2 = class;             { "http://schemas.datacontract.org/2004/07/ConsoleApplication"[GblElm] }
  SensorData2          = class;                 { "http://schemas.datacontract.org/2004/07/ConsoleApplication"[GblElm] }
  ReportStatus         = class;                 { "http://secured.fabric.com/2012/05/Security"[GblElm] }
  ReportStatusResponse = class;                 { "http://secured.fabric.com/2012/05/Security"[GblElm] }

  ArrayOfSensorData = array of SensorData;      { "http://schemas.datacontract.org/2004/07/ConsoleApplication"[GblCplx] }


  // ************************************************************************ //
  // XML       : BuildingSensorsResponse, global, <complexType>
  // Namespace : http://schemas.datacontract.org/2004/07/ConsoleApplication
  // ************************************************************************ //
  BuildingSensorsResponse = class(TRemotable)
  private
    FBuildingSensorData: ArrayOfSensorData;
    FBuildingSensorData_Specified: boolean;
    procedure SetBuildingSensorData(Index: Integer; const AArrayOfSensorData: ArrayOfSensorData);
    function  BuildingSensorData_Specified(Index: Integer): boolean;
  public
    destructor Destroy; override;
  published
    property BuildingSensorData: ArrayOfSensorData  Index (IS_OPTN or IS_NLBL) read FBuildingSensorData write SetBuildingSensorData stored BuildingSensorData_Specified;
  end;



  // ************************************************************************ //
  // XML       : SensorData, global, <complexType>
  // Namespace : http://schemas.datacontract.org/2004/07/ConsoleApplication
  // ************************************************************************ //
  SensorData = class(TRemotable)
  private
    FBuildingID: string;
    FBuildingID_Specified: boolean;
    FIsAlarmed: Boolean;
    FIsAlarmed_Specified: boolean;
    FMeasureDateTime: TXSDateTime;
    FMeasureDateTime_Specified: boolean;
    FSensorID: Integer;
    FSensorID_Specified: boolean;
    FSensorType: string;
    FSensorType_Specified: boolean;
    procedure SetBuildingID(Index: Integer; const Astring: string);
    function  BuildingID_Specified(Index: Integer): boolean;
    procedure SetIsAlarmed(Index: Integer; const ABoolean: Boolean);
    function  IsAlarmed_Specified(Index: Integer): boolean;
    procedure SetMeasureDateTime(Index: Integer; const ATXSDateTime: TXSDateTime);
    function  MeasureDateTime_Specified(Index: Integer): boolean;
    procedure SetSensorID(Index: Integer; const AInteger: Integer);
    function  SensorID_Specified(Index: Integer): boolean;
    procedure SetSensorType(Index: Integer; const Astring: string);
    function  SensorType_Specified(Index: Integer): boolean;
  public
    destructor Destroy; override;
  published
    property BuildingID:      string       Index (IS_OPTN or IS_NLBL) read FBuildingID write SetBuildingID stored BuildingID_Specified;
    property IsAlarmed:       Boolean      Index (IS_OPTN) read FIsAlarmed write SetIsAlarmed stored IsAlarmed_Specified;
    property MeasureDateTime: TXSDateTime  Index (IS_OPTN) read FMeasureDateTime write SetMeasureDateTime stored MeasureDateTime_Specified;
    property SensorID:        Integer      Index (IS_OPTN) read FSensorID write SetSensorID stored SensorID_Specified;
    property SensorType:      string       Index (IS_OPTN or IS_NLBL) read FSensorType write SetSensorType stored SensorType_Specified;
  end;



  // ************************************************************************ //
  // XML       : BuildingSensorsResponse, global, <element>
  // Namespace : http://schemas.datacontract.org/2004/07/ConsoleApplication
  // ************************************************************************ //
  BuildingSensorsResponse2 = class(BuildingSensorsResponse)
  private
  published
  end;



  // ************************************************************************ //
  // XML       : SensorData, global, <element>
  // Namespace : http://schemas.datacontract.org/2004/07/ConsoleApplication
  // ************************************************************************ //
  SensorData2 = class(SensorData)
  private
  published
  end;



  // ************************************************************************ //
  // XML       : ReportStatus, global, <element>
  // Namespace : http://secured.fabric.com/2012/05/Security
  // ************************************************************************ //
  ReportStatus = class(TRemotable)
  private
    FStatusToReport: BuildingSensorsResponse;
    FStatusToReport_Specified: boolean;
    FTest: string;
    FTest_Specified: boolean;
    procedure SetStatusToReport(Index: Integer; const ABuildingSensorsResponse: BuildingSensorsResponse);
    function  StatusToReport_Specified(Index: Integer): boolean;
    procedure SetTest(Index: Integer; const Astring: string);
    function  Test_Specified(Index: Integer): boolean;
  public
    destructor Destroy; override;
  published
    property StatusToReport: BuildingSensorsResponse  Index (IS_OPTN or IS_NLBL) read FStatusToReport write SetStatusToReport stored StatusToReport_Specified;
    property Test:           string                   Index (IS_OPTN or IS_NLBL) read FTest write SetTest stored Test_Specified;
  end;



  // ************************************************************************ //
  // XML       : ReportStatusResponse, global, <element>
  // Namespace : http://secured.fabric.com/2012/05/Security
  // ************************************************************************ //
  ReportStatusResponse = class(TRemotable)
  private
  published
  end;


  // ************************************************************************ //
  // Namespace : http://secured.fabric.com/2012/05/Security
  // transport : http://schemas.xmlsoap.org/soap/http
  // style     : document
  // binding   : BasicHttpBinding_ISecurityConsole
  // service   : SecurityConsole
  // port      : BasicHttpBinding_ISecurityConsole
  // URL       : http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/
  // ************************************************************************ //
  ISecurityConsole = interface(IInvokable)
  ['{6C7E3C68-810E-1176-0C23-19E6C21F7BA4}']
    function  ReportStatus(const parameters: ReportStatus): ReportStatusResponse; stdcall;
  end;

function GetISecurityConsole(UseWSDL: Boolean=System.False; Addr: string=''; HTTPRIO: THTTPRIO = nil): ISecurityConsole;


implementation
  uses SysUtils;

function GetISecurityConsole(UseWSDL: Boolean; Addr: string; HTTPRIO: THTTPRIO): ISecurityConsole;
const
  defWSDL = 'http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/?wsdl';
  defURL  = 'http://localhost:8732/Design_Time_Addresses/ConsoleApplication/SecurityConsole/';
  defSvc  = 'SecurityConsole';
  defPrt  = 'BasicHttpBinding_ISecurityConsole';
var
  RIO: THTTPRIO;
begin
  Result := nil;
  if (Addr = '') then
  begin
    if UseWSDL then
      Addr := defWSDL
    else
      Addr := defURL;
  end;
  if HTTPRIO = nil then
    RIO := THTTPRIO.Create(nil)
  else
    RIO := HTTPRIO;
  try
    Result := (RIO as ISecurityConsole);
    if UseWSDL then
    begin
      RIO.WSDLLocation := Addr;
      RIO.Service := defSvc;
      RIO.Port := defPrt;
    end else
      RIO.URL := Addr;
  finally
    if (Result = nil) and (HTTPRIO = nil) then
      RIO.Free;
  end;
end;


destructor BuildingSensorsResponse.Destroy;
var
  I: Integer;
begin
  for I := 0 to System.Length(FBuildingSensorData)-1 do
    SysUtils.FreeAndNil(FBuildingSensorData[I]);
  System.SetLength(FBuildingSensorData, 0);
  inherited Destroy;
end;

procedure BuildingSensorsResponse.SetBuildingSensorData(Index: Integer; const AArrayOfSensorData: ArrayOfSensorData);
begin
  FBuildingSensorData := AArrayOfSensorData;
  FBuildingSensorData_Specified := True;
end;

function BuildingSensorsResponse.BuildingSensorData_Specified(Index: Integer): boolean;
begin
  Result := FBuildingSensorData_Specified;
end;

destructor SensorData.Destroy;
begin
  SysUtils.FreeAndNil(FMeasureDateTime);
  inherited Destroy;
end;

procedure SensorData.SetBuildingID(Index: Integer; const Astring: string);
begin
  FBuildingID := Astring;
  FBuildingID_Specified := True;
end;

function SensorData.BuildingID_Specified(Index: Integer): boolean;
begin
  Result := FBuildingID_Specified;
end;

procedure SensorData.SetIsAlarmed(Index: Integer; const ABoolean: Boolean);
begin
  FIsAlarmed := ABoolean;
  FIsAlarmed_Specified := True;
end;

function SensorData.IsAlarmed_Specified(Index: Integer): boolean;
begin
  Result := FIsAlarmed_Specified;
end;

procedure SensorData.SetMeasureDateTime(Index: Integer; const ATXSDateTime: TXSDateTime);
begin
  FMeasureDateTime := ATXSDateTime;
  FMeasureDateTime_Specified := True;
end;

function SensorData.MeasureDateTime_Specified(Index: Integer): boolean;
begin
  Result := FMeasureDateTime_Specified;
end;

procedure SensorData.SetSensorID(Index: Integer; const AInteger: Integer);
begin
  FSensorID := AInteger;
  FSensorID_Specified := True;
end;

function SensorData.SensorID_Specified(Index: Integer): boolean;
begin
  Result := FSensorID_Specified;
end;

procedure SensorData.SetSensorType(Index: Integer; const Astring: string);
begin
  FSensorType := Astring;
  FSensorType_Specified := True;
end;

function SensorData.SensorType_Specified(Index: Integer): boolean;
begin
  Result := FSensorType_Specified;
end;

destructor ReportStatus.Destroy;
begin
  SysUtils.FreeAndNil(FStatusToReport);
  inherited Destroy;
end;

procedure ReportStatus.SetStatusToReport(Index: Integer; const ABuildingSensorsResponse: BuildingSensorsResponse);
begin
  FStatusToReport := ABuildingSensorsResponse;
  FStatusToReport_Specified := True;
end;

function ReportStatus.StatusToReport_Specified(Index: Integer): boolean;
begin
  Result := FStatusToReport_Specified;
end;

procedure ReportStatus.SetTest(Index: Integer; const Astring: string);
begin
  FTest := Astring;
  FTest_Specified := True;
end;

function ReportStatus.Test_Specified(Index: Integer): boolean;
begin
  Result := FTest_Specified;
end;

initialization
  InvRegistry.RegisterInterface(TypeInfo(ISecurityConsole), 'http://secured.fabric.com/2012/05/Security', 'utf-8');
  InvRegistry.RegisterDefaultSOAPAction(TypeInfo(ISecurityConsole), '');
  InvRegistry.RegisterInvokeOptions(TypeInfo(ISecurityConsole), ioDocument);
  InvRegistry.RegisterInvokeOptions(TypeInfo(ISecurityConsole), ioLiteral);
  InvRegistry.RegisterExternalParamName(TypeInfo(ISecurityConsole), 'ReportStatus', 'parameters1', 'parameters');
  RemClassRegistry.RegisterXSInfo(TypeInfo(ArrayOfSensorData), 'http://schemas.datacontract.org/2004/07/ConsoleApplication', 'ArrayOfSensorData');
  RemClassRegistry.RegisterXSClass(BuildingSensorsResponse, 'http://schemas.datacontract.org/2004/07/ConsoleApplication', 'BuildingSensorsResponse');
  RemClassRegistry.RegisterXSClass(SensorData, 'http://schemas.datacontract.org/2004/07/ConsoleApplication', 'SensorData');
  RemClassRegistry.RegisterXSClass(BuildingSensorsResponse2, 'http://schemas.datacontract.org/2004/07/ConsoleApplication', 'BuildingSensorsResponse2', 'BuildingSensorsResponse');
  RemClassRegistry.RegisterXSClass(SensorData2, 'http://schemas.datacontract.org/2004/07/ConsoleApplication', 'SensorData2', 'SensorData');
  RemClassRegistry.RegisterXSClass(ReportStatus, 'http://secured.fabric.com/2012/05/Security', 'ReportStatus');
  RemClassRegistry.RegisterXSClass(ReportStatusResponse, 'http://secured.fabric.com/2012/05/Security', 'ReportStatusResponse');

end.