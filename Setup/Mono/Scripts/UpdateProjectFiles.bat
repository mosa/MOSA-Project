
copy Projects\Mono.sln "build\mono-%1\mcs\class\Mono.sln"

CALL scripts\UpdateProject.bat %1 corlib.csproj
CALL scripts\UpdateProject.bat %1 System.csproj
CALL scripts\UpdateProject.bat %1 System.XML.csproj

