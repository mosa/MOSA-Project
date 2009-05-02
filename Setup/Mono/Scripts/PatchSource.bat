
mkdir build\build
mkdir build\build\common

copy ProjectFiles\*.sln build\class
copy "src\mono-%1\mcs\build\common\*.cs" "build\build\common"

rem copy ProjectFiles\*.csproj build\class

rem copy Patches\Parser.cs build\class\
rem copy Patches\PatternParser.cs build\class\
rem copy Patches\PatternTokenizer.cs build\class\


