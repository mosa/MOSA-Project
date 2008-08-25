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
		private uint end;

		public MemoryBlock(uint address, uint size)
		{
			this.address = address;
			this.size = size;
			this.end = address + size - 1;
		}

		public uint Address { get { return address; } }
		public uint Size { get { return size; } }

		public byte this[uint index]
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

		public byte Read8(uint index)
		{
			return MemoryDispatch.Read8((uint)(address + index));
		}

		public void Write8(uint index, byte value)
		{
			MemoryDispatch.Write8((uint)(address + index), value);
		}

	}
}
