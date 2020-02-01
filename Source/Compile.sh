cd $(dirname $0) #Go to directory containing this script, if called from elsewhere

if grep -q Microsoft /proc/version; then
  echo "Detected Windows (WSL)"
  
  cmd.exe /C Compile.bat
  exit
fi

echo "Detected Native Linux"

#mono ../Tools/nuget/nuget.exe restore Mosa.Tool.Mosactl.sln
mono ../Tools/nuget/nuget.exe restore Mosa.sln

#msbuild Mosa.Tool.Mosactl.sln /m
msbuild Mosa.sln /m
