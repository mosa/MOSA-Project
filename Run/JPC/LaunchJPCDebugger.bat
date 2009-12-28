cd ..\..\Tools\JPC
call GetJPC.bat
java -jar JPCDebugger.jar -hda ..\..\Run\build\bootimage.img -boot hda
