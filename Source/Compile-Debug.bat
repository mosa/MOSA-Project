if exist %SYSTEMROOT%\Microsoft.NET\Framework\v3.5 set MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v3.5
if exist %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319 set MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319

set MSBUILD=%MSBUILDPATH%\msbuild.exe

%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Debug /p:Platform="Any CPU" MOSA-VS2012.sln
