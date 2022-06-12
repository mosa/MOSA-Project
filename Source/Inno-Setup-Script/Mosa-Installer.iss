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

Source: "..\..\bin\runtimes\*"; DestDir: "{app}\bin\runtimes"; Flags: ignoreversion recursesubdirs
Source: "..\..\bin\Avalonia*.dll"; DestDir: "{app}\bin"; flags: recursesubdirs
Source: "..\..\bin\Mosa.Tool.Bootstrap.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.Common.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.Framework.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.MosaTypeSystem.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.ARMv8A32.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.Intel.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.x86.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.x64.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Explorer.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Debugger.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Launcher.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Compiler.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Launcher.Console.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.BootImage.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Configuration.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.DebugEngine.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.FileSystem.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Launcher.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.RSP.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Disassembler.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
;Source: "..\..\bin\System.Threading.Channels.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
;Source: "..\..\bin\System.Threading.Tasks.Extensions.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\dnlib.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\CommandLine.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\PriorityQueue.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\WinFormsUI.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Arch.Arm.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Arch.X86.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Core.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\Tools\qemu\*.*"; DestDir: "{app}\Tools\qemu"; Flags: ignoreversion
Source: "..\..\Tools\wget\*.*"; DestDir: "{app}\Tools\wget"; Flags: ignoreversion
Source: "..\..\Tools\nasm\*.*"; DestDir: "{app}\Tools\nasm"; Flags: ignoreversion
Source: "..\..\Tools\ndisasm\*.*"; DestDir: "{app}\Tools\ndisasm"; Flags: ignoreversion
Source: "..\..\Tools\Bochs\*.*"; DestDir: "{app}\Tools\Bochs"; Flags: ignoreversion
Source: "..\..\Tools\7zip\*.*"; DestDir: "{app}\Tools\7zip"; Flags: ignoreversion
Source: "..\..\Tools\mkisofs\*.*"; DestDir: "{app}\Tools\mkisofs"; Flags: ignoreversion
;Source: "..\..\Tools\rufus\*.*"; DestDir: "{app}\Tools\rufus"; Flags: ignoreversion

[ThirdParty]
UseRelativePaths=True
