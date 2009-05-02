
del /Q /F /S build\class > NUL
rd /Q /F build\class > NUL

..\..\Mosa\bin\Mosa.Tools.Mono.TransformSource.exe "src\mono-%1\mcs\class" build\class > NUL

rem ..\..\Mosa\bin\Mosa.Tools.Mono.CreateProjects.exe "src\mono-%1\mcs\class" build\class
