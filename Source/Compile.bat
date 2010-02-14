set MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v3.5
set MSBUILD=%MSBUILDPATH%\msbuild.exe

%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Release /p:Platform="Any CPU" mosa.sln
