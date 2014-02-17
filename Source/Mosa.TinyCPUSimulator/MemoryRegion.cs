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
	public class MemoryRegion
	{
		public ulong Address { get; private set; }

		public ulong End { get { return Address + Size; } }

		public ulong Size { get; private set; }

		public uint Type { get; private set; }

		public MemoryRegion(ulong address, ulong size, uint type)
		{
			Address = address;
			Size = size;
			Type = type;
		}

		public MemoryRegion(ulong address, ulong size)
			: this(address, size, 1)
		{
		}

		public bool Contains(ulong address)
		{
			return address >= Address && address < (Address + Size);
		}

		public override string ToString()
		{
			return Address.ToString("X") + " - " + (Address + Size).ToString("X");
		}
	}
}