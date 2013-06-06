call ..\FindJava.bat

cd ..\..\Tools\JPC
call GetJPC.bat

%JAVABIN% -jar JPCApplication.jar -hda ..\..\Run\build\bootimage.img -boot hda

pause