cd ..\..\..\Tools\JPC
call GetJPC.bat
java -jar JPCDebugger.jar -hda ..\..\Setup\Boot\build\bootimage.img -boot hda
