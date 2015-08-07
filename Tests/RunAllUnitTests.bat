
SET XUNIT="..\Source\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.x86.exe"

if not exist results mkdir results

%XUNIT% Mosa.UnitTests.xunit


