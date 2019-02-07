#cp ../3rdParty/*.dll ../bin/
cd $(dirname $0) #Go to directory containing this script, if called from elsewhre
mono ../Tools/nuget/nuget.exe restore
msbuild Mosa.Tool.Mosactl.sln /m
msbuild Mosa.sln /m
