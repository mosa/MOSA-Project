// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime
{
	public struct Pointer
	{
		private readonly unsafe void* value;

		public static Pointer Zero { get { return new Pointer(0); } }

		public unsafe static int Size { get { return sizeof(void*); } }

		public bool IsNull { get { return ToInt64() == 0; } }

		public unsafe Pointer(IntPtr value)
		{
			this.value = value.ToPointer();
		}

		public unsafe Pointer(UIntPtr value)
		{
			this.value = value.ToPointer();
		}

		public unsafe Pointer(int value)
		{
			this.value = (void*)value;
		}

		public unsafe Pointer(long value)
		{
			this.value = (void*)value;
		}

		public unsafe Pointer(uint value)
		{
			this.value = (void*)value;
		}

		public unsafe Pointer(ulong value)
		{
			this.value = (void*)value;
		}

		public unsafe Pointer(void* value)
		{
			this.value = value;
		}

		public unsafe override bool Equals(Object obj)
		{
			if (obj is Pointer)
			{
				return (value == ((Pointer)obj).value);
			}
			return false;
		}

		public unsafe override int GetHashCode()
		{
			return (int)value;
		}

		public unsafe int ToInt32()
		{
			return (int)value;
		}

		public unsafe long ToInt64()
		{
			return (long)value;
		}

		public unsafe uint ToUInt32()
		{
			return (uint)value;
		}

		public unsafe ulong ToUInt64()
		{
			return (ulong)value;
		}

		public unsafe IntPtr ToIntPtr()
		{
			return new IntPtr(value);
		}

		public unsafe UIntPtr ToUIntPtr()
		{
			return new UIntPtr(value);
		}

		public static unsafe explicit operator Pointer(uint value)
		{
			return new Pointer(value);
		}

		public static unsafe explicit operator Pointer(ulong value)
		{
			return new Pointer(value);
		}

		public static unsafe explicit operator Pointer(int value)
		{
			return new Pointer(value);
		}

		public static unsafe explicit operator Pointer(long value)
		{
			return new Pointer(value);
		}

		public static unsafe explicit operator Pointer(void* value)
		{
			return new Pointer(value);
		}

		public static unsafe explicit operator void*(Pointer value)
		{
			return value.value;
		}

		public static unsafe explicit operator int(Pointer value)
		{
			return (int)value.value;
		}

		public static unsafe explicit operator long(Pointer value)
		{
			return (long)value.value;
		}

		public static unsafe explicit operator IntPtr(Pointer value)
		{
			return new IntPtr(value.value);
		}

		public static unsafe explicit operator UIntPtr(Pointer value)
		{
			return new UIntPtr(value.value);
		}

		public static unsafe bool operator ==(Pointer value1, Pointer value2)
		{
			return value1.value == value2.value;
		}

		public static unsafe bool operator !=(Pointer value1, Pointer value2)
		{
			return value1.value != value2.value;
		}

		public static unsafe bool operator >(Pointer value1, Pointer value2)
		{
			return value1.value > value2.value;
		}

		public static unsafe bool operator >=(Pointer value1, Pointer value2)
		{
			return value1.value >= value2.value;
		}

		public static unsafe bool operator <(Pointer value1, Pointer value2)
		{
			return value1.value < value2.value;
		}

		public static unsafe bool operator <=(Pointer value1, Pointer value2)
		{
			return value1.value <= value2.value;
		}

		public static unsafe Pointer operator +(Pointer pointer, int offset)
		{
			return new Pointer(pointer.ToInt64() + (uint)offset);
		}

		public static unsafe Pointer operator +(Pointer pointer, long offset)
		{
			return new Pointer(pointer.ToInt64() + offset);
		}

		public static unsafe Pointer operator +(Pointer pointer, ulong offset)
		{
			return new Pointer(pointer.ToUInt64() + offset);
		}

		public static unsafe Pointer operator -(Pointer pointer, int offset)
		{
			return new Pointer((long)pointer.value - offset);
		}

		public static unsafe Pointer operator -(Pointer pointer, uint offset)
		{
			return new Pointer((ulong)pointer.value - offset);
		}

		public static unsafe Pointer operator -(Pointer pointer, long offset)
		{
			return new Pointer((long)pointer.value - offset);
		}

		public static unsafe Pointer operator -(Pointer pointer, ulong offset)
		{
			return new Pointer((ulong)pointer.value - offset);
		}

		public unsafe void* ToPointer()
		{
			return value;
		}

		public unsafe override string ToString()
		{
			return ((long)value).ToString();
		}

		public long GetOffset(Pointer b)
		{
			return b.ToInt64() - ToInt64();
		}

		public ushort Load16(uint offset)
		{
			return Intrinsic.Load16(this, offset);
		}

		public uint Load24(uint offset)
		{
			return Intrinsic.Load16(this, offset) | (uint)(Intrinsic.Load8(this, offset + 2) << 16);
		}

		public uint Load32()
		{
			return Intrinsic.Load32(this);
		}

		public uint Load32(uint offset)
		{
			return Intrinsic.Load32(this, offset);
		}

		public uint Load32(int offset)
		{
			return Intrinsic.Load32(this, offset);
		}

		public ulong Load64(int offset)
		{
			return Intrinsic.Load64(this, offset);
		}

		public ulong Load64(uint offset)
		{
			return Intrinsic.Load64(this, offset);
		}

		public byte Load8(uint offset)
		{
			return Intrinsic.Load8(this, offset);
		}

		public byte Load8(int offset)
		{
			return Intrinsic.Load8(this, offset);
		}

		public Pointer LoadPointer()
		{
			return Intrinsic.LoadPointer(this);
		}

		public Pointer LoadPointer(uint offset)
		{
			return Intrinsic.LoadPointer(this, offset);
		}

		public Pointer LoadPointer(int offset)
		{
			return Intrinsic.LoadPointer(this, offset);
		}

		public void Store16(uint offset, ushort value)
		{
			Intrinsic.Store16(this, offset, value);
		}

		public void Store24(uint offset, uint value)
		{
			Intrinsic.Store16(this, offset, (ushort)(value & 0xFFFF));
			Intrinsic.Store8(this, offset + 2, (byte)((value >> 16) & 0xFF));
		}

		public void Store32(uint offset, uint value)
		{
			Intrinsic.Store32(this, offset, value);
		}

		public void Store32(int offset, uint value)
		{
			Intrinsic.Store32(this, offset, value);
		}

		public void Store32(uint offset, int value)
		{
			Intrinsic.Store32(this, offset, value);
		}

		public void Store32(int offset, int value)
		{
			Intrinsic.Store32(this, offset, value);
		}

		public void Store32(uint value)
		{
			Intrinsic.Store32(this, value);
		}

		public void Store64(uint offset, ulong value)
		{
			Intrinsic.Store64(this, offset, value);
		}

		public void Store64(int offset, ulong value)
		{
			Intrinsic.Store64(this, offset, value);
		}

		public void Store64(uint offset, long value)
		{
			Intrinsic.Store64(this, offset, value);
		}

		public void Store8(uint offset, byte value)
		{
			Intrinsic.Store8(this, offset, value);
		}

		public void Store8(int offset, byte value)
		{
			Intrinsic.Store8(this, offset, value);
		}

		public void StorePointer(Pointer value)
		{
			Intrinsic.StorePointer(this, 0, value);
		}

		public void StorePointer(uint offset, Pointer value)
		{
			Intrinsic.StorePointer(this, offset, value);
		}

		public void StorePointer(int offset, Pointer value)
		{
			Intrinsic.StorePointer(this, offset, value);
		}

		public void StorePointer(long offset, Pointer value)
		{
			Intrinsic.StorePointer(this, offset, value);
		}

		public void StorePointer(ulong offset, Pointer value)
		{
			Intrinsic.StorePointer(this, offset, value);
		}
	}
}
