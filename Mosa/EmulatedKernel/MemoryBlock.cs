/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.EmulatedKernel
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
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public byte Read8(uint index)
		{
			return MemoryDispatch.Read8((uint)(address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write8(uint index, byte value)
		{
			MemoryDispatch.Write8((uint)(address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public ushort Read16(uint index)
		{
			return MemoryDispatch.Read16((uint)(address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write16(uint index, ushort value)
		{
			MemoryDispatch.Write16((uint)(address + index), value);
		}

        /// <summary>
        /// Reads the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ushort Read24(uint index)
        {
            return MemoryDispatch.Read24((uint)(address + index));
        }

        /// <summary>
        /// Writes the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Write24(uint index, ushort value)
        {
            MemoryDispatch.Write24((uint)(address + index), value);
        }

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint Read32(uint index)
		{
			return MemoryDispatch.Read16((uint)(address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write32(uint index, uint value)
		{
			MemoryDispatch.Write32((uint)(address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public uint Read32(uint index, byte count)
		{
			uint offset = address + index;
			uint value = 0;

			if (count == 1)
				return MemoryDispatch.Read8(offset);

			while (count > 0) {
				value = (value >> 8) | MemoryDispatch.Read8(offset);
				count--;
				offset++;
			}

			return value;
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		/// <param name="count">The count.</param>
		public void Write32(uint index, uint value, byte count)
		{
			uint offset = address + index;

			if (count == 1) {
				MemoryDispatch.Write8(offset, (byte)value);
				return;
			}

			while (count > 0) {
				MemoryDispatch.Write8(offset, (byte)(value & 0xFF));
				value = value >> 8;
				count--;
				offset++;
			}
		}

	}
}
