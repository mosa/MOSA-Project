/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.EmulatedDevices.Kernel
{
	public class MemoryBlock : IMemory
	{
		private uint address;
		private uint size;

		public MemoryBlock(uint address, uint size)
		{
			this.address = address;
			this.size = size;
		}

		public uint Address { get { return address; } }
		public uint Size { get { return size; } }

		public byte this[long index]
		{
			get
			{
				return MemoryDispatch.Read8((uint)(address + index));
			}
			set
			{
				MemoryDispatch.Write8((uint)(address + index), value);
			}
		}

		public byte this[uint index]
		{
			get
			{
				return MemoryDispatch.Read8(address + index);
			}
			set
			{
				MemoryDispatch.Write8(address + index, value);
			}
		}
	}
}
