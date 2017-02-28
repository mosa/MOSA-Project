
cd ..\bin

SET XUNIT="..\Source\packages\xunit.runner.console.2.2.0\tools\xunit.console.x86.exe"

echo %DATE% %TIME%

%XUNIT% Mosa.Compiler.Framework.xUnit.dll
echo %DATE% %TIME%

%XUNIT% Mosa.UnitTest.Collection.xUnit.dll
echo %DATE% %TIME%
