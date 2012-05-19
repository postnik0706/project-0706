program SecurityModule;

uses
  Forms,
  USecurityModule in 'USecurityModule.pas' {Form1},
  SOAPHTTPTrans in 'C:\Program Files (x86)\Embarcadero\RAD Studio\7.0\source\Win32\soap\SOAPHTTPTrans.pas',
  UWebSecurityConsole in 'UWebSecurityConsole.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
