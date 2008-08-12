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
	public class MemoryRegion
	{
		protected MemorySpace memorySpace;
		protected uint baseAddress;
		protected uint size;

		public uint BaseAddress { get { return baseAddress; } }
		public uint Size { get { return size; } }

		public MemoryRegion(MemorySpace memorySpace, uint baseAddress, uint size)
		{
			this.memorySpace = memorySpace;
			this.baseAddress = baseAddress;
			this.size = size;
		}

		public byte[] GetMemory()
		{
			return memorySpace.GetMemory(baseAddress);
		}

		public byte[] GetMemory(uint offset)
		{
			return memorySpace.GetMemory(baseAddress + offset);
		}
	}

}
