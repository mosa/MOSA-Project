
cd ..\bin

SET XUNIT="..\Source\packages\xunit.runner.console.2.0.0\tools\xunit.console.x86.exe"

echo %DATE% %TIME%

%XUNIT% Mosa.Compiler.Framework.xUnit.dll
echo %DATE% %TIME%

%XUNIT% Mosa.Test.Collection.x86.xUnit.dll
echo %DATE% %TIME%
