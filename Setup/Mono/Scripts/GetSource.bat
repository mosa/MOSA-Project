if exist "src\mono-%1.tar.bz2" goto end

mkdir src

if not exist "..\..\..\patches\mono\mono-%1.tar.bz2" goto next

copy "..\..\..\patches\mono\mono-%1.tar.bz2" "src\mono-%1.tar.bz2"

goto end

:next

..\..\Tools\wget\wget.exe "http://ftp.novell.com/pub/mono/sources/mono/mono-%1.tar.bz2" -O "src\mono-%1.tar.bz2"

:end
