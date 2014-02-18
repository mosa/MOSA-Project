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
using dnlib.DotNet;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaModule
	{
		public ModuleDefMD InternalModule { get; private set; }

		public MosaMethod EntryPoint { get; internal set; }

		public string Name { get; private set; }

		// Key is ( Type namespace, Type full name )
		public IDictionary<Tuple<string, string>, MosaType> Types { get; private set; }
		
		public MosaModule(ModuleDefMD module)
		{
			this.InternalModule = module;
			this.Name = module.Name;
			Types = new Dictionary<Tuple<string, string>, MosaType>();
		}

		public MosaModule(string name)
		{
			this.InternalModule = null;
			this.Name = name;
			Types = new Dictionary<Tuple<string, string>, MosaType>();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}