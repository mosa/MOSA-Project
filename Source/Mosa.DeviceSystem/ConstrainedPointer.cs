// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Provides indirect access to a block of memory
	/// </summary>
	public readonly struct ConstrainedPointer
	{
		private readonly Pointer address;
		private readonly uint size;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public Pointer Address { get { return address; } }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public uint Size { get { return size; } }

		public ConstrainedPointer(Pointer address, uint size)
		{
			this.address = address;
			this.size = size;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckOffset(uint offset)
		{
			if (offset >= size)
			{
				throw new ArgumentOutOfRangeException(nameof(offset));
			}
		}

		public byte this[uint offset]
		{
			get { CheckOffset(offset); return address.Load8(offset); }
			set { CheckOffset(offset); address.Store8(offset, value); }
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Read8(uint offset)
		{
			CheckOffset(offset);
			return address.Load8(offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write8(uint offset, byte value)
		{
			CheckOffset(offset);
			address.Store8(offset, value);
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort Read16(uint offset)
		{
			CheckOffset(offset);
			return address.Load16(offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write16(uint offset, ushort value)
		{
			CheckOffset(offset);
			address.Store16(offset, value);
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Read24(uint offset)
		{
			CheckOffset(offset);
			return address.Load16(offset) | (uint)(address.Load8(offset + 2) << 16);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write24(uint offset, uint value)
		{
			CheckOffset(offset);
			address.Store16(offset, (ushort)(value & 0xFFFF));
			address.Store8(offset + 2, (byte)((value >> 16) & 0xFF));
		}

		/// <summary>
		/// Reads the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Read32(uint offset)
		{
			CheckOffset(offset);
			return address.Load32(offset);
		}

		/// <summary>
		/// Writes the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write32(uint offset, uint value)
		{
			CheckOffset(offset);
			address.Store32(offset, value);
		}
	}
}
