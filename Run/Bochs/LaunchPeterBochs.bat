set BOCHS=%CD%\..\..\Tools\Bochs
set SETTINGS=%CD%

cd ..\..\Tools\peter-bochs
call GetPeterBochs.bat

java -jar peter-bochs-debugger.jar %BOCHS%\bochsdbg.exe -q -f %SETTINGS%\peter-bochsrc.bxrc
