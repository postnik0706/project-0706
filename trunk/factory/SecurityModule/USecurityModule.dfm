object frmSecurityModule: TfrmSecurityModule
  Left = 0
  Top = 0
  Caption = 'Security Module'
  ClientHeight = 351
  ClientWidth = 555
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 13
  object log: TMemo
    Left = 0
    Top = 0
    Width = 555
    Height = 351
    Align = alClient
    ReadOnly = True
    TabOrder = 0
    ExplicitLeft = 96
    ExplicitWidth = 459
  end
  object consoleCallTimer: TTimer
    Enabled = False
    OnTimer = consoleCallTimerTimer
    Left = 8
    Top = 64
  end
end
