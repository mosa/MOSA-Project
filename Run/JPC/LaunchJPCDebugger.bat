call ..\FindJava.bat

cd ..\..\Tools\JPC
call GetJPC.bat

%JAVABIN% -jar JPCDebugger.jar -hda ..\..\Run\build\bootimage.img -boot hda
