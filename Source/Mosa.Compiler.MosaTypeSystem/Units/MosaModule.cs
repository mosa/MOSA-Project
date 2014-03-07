/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaModule : MosaUnit
	{
		public string Assembly { get; private set; }
		public MosaMethod EntryPoint { get; private set; }

		public IDictionary<uint, MosaType> Types { get; private set; }

		internal MosaModule()
		{
			Types = new Dictionary<uint, MosaType>();
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			MosaModule module;
			internal Mutator(MosaModule module)
				: base(module)
			{
				this.module = module;
			}

			public string Assembly { set { module.Assembly = value; } }
			public MosaMethod EntryPoint { set { module.EntryPoint = value; } }

			public override void Dispose()
			{
				module.FullName = module.Name;
				module.ShortName = module.Name;
			}
		}
	}
}