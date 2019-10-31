// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			Name = name;
			Type = type;
			IsPinned = isPinned;
		}

		public override string ToString()
		{
			return Name + (IsPinned ? " [Pinned]" : "") + " " + Type;
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
