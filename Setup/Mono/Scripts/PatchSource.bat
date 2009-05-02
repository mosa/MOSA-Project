mkdir build\build
mkdir build\build\common

copy "src\mono-%1\mcs\build\common\*.cs" "build\build\common"

copy ProjectFiles\*.sln build\class
copy ProjectFiles\*.csproj build\class

copy Patches\Parser.cs build\class\System.XML\System.Xml.XPath
copy Patches\PatternParser.cs build\class\System.XML\System.Xml.XPath
copy Patches\PatternTokenizer.cs build\class\System.XML\System.Xml.XPath

CALL "Patches\Scripts\PatchSource-%1.bat"
