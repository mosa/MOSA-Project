
CD ..\Source
rem CALL Compile-Debug.bat
CD ..\Tests

IF EXIST "%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles(x86)%\Gallio\bin\Gallio.Echo.exe"
IF EXIST "%ProgramFiles%\Gallio\bin\Gallio.Echo.exe" SET GALLIO="%ProgramFiles%\Gallio\bin\Gallio.Echo.exe"

IF NOT EXIST reports MKDIR reports

IF NOT EXIST bin MKDIR bin

DELETE bin\*.dll

COPY ..\bin\*.dll bin

CD bin

%GALLIO% /rnf:Tests /rt:Xml-Inline /report-directory:..\reports Mosa.Test.Cases.dll "/filter:Namespace:Mosa.Test.Cases.CIL"

CD ..

CALL ExtractResults.BAT

notepad.exe reports\Failed.txt

pause
