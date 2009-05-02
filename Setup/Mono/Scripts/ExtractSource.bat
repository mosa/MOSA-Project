
if exist "src\mono-%1.tar" goto next

..\..\Tools\7Zip\7za.exe x -y -osrc src\mono-%1.tar.bz2

:next

if exist "src\mono-%1" goto end

del /Q /F /S "src\mono-%1" > NUL

..\..\Tools\7Zip\7za.exe x -y -osrc "src\mono-%1.tar" -ir!*.cs -ir!*.sources -x!docs -x!samples -x!tools -x!mono > NUL

:end
