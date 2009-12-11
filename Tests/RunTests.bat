"%ProgramFiles%\Gallio\bin\Gallio.Echo.exe" /rnf:Tests /rt:Xml-Inline /report-directory:Reports ..\bin\Test.Mosa.Runtime.CompilerFramework.dll

CALL ExtractResults.BAT

pause
