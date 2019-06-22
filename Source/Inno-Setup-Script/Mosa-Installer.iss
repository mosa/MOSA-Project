; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "MOSA-Project"
#define MyAppVersion "1.5"
#define MyAppPublisher "MOSA-Project"
#define MyAppURL "http://www.mosa-project.org"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{516E4655-F79C-44AC-AA6D-D9A879450A64}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableDirPage=yes
DisableReadyPage=yes
DefaultGroupName={#MyAppName}
OutputDir=..\..\bin
OutputBaseFilename=MOSA-Installer
SolidCompression=yes
MinVersion=0,6.0
AllowUNCPath=False
Compression=lzma2/ultra64
InternalCompressLevel=ultra64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]

[Icons]
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram, {#MyAppName}}"; Filename: "{uninstallexe}"

[Run]

[Dirs]
Name: "{app}\Tools"
Name: "{app}\bin"
Name: "{app}\Lib"

[Files]
Source: "..\..\Tools\readme.md"; DestDir: "{app}\Tools"; Flags: ignoreversion
Source: "..\..\*.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\*.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\app.config"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\Boot.cs"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\Mosa.Starter.x86.csproj"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\MyTemplate.vstemplate"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\packages.config"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\Program.cs"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\MOSA Project\__TemplateIcon.ico"; DestDir: "{userdocs}\Visual Studio 2017\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Bootstrap.exe"; DestDir: "{app}\bin"; Flags: ignoreversion

[ThirdParty]
UseRelativePaths=True
