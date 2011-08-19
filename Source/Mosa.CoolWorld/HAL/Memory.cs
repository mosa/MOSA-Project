/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.CoolWorld.HAL
{
	public class Memory : IMemory
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
		public uint Address { get { return address; } }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public uint Size { get { return size; } }

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		public byte this[uint index]
		{
			get { return Read8(index); }
			set { Write8(index, value); }
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public byte Read8(uint index)
		{
			return Native.Get8(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write8(uint index, byte value)
		{
			Native.Set8(address + index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public ushort Read16(uint index)
		{
			return Native.Get16(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write16(uint index, ushort value)
		{
			Native.Set16(address + index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public uint Read32(uint index)
		{
			return Native.Get32(address + index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public void Write32(uint index, uint value)
		{
			Native.Set32(address + index, value);
		}

	}

}
