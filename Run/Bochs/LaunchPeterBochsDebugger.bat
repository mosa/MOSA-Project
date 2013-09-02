set BOCHS=%CD%\..\..\Tools\Bochs
set SETTINGS=%CD%

call ..\FindJava.bat

cd ..\..\Tools\peter-bochs
call GetPeterBochs.bat

%JAVABIN% -jar peter-bochs-debugger.jar %BOCHS%\bochsdbg.exe -q -f %SETTINGS%\peter-bochsrc.bxrc
