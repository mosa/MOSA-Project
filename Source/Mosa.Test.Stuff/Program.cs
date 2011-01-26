using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;

namespace Mosa.Test.Stuff
{
	static class Program
	{
		static void Main(string[] args)
		{
			string[] files = { @"X:\MOSA-Project-tgiphil\bin\Mosa.Vm.dll", @"X:\MOSA-Project-tgiphil\bin\mscorlib.dll" };
			
			ITypeSystem typeSystem = new TypeSystem();

			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(files);

			IMetadataModule metadataModule1 = assemblyLoader.LoadModule(files[0]);
			IMetadataModule metadataModule2 = assemblyLoader.LoadModule(files[1]);

			ITypeModule loader = new TypeModule(typeSystem, metadataModule);

			return;
		}
	}
}
