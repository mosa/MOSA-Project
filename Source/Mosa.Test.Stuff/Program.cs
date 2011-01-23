using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.TypeLoader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;

namespace Mosa.Test.Stuff
{
	static class Program
	{
		static void Main(string[] args)
		{
			string[] files = { @"X:\MOSA-Project-tgiphil\bin\Mosa.Test.Quick.exe" };

			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(files);

			IMetadataModule metadataModule = assemblyLoader.LoadModule(files[0]);

			Loader loader = new Loader(metadataModule.Metadata);

			return;
		}
	}
}
