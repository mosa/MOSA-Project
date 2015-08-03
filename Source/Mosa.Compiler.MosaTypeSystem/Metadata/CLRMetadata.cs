// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	internal class CLRMetadata : IMetadata
	{
		private MosaModuleLoader moduleLoader;

		public CLRMetadata(MosaModuleLoader loader)
		{
			this.moduleLoader = loader;
		}

		public TypeSystem TypeSystem { get; private set; }

		public ITypeSystemController Controller { get; private set; }

		public MetadataCache Cache { get; private set; }

		public MetadataLoader Loader { get; private set; }

		public MetadataResolver Resolver { get; private set; }

		public void Initialize(TypeSystem system, ITypeSystemController controller)
		{
			TypeSystem = system;
			Controller = controller;
			Cache = new MetadataCache();
			Loader = new MetadataLoader(this);
			Resolver = new MetadataResolver(this);
		}

		public void LoadMetadata()
		{
			foreach (var module in moduleLoader.Modules)
				Loader.Load(module);

			Controller.SetCorLib(Loader.CorLib);

			Resolver.Resolve();

			foreach (var module in Cache.Modules.Values)
				if (module.EntryPoint != null)
				{
					Controller.SetEntryPoint(module.EntryPoint);
					break;
				}
		}

		public string LookupUserString(MosaModule module, uint id)
		{
			return Cache.GetStringById(id);
		}
	}
}