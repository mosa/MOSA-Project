
echo "#### Compile the Solution First!!! ####"

mkdir output

rm Kernel/hello.exe

cd output

../../Mosa/Bin/mosacl.exe -a=x86 -f=PE --pe-file-alignment=4096 --map=hello.map -b=mb0.7 -o ../Kernel/hello.exe ../../Mosa/Bin/Mosa.HelloWorld.exe

cd ..

