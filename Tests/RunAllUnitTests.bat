
cd ..\bin

SET XUNIT="..\Source\packages\xunit.runner.console.2.3.1\tools\net452\xunit.console.x86.exe"

echo %DATE% %TIME%

%XUNIT% Mosa.Compiler.Framework.xUnit.dll
echo %DATE% %TIME%

%XUNIT% Mosa.UnitTest.Collection.xUnit.dll -parallel all
echo %DATE% %TIME%
