/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	public class MemoryRegion : IMemoryRegion
	{
		protected uint baseAddress;
		protected uint size;

		public uint BaseAddress { get { return baseAddress; } }
		public uint Size { get { return size; } }

		public MemoryRegion(uint baseAddress, uint size)
		{
			this.baseAddress = baseAddress;
			this.size = size;
		}

	}

}
