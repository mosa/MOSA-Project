if exist %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe set MSBUILD=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
if exist %SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe set MSBUILD=%SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" set MSBUILD="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\msbuild.exe" set MSBUILD="%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\msbuild.exe"

..\Tools\nuget\nuget.exe restore MOSA.sln

%MSBUILD% /nologo /m /p:BuildInParallel=true /p:Configuration=Debug /p:Platform="Any CPU" MOSA.sln
