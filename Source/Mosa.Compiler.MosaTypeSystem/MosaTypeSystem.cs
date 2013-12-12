using Mosa.Compiler.Metadata.Loader;
using System.IO;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeSystem
	{
		public MosaTypeResolver Resolver { get; internal set; }

		private MosaTypeLoader loader;

		public MosaTypeSystem()
		{
			Resolver = new MosaTypeResolver();
			this.loader = new MosaTypeLoader(Resolver);
		}

		public void LoadAssembly(IMetadataModule metadataModule)
		{
			loader.Load(metadataModule);
		}

		public void Load(MosaAssemblyLoader assemblyLoader)
		{
			foreach(var module in assemblyLoader.Modules)
			{
				LoadAssembly(module);
			}
		}

	}
}