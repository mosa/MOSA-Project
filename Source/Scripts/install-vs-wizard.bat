if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64"
if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin"
if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\x64" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\x64"
if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"
if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\x64"
if exist "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools" set GACUTILPATH="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"

set SOURCE=%~dp0

%GACUTILPATH%\gacutil.exe -i "%SOURCE%\bin\Mosa.VisualStudio.Wizard.dll"

pause
