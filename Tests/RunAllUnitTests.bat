
CD ..\Source
CALL Compile-Debug.bat
CD ..\Tests

SET XUNIT="..\Tools\xunit\xunit.console.clr4.x86.exe"

%XUNIT% Mosa.UnitTests.xunit
