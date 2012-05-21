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
  object Button1: TButton
    Left = 32
    Top = 24
    Width = 75
    Height = 25
    Caption = 'Button1'
    TabOrder = 0
  end
  object log: TMemo
    Left = 136
    Top = 0
    Width = 419
    Height = 351
    Align = alRight
    ReadOnly = True
    TabOrder = 1
  end
  object consoleCallTimer: TTimer
    Enabled = False
    OnTimer = consoleCallTimerTimer
    Left = 8
    Top = 64
  end
end
