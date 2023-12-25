; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "�°൹��ʱ"
#define MyAppVersion "2.0.1"
#define MyAppPublisher "ZhangYuan"
#define MyAppExeName "TImer.exe"
#define MyAppAssocName MyAppName + " File"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{49453914-5F61-4755-A4AF-7ABE00022E22}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
ChangesAssociations=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=OutPut
OutputBaseFilename=Timer_install_2.0.1
SetupIconFile=E:\Projects\Timer\InnoSetUp\res\time.ico
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
Source: "E:\Projects\Timer\InnoSetUp\res\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Projects\Timer\InnoSetUp\res\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKLM;Subkey:"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";ValueType: string; ValueName:"Timer";ValueData:"{app}\{#MyAppExeName}";


[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
