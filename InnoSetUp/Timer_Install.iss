; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "下班倒计时"
#define MyAppVersion "4.2.0"
#define MyAppPublisher "ZhangYuan"
#define MyAppExeName "TImer.exe"
#define MyAppAssocName MyAppName + " File"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{49453914-5F61-4755-A4AF-7ABE00022E22}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={code:GetInstallPath}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
ChangesAssociations=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=OutPut
OutputBaseFilename=Timer_install_4.2.0
SetupIconFile=.\res\time.ico
UninstallDisplayName={#MyAppName}
UninstallDisplayIcon={app}\time.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimplified"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\res\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\res\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKLM;Subkey:"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";ValueType: string; ValueName:"Timer";ValueData:"{app}\{#MyAppExeName}";


[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; 卸载后删除安装目录下所有文件
Type: filesandordirs; Name: "{app}"


[Code]
//获取系统目录
function GetInstallPath(Param: String):String;
begin
   if ParamCount >1 then 
      begin
        Result:=ParamStr(2);
        Exit;
      end; 

   Result := UpperCase(ExtractFileDrive(GetSystemDir)) + '\{#MyAppName}' ;
end;

var 
    ResultCode1:integer;
procedure CurPageChanged(CurPageID: Integer);
var
  ErrorCode: Integer; 
begin  
Log(format( 'CurPageID id = %d',[ ( CurPageID) ]));
  begin
      case CurPageID of
           wpSelectDir:
           begin

           end;
           wpInstalling:
           begin
          
           end;
           wpFinished:
           begin
                if ParamCount >1 then 
               begin
                  //msgbox(ParamStr(2)+'\{#MyAppExeName}', mbInformation,MB_OK);
                  Exec(ParamStr(2)+'\{#MyAppExeName}', ParamStr(2), '', SW_SHOWNORMAL, ewNoWait, ResultCode1);
              end;  
          end;
      end;
  end;
end;
