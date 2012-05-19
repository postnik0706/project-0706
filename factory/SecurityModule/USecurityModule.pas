unit USecurityModule;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Button1: TButton;
    procedure Button1Click(Sender: TObject);
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
  UWebSecurityConsole, SOAPHttpTrans, SOAPHTTPClient;

procedure TForm1.Button1Click(Sender: TObject);
var
  par: ReportStatus;
  par1: ReportStatusResponse;
  arr: ArrayOfSensorData;
begin
  try

    par := ReportStatus.Create;
    par.StatusToReport := BuildingSensorsResponse.Create;
    SetLength(arr, 1);
    par.StatusToReport.BuildingSensorData := arr;
    arr[0] := SensorData.Create;
    arr[0].SensorID := 5;
    arr[0].IsAlarmed := True;
    arr[0].SensorType := 'WINDOW';
    GetISecurityConsole().ReportStatus(par, par1);
  finally
    FreeAndNil(arr[0]);
    FreeAndNil(par);
  end;
end;

end.
