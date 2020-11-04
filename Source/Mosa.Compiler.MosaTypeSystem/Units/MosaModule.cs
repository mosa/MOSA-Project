// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public sealed class MosaModule : MosaUnit
	{
		public string Assembly { get; private set; }

		public bool IsReflectionOnly { get; private set; }

		public MosaMethod EntryPoint { get; private set; }

		public IDictionary<uint, MosaType> Types { get; private set; }

		internal MosaModule()
		{
			Types = new Dictionary<uint, MosaType>();
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private readonly MosaModule module;

			internal Mutator(MosaModule module)
				: base(module)
			{
				this.module = module;
			}

			public string Assembly { set { module.Assembly = value; } }

			public bool IsReflectionOnly { set { module.IsReflectionOnly = value; } }

			public MosaMethod EntryPoint { set { module.EntryPoint = value; } }

			public override void Dispose()
			{
				module.FullName = module.Name;
				module.ShortName = module.Name;
			}
		}
	}
}
