mkdir ..\..\bin\nupkg
cd ..\..\bin\nupkg

..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Korlib\Mosa.Korlib.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Runtime\Mosa.Runtime.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Kernel.x86\Mosa.Kernel.x86.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Runtime.x86\Mosa.Runtime.x86.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.ClassLib\Mosa.ClassLib.nuspec

..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Tools.Package\Mosa.Tools.Package.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Tools.Qemu.Package\Mosa.Tools.Qemu.Package.nuspec

pause
