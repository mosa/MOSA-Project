/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata.Loader;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeSystem
	{
		public MosaTypeResolver Resolver { get; internal set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		public IList<MosaType> AllTypes { get { return Resolver.Types; } }

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