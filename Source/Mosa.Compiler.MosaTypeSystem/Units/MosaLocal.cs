// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.MosaTypeSystem.Units
{
	public class MosaLocal : IEquatable<MosaLocal>, IEquatable<MosaType>
	{
		public string Name { get; }

		public MosaType Type { get; }

		public bool IsPinned { get; }

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

		public bool Equals(MosaLocal? local)
		{
			return Type.Equals(local?.Type);
		}

		public bool Equals(MosaType? type)
		{
			return Type.Equals(type);
		}
	}
}
