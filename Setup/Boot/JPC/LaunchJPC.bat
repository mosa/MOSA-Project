cd ..\..\..\Tools\JPC
call GetJPC.bat
java -jar JPCApplication.jar -hda ..\..\Setup\Boot\build\bootimage.img -boot hda
