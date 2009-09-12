PATH=C:\Program Files\Mono-2.4.2.1\Bin;%PATH%

call gmcs -lib:Bin -target:exe -unsafe -out:Bin\Mosa.HelloWorld.exe HelloWorld\*.cs 

pause
