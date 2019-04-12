// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Provides indirect access to a block of memory
	/// </summary>
	public readonly struct Pointer
	{
		private readonly IntPtr address;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public IntPtr Address { get { return address; } }

		public Pointer(IntPtr address)
		{
			this.address = address;
		}

		public Pointer(Pointer pointer)
		{
			this.address = pointer.address;
		}

		public Pointer(Pointer pointer, int offset)
		{
			this.address = pointer.address + offset;
		}

		public byte this[uint offset]
		{
			get { return Intrinsic.Load8(address, offset); }
			set { Intrinsic.Store8(address, offset, value); }
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public byte Read8(uint offset)
		{

			return Intrinsic.Load8(address, offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void Write8(uint offset, byte value)
		{

			Intrinsic.Store8(address, offset, value);
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public ushort Read16(uint offset)
		{

			return Intrinsic.Load16(address, offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void Write16(uint offset, ushort value)
		{

			Intrinsic.Store16(address, offset, value);
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public uint Read24(uint offset)
		{

			return Intrinsic.Load16(address, offset) | (uint)(Intrinsic.Load8(address, offset + 2) << 16);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void Write24(uint offset, uint value)
		{
			Intrinsic.Store16(address, offset, (ushort)(value & 0xFFFF));
			Intrinsic.Store8(address, offset + 2, (byte)((value >> 16) & 0xFF));
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public uint Read32(uint offset)
		{

			return Intrinsic.Load32(address, offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void Write32(uint offset, uint value)
		{

			Intrinsic.Store32(address, offset, value);
		}
	}
}
