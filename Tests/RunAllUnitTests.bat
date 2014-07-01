
SET XUNIT="..\Tools\xunit\xunit.console.clr4.x86.exe"

if not exist results mkdir results

%XUNIT% Mosa.UnitTests.xunit


