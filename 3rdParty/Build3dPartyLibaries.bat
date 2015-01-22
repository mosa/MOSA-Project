if exist %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319 set MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319
set MSBUILD=%MSBUILDPATH%\msbuild.exe

set GIT="C:\Program Files (x86)\Git\bin\git.exe" 

rmdir /q /s source
mkdir source

%GIT% clone --branch v1.0.2-mosa --depth 1 https://github.com/mosa/dnlib.git source/dnlib
%GIT% clone --branch v3.0-alpha8-mosa  --depth 1 https://github.com/mosa/dockpanelsuite.git source/dockpanelsuite

%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Release /p:Platform="Any CPU" source/dnlib\dnlib.sln

cd source/dockpanelsuite
CALL all.bat
cd ..

copy dnlib\Release\bin\dnlib.dll ..
copy dockpanelsuite\bin\net40\WeifenLuo.WinFormsUI.Docking.dll ..

pause

