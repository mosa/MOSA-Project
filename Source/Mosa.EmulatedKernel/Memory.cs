/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.Platform.x86;

namespace Mosa.EmulatedKernel
{
	/// <summary>
	/// 
	/// </summary>
	public class Memory : IMemory
	{
		private uint _address;
		private uint _size;
		//private uint _end;

		/// <summary>
		/// Initializes a new instance of the <see cref="Memory"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public Memory(uint address, uint size)
		{
			_address = address;
			_size = size;
			//_end = address + size - 1;
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public uint Address { get { return _address; } }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public uint Size { get { return _size; } }

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public byte this[uint index]
		{
			get
			{
				return MemoryDispatch.Read8((uint)(_address + index));
			}
			set
			{
				MemoryDispatch.Write8((uint)(_address + index), value);
			}
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public byte Read8(uint index)
		{
			return MemoryDispatch.Read8((uint)(_address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write8(uint index, byte value)
		{
			MemoryDispatch.Write8((uint)(_address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public ushort Read16(uint index)
		{
			return MemoryDispatch.Read16((uint)(_address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write16(uint index, ushort value)
		{
			MemoryDispatch.Write16((uint)(_address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint Read24(uint index)
		{
			return MemoryDispatch.Read24((uint)(_address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write24(uint index, uint value)
		{
			MemoryDispatch.Write24((uint)(_address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint Read32(uint index)
		{
			return MemoryDispatch.Read16((uint)(_address + index));
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write32(uint index, uint value)
		{
			MemoryDispatch.Write32((uint)(_address + index), value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public uint Read32(uint index, byte count)
		{
			uint offset = _address + index;
			uint value = 0;

			if (count == 1)
				return MemoryDispatch.Read8(offset);

			while (count > 0)
			{
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
			uint offset = _address + index;

			if (count == 1)
			{
				MemoryDispatch.Write8(offset, (byte)value);
				return;
			}

			while (count > 0)
			{
				MemoryDispatch.Write8(offset, (byte)(value & 0xFF));
				value = value >> 8;
				count--;
				offset++;
			}
		}

	}
}
