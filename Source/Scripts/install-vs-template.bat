mkdir "%USERPROFILE%\Documents\Visual Studio 2017\Templates\ProjectTemplates\MOSA Project"
mkdir "%USERPROFILE%\Documents\Visual Studio 2019\Templates\ProjectTemplates\MOSA Project"

xcopy /E /C /Y /Q "..\..\bin\MOSA Project" "%USERPROFILE%\Documents\Visual Studio 2017\Templates\ProjectTemplates\MOSA Project"
xcopy /E /C /Y /Q "..\..\bin\MOSA Project" "%USERPROFILE%\Documents\Visual Studio 2019\Templates\ProjectTemplates\MOSA Project"
