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
			string[] files = { @"X:\MOSA-Project-tgiphil\bin\mscorlib.dll", @"X:\MOSA-Project-tgiphil\bin\Mosa.Platform.X86.Intrinsic.dll", @"X:\MOSA-Project-tgiphil\bin\Mosa.Kernel.dll" };

			ITypeSystem typeSystem = new TypeSystem();

			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(files);

			foreach (string file in files)
			{
				IMetadataModule metadataModule = assemblyLoader.LoadModule(file);

				typeSystem.LoadModule(metadataModule);
			}

			return;
		}
	}
}
