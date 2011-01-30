cd ..\..\Tools\JPC
call GetJPC.bat
"%ProgramFiles(x86)%\Java\jre6\bin\java.exe" -jar JPCDebugger.jar -hda ..\..\Run\build\bootimage.img -boot hda
