if exist %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319 set MSBUILDPATH=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319
if exist "%ProgramFiles(x86)%\MSBuild\14.0\Bin" set MSBUILDPATH="%ProgramFiles(x86)%\MSBuild\14.0\Bin"

set MSBUILD=%MSBUILDPATH%\msbuild.exe

..\Tools\nuget\nuget.exe restore MOSA.sln

%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Release /p:Platform="Any CPU" MOSA.sln
