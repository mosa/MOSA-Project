// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem.CLR.Metadata
{
	internal class ClrMetadata : IMetadata
	{
		private readonly ClrModuleLoader moduleLoader;

		public ClrMetadata(ClrModuleLoader loader)
		{
			moduleLoader = loader;
		}

		public TypeSystem TypeSystem { get; private set; }

		public ITypeSystemController Controller { get; private set; }

		public ClrMetadataCache Cache { get; private set; }

		public ClrMetadataLoader Loader { get; private set; }

		public ClrMetadataResolver Resolver { get; private set; }

		public void Initialize(TypeSystem system, ITypeSystemController controller)
		{
			TypeSystem = system;
			Controller = controller;
			Cache = new ClrMetadataCache();
			Loader = new ClrMetadataLoader(this);
			Resolver = new ClrMetadataResolver(this);
		}

		public void LoadMetadata()
		{
			foreach (var module in moduleLoader.Modules)
			{
				Loader.Load(module);
			}

			Controller.SetCorLib(Loader.CorLib);

			Resolver.Resolve();

			foreach (var module in Cache.Modules.Values)
			{
				if (module.EntryPoint != null)
				{
					Controller.SetEntryPoint(module.EntryPoint);
					break;
				}
			}
		}

		public string LookupUserString(MosaModule module, uint id)
		{
			return Cache.GetStringById(id);
		}
	}
}
