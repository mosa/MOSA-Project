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
		private ulong address;
		private ulong size;
		private ulong end;

		public MemoryBlock(ulong address, ulong size)
		{
			this.address = address;
			this.size = size;
			this.end = address + size - 1;
		}

		public ulong Address { get { return address; } }
		public ulong Size { get { return size; } }

		public byte this[ulong index]
		{
			get
			{
				return MemoryDispatch.Read8((ulong)(address + index));
			}
			set
			{
				MemoryDispatch.Write8((ulong)(address + index), value);
			}
		}

		public byte Read8(ulong index)
		{
			return MemoryDispatch.Read8((ulong)(address + index));
		}

		public void Write8(ulong index, byte value)
		{
			MemoryDispatch.Write8((ulong)(address + index), value);
		}

	}
}
