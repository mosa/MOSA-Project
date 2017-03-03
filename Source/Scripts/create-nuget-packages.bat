mkdir ..\..\bin\nupkg
cd ..\..\bin\nupkg

..\..\Tools\nuget\nuget.exe pack ..\..\Source\Korlib\Korlib.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Runtime\Mosa.Runtime.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Kernel.x86\Mosa.Kernel.x86.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Runtime.x86\Mosa.Runtime.x86.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.ClassLib\Mosa.ClassLib.nuspec

..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Compiler.Framework\Mosa.Compiler.Framework.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Tool.Explorer\Mosa.Tool.Explorer.nuspec
..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Tool.Compiler\Mosa.Tool.Compiler.nuspec

..\..\Tools\nuget\nuget.exe pack ..\..\Source\Mosa.Tool.Launcher\Mosa.Tool.Launcher.nuspec

pause
