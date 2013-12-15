using Mosa.Compiler.Metadata.Loader;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeSystem
	{
		public MosaTypeResolver Resolver { get; internal set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		public MosaTypeSystem()
		{
			Resolver = new MosaTypeResolver();
			this.BuiltIn = Resolver.BuiltIn;
		}

		public void LoadAssembly(IMetadataModule metadataModule)
		{
			MosaTypeLoader.Load(metadataModule, Resolver);
		}

		public void Load(MosaAssemblyLoader assemblyLoader)
		{
			foreach (var module in assemblyLoader.Modules)
			{
				LoadAssembly(module);
			}
		}
	}
}