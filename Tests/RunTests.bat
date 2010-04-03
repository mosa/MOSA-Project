"%ProgramFiles%\Gallio\bin\Gallio.Echo.exe" /rnf:Tests /rt:Xml-Inline /report-directory:reports ..\bin\Test.Mosa.Runtime.CompilerFramework.dll

CALL ExtractResults.BAT

pause
