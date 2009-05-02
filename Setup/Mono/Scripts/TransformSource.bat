
del /Q /F /S build\class > NUL

..\..\Mosa\bin\Mosa.Tools.Mono.TransformSource.exe "src\mono-%1\mcs\class" build\class
..\..\Mosa\bin\Mosa.Tools.Mono.CreateProjects.exe "src\mono-%1\mcs\class" build\class

