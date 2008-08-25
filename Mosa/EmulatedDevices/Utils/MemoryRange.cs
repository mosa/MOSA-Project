/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.EmulatedDevices.Utils
{

	public class MemoryRange
	{
		public ulong address;
		public ulong size;
		public MemoryRead8 read8;
		public MemoryWrite8 write8;

		public MemoryRange(ulong address, ulong size, MemoryRead8 read8, MemoryWrite8 write8)
		{
			this.address = address;
			this.size = size;
			this.read8 = read8;
			this.write8 = write8;
		}

		public bool Contains(ulong address)
		{
			return ((address >= this.address) && (address < (this.address + this.size)));
		}
	}

}
