IF EXIST "%ProgramFiles%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles%\Gallio\bin\Gallio.Echo.exe"
IF EXIST "%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe"


%GALLIO% /rnf:Tests /rt:Xml-Inline /report-directory:reports ..\bin\Test.Mosa.Runtime.CompilerFramework.dll

CALL ExtractResults.BAT

pause
