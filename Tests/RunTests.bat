
CD ..\Source
CALL Compile-Debug.bat
CD ..\Tests

IF EXIST "%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe"
IF EXIST "%ProgramFiles%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles%\Gallio\bin\Gallio.Echo.exe"

IF NOT EXIST reports MKDIR reports

CD ..\bin

%GALLIO% /rnf:Tests /rt:Xml-Inline /report-directory:..\Tests\reports Mosa.Test.Cases.dll "/filter:Namespace:Mosa.Test.Cases.CIL"

CD ..\Tests

CALL ExtractResults.BAT

notepad.exe reports\Failed.txt

pause
