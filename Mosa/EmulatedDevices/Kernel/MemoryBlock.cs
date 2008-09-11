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
    /// <summary>
    /// 
    /// </summary>
	public class MemoryBlock : IMemory
	{
        /// <summary>
        /// 
        /// </summary>
		private uint address;

        /// <summary>
        /// 
        /// </summary>
		private uint size;

        /// <summary>
        /// 
        /// </summary>
		private uint end;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
		public MemoryBlock(uint address, uint size)
		{
			this.address = address;
			this.size = size;
			this.end = address + size - 1;
		}

        /// <summary>
        /// 
        /// </summary>
		public uint Address { get { return address; } }

        /// <summary>
        /// 
        /// </summary>
		public uint Size { get { return size; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public byte Read8(uint index)
		{
			return MemoryDispatch.Read8((uint)(address + index));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
		public void Write8(uint index, byte value)
		{
			MemoryDispatch.Write8((uint)(address + index), value);
		}

	}
}
