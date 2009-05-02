if exist "src\mono-%1.tar.bz2" goto end

..\..\Tools\wget\wget.exe "http://ftp.novell.com/pub/mono/sources/mono/mono-%1.tar.bz2" -O "src\mono-%1.tar.bz2"

rem ..\..\Tools\wget\wget.exe "http://mono.ximian.com/monobuild/snapshot/snapshot_sources/mono/mono-%1.tar.bz2" -O "src\mono-%1.tar.bz2"

:end
