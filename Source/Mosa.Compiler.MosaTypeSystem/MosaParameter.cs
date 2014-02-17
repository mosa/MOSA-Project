/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaParameter
	{
		internal MosaParameter(string name, MosaType type)
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

		public bool Matches(MosaParameter parameter)
		{
			return Type.Matches(parameter.Type);
		}

		public bool Matches(MosaType type)
		{
			return Type.Matches(type);
		}
	}
}