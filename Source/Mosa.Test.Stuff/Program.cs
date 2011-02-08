using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Test.Stuff
{
	static class Program
	{

		static void Main(string[] args)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.LoadModule(@"X:\MOSA-Project-tgiphil\bin\Mosa.HelloWorld.exe");

			ITypeSystem typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);

			return;
		}
	}
}
