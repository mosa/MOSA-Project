copy "Projects\%2" "build\mono-%1\mcs\class\%2"

..\..\Mosa\bin\Mosa.Tools.Mono.TransformProjectFiles.exe "build\mono-%1\mcs\class\%2" > NUL
..\..\Mosa\bin\Mosa.Tools.Mono.UpdateProject.exe "build\mono-%1\mcs\class\%2" > NUL
