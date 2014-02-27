/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaLocal : IEquatable<MosaLocal>, IEquatable<MosaType>
	{
		public string Name { get; private set; }
		public MosaType Type { get; private set; }
		public bool IsPinned { get; private set; }

		public MosaLocal(string name, MosaType type, bool isPinned)
		{
			this.Name = name;
			this.Type = type;
			this.IsPinned = isPinned;
		}

		public override string ToString()
		{
			return Type + " " + Name + (IsPinned ? " [Pinned]" : "");
		}

		public bool Equals(MosaLocal local)
		{
			return Type.Equals(local.Type);
		}

		public bool Equals(MosaType type)
		{
			return Type.Equals(type);
		}
	}
}
