/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaParameter : IEquatable<MosaParameter>, IEquatable<MosaType>
	{
		public MosaParameter(string name, MosaType type)
		{
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; private set; }

		public MosaType Type { get; private set; }

		public override string ToString()
		{
			return Type + " " + Name;
		}

		public bool Equals(MosaParameter parameter)
		{
			return Type.Equals( parameter.Type);
		}

		public bool Equals(MosaType type)
		{
			return Type.Equals(type);
		}
	}
}