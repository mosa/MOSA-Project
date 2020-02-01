rem set INNOSETUP=C:\Program Files (x86)\Inno Setup 5\ISCC.exe
set INNOSETUP=C:\Program Files (x86)\Inno Setup 6\ISCC.exe

if exist "%INNOSETUP%" "%INNOSETUP%" /DMyAppVersion=2.0.0 Mosa-Installer.iss
