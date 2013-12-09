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
	public class MosaAssembly
	{
		public string Name { get; internal set; }

		public MosaAssembly()
		{
		}

		public MosaAssembly(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}