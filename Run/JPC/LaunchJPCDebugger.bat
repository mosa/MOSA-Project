cd ..\..\Tools\JPC
call GetJPC.bat

IF EXIST "%ProgramFiles(x86)%\Java\jre6\bin\java.exe" SET JAVABIN="%ProgramFiles(x86)%\Java\jre6\bin\java.exe"
IF EXIST "%ProgramFiles%\Java\jre6\bin\java.exe" SET JAVABIN="%ProgramFiles%\Java\jre6\bin\java.exe"

IF EXIST "%ProgramFiles(x86)%\Java\jre7\bin\java.exe" SET JAVABIN="%ProgramFiles(x86)%\Java\jre7\bin\java.exe"
IF EXIST "%ProgramFiles%\Java\jre7\bin\java.exe" SET JAVABIN="%ProgramFiles%\Java\jre7\bin\bin\java.exe"

IF EXIST "%ProgramFiles(x86)%\Java\jdk1.7.0\bin\java.exe" SET JAVABIN="%ProgramFiles(x86)%\Java\jdk1.7.0\bin\java.exe"
IF EXIST "%ProgramFiles%\Java\jdk1.7.0\bin\java.exe" SET JAVABIN="%ProgramFiles%\Java\jdk1.7.0\bin\java.exe"

%JAVABIN% -jar JPCDebugger.jar -hda ..\..\Run\build\bootimage.img -boot hda
