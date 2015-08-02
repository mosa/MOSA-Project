/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
	public class SimSymbol
	{
		public string Name { get; private set; }

		public ulong Address { get; private set; }

		public ulong Size { get; private set; }

		public ulong EndAddress { get { return Size == 0 ? Address : Address + Size - 1; } }

		public SimSymbol(string name, ulong address, ulong size)
		{
			Name = name;
			Address = address;
			Size = size;
		}

		public override string ToString()
		{
			return "0x" + Address.ToString("X") + " - " + "0x" + EndAddress.ToString("X") + " " + Name + " (" + Size.ToString() + ")";
		}
	}
}
