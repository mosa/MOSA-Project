#define MyAppName "MOSA-Project"
#define MyAppPublisher "MOSA-Project"
#define MyAppURL "http://www.mosa-project.org"
#define MyAppVersion "2.2"

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
DefaultDirName={commonpf}\{#MyAppName}
DisableDirPage=yes
DisableReadyPage=yes
DefaultGroupName={#MyAppName}
OutputDir=..\..\bin
OutputBaseFilename=MOSA-Installer-{#MyAppVersion}
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

Source: "..\Mosa.VisualStudio.ProjectTemplate\Boot.cs"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\Mosa.VisualStudio.ProjectTemplate\Mosa.Starter.x86.csproj"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\Mosa.VisualStudio.ProjectTemplate\MyTemplate.vstemplate"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\Mosa.VisualStudio.ProjectTemplate\Program.cs"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\Mosa.VisualStudio.ProjectTemplate\__TemplateIcon.ico"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project"; Flags: ignoreversion
Source: "..\Mosa.VisualStudio.ProjectTemplate\Properties\launchSettings.json"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates\Mosa Project\Properties"; Flags: ignoreversion

Source: "..\..\bin\*.*"; DestDir: "{app}\bin"; Flags: ignoreversion recursesubdirs
Source: "..\..\Tools\*.*"; DestDir: "{app}\Tools"; Flags: ignoreversion recursesubdirs

[ThirdParty]
UseRelativePaths=True