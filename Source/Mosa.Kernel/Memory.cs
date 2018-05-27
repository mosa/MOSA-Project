// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;
using System;

namespace Mosa.Kernel
{
	public sealed class Memory : BaseMemory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Memory"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public Memory(uint address, uint size) : base(address, size)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Memory"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public Memory(UIntPtr address, uint size) : base(address.ToUInt32(), size)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Memory"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public Memory(IntPtr address, uint size) : base((uint)address.ToInt32(), size)
		{
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Byte" /> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="System.Byte"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public override byte this[uint index]
		{
			get { return Intrinsic.Load8(Address, index); }
			set { Intrinsic.Store8(Address, index, value); }
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public override byte Read8(uint index)
		{
			return Intrinsic.Load8(Address, index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public override void Write8(uint index, byte value)
		{
			Intrinsic.Store8(Address, index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public override ushort Read16(uint index)
		{
			return Intrinsic.Load16(Address, index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public override void Write16(uint index, ushort value)
		{
			Intrinsic.Store16(Address, index, value);
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public override uint Read24(uint index)
		{
			return Intrinsic.Load16(Address, index) | (uint)(Intrinsic.Load8(Address, index + 2) << 16);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public override void Write24(uint index, uint value)
		{
			Intrinsic.Store16(Address, index, (ushort)(value & 0xFFFF));
			Intrinsic.Store8(Address, index + 2, (byte)((value >> 16) & 0xFF));
		}

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public override uint Read32(uint index)
		{
			return Intrinsic.Load32(Address, index);
		}

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public override void Write32(uint index, uint value)
		{
			Intrinsic.Store32(Address, index, value);
		}
	}
}
