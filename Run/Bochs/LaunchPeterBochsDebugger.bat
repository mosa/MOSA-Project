set BOCHS=%CD%\..\..\Tools\Bochs
set SETTINGS=%CD%


IF EXIST "%ProgramFiles(x86)%\Java\jre6\bin\Java.exe" SET JAVA="%ProgramFiles(x86)%\Java\jre6\bin\Java.exe"
IF EXIST "%ProgramFiles%\Java\jre6\bin\Java.exe" SET JAVA="%ProgramFiles%\Java\jre6\bin\Java.exe"

cd ..\..\Tools\peter-bochs
call GetPeterBochs.bat

%JAVA% -jar peter-bochs-debugger.jar %BOCHS%\bochsdbg.exe -q -f %SETTINGS%\peter-bochsrc.bxrc
