
if exist "src\mono-%1.tar" goto next

..\..\Tools\7Zip\7za.exe x -y -osrc src\mono-%1.tar.bz2

:next

del /Q /F /S "build\mono-%1" > NUL
rd /Q /S "build\mono-%1"

..\..\Tools\7Zip\7za.exe x -y -obuild "src\mono-%1.tar" -ir!*.cs -ir!*.sources -x!docs -x!samples -x!tools -x!mono > NUL

:end
