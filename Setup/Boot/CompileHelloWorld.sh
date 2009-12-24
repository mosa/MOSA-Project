mkdir build
rm ./build/hello.exe
mono ../../Bin/mosacl.exe -a=x86 -f=PE -pe-file-alignment=4096 -b=mb0.7 -o ./build/hello.exe ../../Bin/Mosa.HelloWorld.exe
