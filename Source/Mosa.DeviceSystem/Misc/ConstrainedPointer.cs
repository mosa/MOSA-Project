// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.Misc;

/// <summary>
/// Provides access to a sized region in memory, using a <see cref="Pointer"/>. It also provides methods for directly reading and
/// writing 24-bit integers.
/// </summary>
public readonly struct ConstrainedPointer
{
	public Pointer Address { get; }

	public uint Size { get; }

	public ConstrainedPointer(Pointer address, uint size)
	{
		Address = address;
		Size = size;
	}

	public byte this[uint offset]
	{
		get { CheckOffset(offset); return Address.Load8(offset); }
		set { CheckOffset(offset); Address.Store8(offset, value); }
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte Read8(uint offset)
	{
		CheckOffset(offset);
		return Address.Load8(offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Write8(uint offset, byte value)
	{
		CheckOffset(offset);
		Address.Store8(offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort Read16(uint offset)
	{
		CheckOffset(offset);
		return Address.Load16(offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Write16(uint offset, ushort value)
	{
		CheckOffset(offset);
		Address.Store16(offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Read24(uint offset)
	{
		CheckOffset(offset);
		return Address.Load16(offset) | (uint)(Address.Load8(offset + 2) << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Write24(uint offset, uint value)
	{
		CheckOffset(offset);
		Address.Store16(offset, (ushort)(value & 0xFFFF));
		Address.Store8(offset + 2, (byte)((value >> 16) & 0xFF));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Read32(uint offset)
	{
		CheckOffset(offset);
		return Address.Load32(offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Write32(uint offset, uint value)
	{
		CheckOffset(offset);
		Address.Store32(offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong Read64(uint offset)
	{
		CheckOffset(offset);
		return Address.Load64(offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Write64(uint offset, ulong value)
	{
		CheckOffset(offset);
		Address.Store64(offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckOffset(uint offset)
	{
		if (offset >= Size)
			throw new ArgumentOutOfRangeException(nameof(offset));
	}
}
