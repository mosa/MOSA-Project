#define MyAppName "MOSA-Project"
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
Filename: "dotnet.exe"; Parameters: "new install Mosa.Templates"

[Dirs]
Name: "{app}\Tools"
Name: "{app}\bin"
Name: "{app}\Lib"

[Files]
Source: "..\..\Tools\readme.md"; DestDir: "{app}\Tools"; Flags: ignoreversion
Source: "..\..\*.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\*.md"; DestDir: "{app}"; Flags: ignoreversion

Source: "..\..\bin\runtimes\*"; DestDir: "{app}\bin\runtimes"; Flags: ignoreversion recursesubdirs
Source: "..\..\bin\Avalonia*.dll"; DestDir: "{app}\bin"; flags: recursesubdirs
Source: "..\..\bin\Mosa.Compiler.Common.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.Framework.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.MosaTypeSystem.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Compiler.MosaTypeSystem.CLR.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.ARMv8A32.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.x86.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Platform.x64.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Bootstrap.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Explorer.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Debugger.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Launcher.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Compiler.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Tool.Launcher.Console.*"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.BootImage.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Configuration.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.DebugEngine.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.FileSystem.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Launcher.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.RSP.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Mosa.Utility.Disassembler.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\dnlib.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\PriorityQueue.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\WinFormsUI.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Arch.Arm.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Arch.X86.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.Core.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Reko.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\System.Reactive.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\SkiaSharp.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\HarfBuzzSharp.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "..\..\bin\Tmds.DBus.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
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
