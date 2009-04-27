
echo "#### Compile the Solution First!!! ####"

mkdir build

rm build/hello.exe

cd build

../../../Mosa/Bin/mosacl.exe -a=x86 -f=PE --pe-file-alignment=4096 --map=hello.map -b=mb0.7 -o ../build/hello.exe ../../../Mosa/Bin/Mosa.HelloWorld.exe

cd ..

