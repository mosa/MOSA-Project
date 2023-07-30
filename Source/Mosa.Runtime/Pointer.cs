// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime;

public struct Pointer
{
	private readonly unsafe void* value;

	public static Pointer Zero
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new Pointer(0);
	}

	public static unsafe int Size
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => sizeof(void*);
	}

	public bool IsNull
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ToInt64() == 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(IntPtr value)
	{
		this.value = value.ToPointer();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(UIntPtr value)
	{
		this.value = value.ToPointer();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(int value)
	{
		this.value = (void*)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(long value)
	{
		this.value = (void*)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(uint value)
	{
		this.value = (void*)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(ulong value)
	{
		this.value = (void*)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Pointer(void* value)
	{
		this.value = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override unsafe bool Equals(Object obj)
	{
		if (obj is Pointer)
		{
			return value == ((Pointer)obj).value;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override unsafe int GetHashCode()
	{
		return (int)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe int ToInt32()
	{
		return (int)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe long ToInt64()
	{
		return (long)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe uint ToUInt32()
	{
		return (uint)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe ulong ToUInt64()
	{
		return (ulong)value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe IntPtr ToIntPtr()
	{
		return new IntPtr(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe UIntPtr ToUIntPtr()
	{
		return new UIntPtr(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator Pointer(uint value)
	{
		return new Pointer(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator Pointer(ulong value)
	{
		return new Pointer(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator Pointer(int value)
	{
		return new Pointer(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator Pointer(long value)
	{
		return new Pointer(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator Pointer(void* value)
	{
		return new Pointer(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator void*(Pointer value)
	{
		return value.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator int(Pointer value)
	{
		return (int)value.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator long(Pointer value)
	{
		return (long)value.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator IntPtr(Pointer value)
	{
		return new IntPtr(value.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe explicit operator UIntPtr(Pointer value)
	{
		return new UIntPtr(value.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator ==(Pointer value1, Pointer value2)
	{
		return value1.value == value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator !=(Pointer value1, Pointer value2)
	{
		return value1.value != value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator >(Pointer value1, Pointer value2)
	{
		return value1.value > value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator >=(Pointer value1, Pointer value2)
	{
		return value1.value >= value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator <(Pointer value1, Pointer value2)
	{
		return value1.value < value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool operator <=(Pointer value1, Pointer value2)
	{
		return value1.value <= value2.value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator +(Pointer pointer, int offset)
	{
		return new Pointer(pointer.ToInt64() + (uint)offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator +(Pointer pointer, uint offset)
	{
		return new Pointer(pointer.ToInt64() + offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator +(Pointer pointer, long offset)
	{
		return new Pointer(pointer.ToInt64() + offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator +(Pointer pointer, ulong offset)
	{
		return new Pointer(pointer.ToUInt64() + offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator ++(Pointer pointer)
	{
		return new Pointer(pointer.ToInt64() + 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator -(Pointer pointer, int offset)
	{
		return new Pointer((long)pointer.value - offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator -(Pointer pointer, uint offset)
	{
		return new Pointer((ulong)pointer.value - offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator -(Pointer pointer, long offset)
	{
		return new Pointer((long)pointer.value - offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator -(Pointer pointer, ulong offset)
	{
		return new Pointer((ulong)pointer.value - offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Pointer operator --(Pointer pointer)
	{
		return new Pointer((long)pointer.value - 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe void* ToPointer()
	{
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override unsafe string ToString()
	{
		return ((long)value).ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long GetOffset(Pointer b)
	{
		return b.ToInt64() - ToInt64();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort Load16()
	{
		return Intrinsic.Load16(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort Load16(uint offset)
	{
		return Intrinsic.Load16(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort Load16(int offset)
	{
		return Intrinsic.Load16(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Load24(uint offset)
	{
		return Intrinsic.Load16(this, offset) | (uint)(Intrinsic.Load8(this, offset + 2) << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Load32()
	{
		return Intrinsic.Load32(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Load32(uint offset)
	{
		return Intrinsic.Load32(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Load32(int offset)
	{
		return Intrinsic.Load32(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong Load64()
	{
		return Intrinsic.Load64(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong Load64(int offset)
	{
		return Intrinsic.Load64(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong Load64(uint offset)
	{
		return Intrinsic.Load64(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte Load8()
	{
		return Intrinsic.Load8(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte Load8(uint offset)
	{
		return Intrinsic.Load8(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte Load8(int offset)
	{
		return Intrinsic.Load8(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LoadR4()
	{
		return Intrinsic.LoadR4(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LoadR4(uint offset)
	{
		return Intrinsic.LoadR4(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double LoadR8()
	{
		return Intrinsic.LoadR8(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double LoadR8(uint offset)
	{
		return Intrinsic.LoadR8(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Pointer LoadPointer()
	{
		return Intrinsic.LoadPointer(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Pointer LoadPointer(uint offset)
	{
		return Intrinsic.LoadPointer(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Pointer LoadPointer(int offset)
	{
		return Intrinsic.LoadPointer(this, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store16(ushort value)
	{
		Intrinsic.Store16(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store16(int offset, ushort value)
	{
		Intrinsic.Store16(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store16(uint offset, ushort value)
	{
		Intrinsic.Store16(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store24(uint offset, uint value)
	{
		Intrinsic.Store16(this, offset, (ushort)(value & 0xFFFF));
		Intrinsic.Store8(this, offset + 2, (byte)((value >> 16) & 0xFF));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(uint offset, uint value)
	{
		Intrinsic.Store32(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(int offset, uint value)
	{
		Intrinsic.Store32(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(uint offset, int value)
	{
		Intrinsic.Store32(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(int offset, int value)
	{
		Intrinsic.Store32(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(uint value)
	{
		Intrinsic.Store32(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store32(int value)
	{
		Intrinsic.Store32(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store64(ulong value)
	{
		Intrinsic.Store64(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store64(uint offset, ulong value)
	{
		Intrinsic.Store64(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store64(int offset, ulong value)
	{
		Intrinsic.Store64(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store64(uint offset, long value)
	{
		Intrinsic.Store64(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store8(byte value)
	{
		Intrinsic.Store8(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store8(uint offset, byte value)
	{
		Intrinsic.Store8(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Store8(int offset, byte value)
	{
		Intrinsic.Store8(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR4(float value)
	{
		Intrinsic.StoreR4(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR8(double value)
	{
		Intrinsic.StoreR8(this, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR4(uint offset, float value)
	{
		Intrinsic.StoreR4(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR4(int offset, float value)
	{
		Intrinsic.StoreR4(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR8(uint offset, double value)
	{
		Intrinsic.StoreR8(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StoreR8(int offset, double value)
	{
		Intrinsic.StoreR8(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StorePointer(Pointer value)
	{
		Intrinsic.StorePointer(this, 0, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StorePointer(uint offset, Pointer value)
	{
		Intrinsic.StorePointer(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StorePointer(int offset, Pointer value)
	{
		Intrinsic.StorePointer(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StorePointer(long offset, Pointer value)
	{
		Intrinsic.StorePointer(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void StorePointer(ulong offset, Pointer value)
	{
		Intrinsic.StorePointer(this, offset, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear(uint size)
	{
		Internal.MemoryClear(this, size);
	}
}
