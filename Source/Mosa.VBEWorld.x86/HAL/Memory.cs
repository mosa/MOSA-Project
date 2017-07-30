// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;
using Mosa.Runtime.x86;

namespace Mosa.VBEWorld.x86.HAL
{
	public sealed class Memory : IMemory
	{
		private uint address;
		private uint size;

		public Memory(uint address, uint size)
		{
			this.address = address;
			this.size = size;
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		uint IMemory.Address { get { return address; } }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		uint IMemory.Size { get { return size; } }

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		byte IMemory.this[uint index]
		{
			get { return Native.Get8(address + index); }
			set { Native.Set8(address + index, value); }
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		byte IMemory.Read8(uint index)
		{
			return Native.Get8(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void IMemory.Write8(uint index, byte value)
		{
			Native.Set8(address + index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		ushort IMemory.Read16(uint index)
		{
			return Native.Get16(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void IMemory.Write16(uint index, ushort value)
		{
			Native.Set16(address + index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		uint IMemory.Read24(uint index)
		{
			return Native.Get16(address + index) | (uint)(Native.Get8(address + index + 2) << 16);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void IMemory.Write24(uint index, uint value)
		{
			Native.Set16(address + index, (ushort)(value & 0xFFFF));
			Native.Set8(address + index + 2, (byte)((value >> 16) & 0xFF));
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		uint IMemory.Read32(uint index)
		{
			return Native.Get32(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void IMemory.Write32(uint index, uint value)
		{
			Native.Set32(address + index, value);
		}
	}
}
