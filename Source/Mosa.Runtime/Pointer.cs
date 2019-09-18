// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Mosa.Runtime
{
	public struct Pointer
	{
		private readonly unsafe void* _value; // Do not rename (binary serialization)

		public static readonly Pointer Zero;
		public bool IsNull { get { return ToInt64() == 0; } }

		public unsafe Pointer(IntPtr value)
		{
			_value = value.ToPointer();
		}

		public unsafe Pointer(UIntPtr value)
		{
			_value = value.ToPointer();
		}

		public unsafe Pointer(int value)
		{
			_value = (void*)value;
		}

		public unsafe Pointer(long value)
		{
			_value = (void*)((int)value);
		}

		public unsafe Pointer(void* value)
		{
			_value = value;
		}

		public unsafe override bool Equals(Object obj)
		{
			if (obj is Pointer)
			{
				return (_value == ((Pointer)obj)._value);
			}
			return false;
		}

		public unsafe override int GetHashCode()
		{
			return ((int)_value);
		}

		public unsafe int ToInt32()
		{
			return (int)_value;
		}

		public unsafe long ToInt64()
		{
			return (long)_value;
		}

		public unsafe uint ToUInt32()
		{
			return (uint)_value;
		}

		public unsafe ulong ToUInt64()
		{
			return (ulong)_value;
		}

		public unsafe IntPtr ToIntPtr()
		{
			return new IntPtr(_value);
		}

		public unsafe UIntPtr ToUIntPtr()
		{
			return new UIntPtr(_value);
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
			return value._value;
		}

		public static unsafe explicit operator int(Pointer value)
		{
			return (int)value._value;
		}

		public static unsafe explicit operator long(Pointer value)
		{
			return (long)value._value;
		}

		public static unsafe explicit operator IntPtr(Pointer value)
		{
			return new IntPtr(value._value);
		}

		public static unsafe explicit operator UIntPtr(Pointer value)
		{
			return new UIntPtr(value._value);
		}

		public static unsafe bool operator ==(Pointer value1, Pointer value2)
		{
			return value1._value == value2._value;
		}

		public static unsafe bool operator !=(Pointer value1, Pointer value2)
		{
			return value1._value != value2._value;
		}

		public static Pointer Add(Pointer pointer, int offset)
		{
			return pointer + offset;
		}

		public static unsafe Pointer operator +(Pointer pointer, int offset)
		{
			return new Pointer(pointer.ToInt64() + (uint)offset);
		}

		public static Pointer Subtract(Pointer pointer, int offset)
		{
			return pointer - offset;
		}

		public static unsafe Pointer operator -(Pointer pointer, int offset)
		{
			return new Pointer((long)pointer._value - offset);
		}

		public unsafe static int Size
		{
			get
			{
				return sizeof(void*);
			}
		}

		public unsafe void* ToPointer()
		{
			return _value;
		}

		public unsafe override string ToString()
		{
			return ((long)_value).ToString();
		}

		public Pointer Add(ulong b)
		{
			return new Pointer(ToInt64() + (long)b);
		}

		public Pointer Add(uint b)
		{
			return new Pointer(ToInt64() + b);
		}

		public bool GreaterThan(Pointer b)
		{
			return ToInt64() > b.ToInt64();
		}

		public bool GreaterThanOrEqual(Pointer b)
		{
			return ToInt64() >= b.ToInt64();
		}

		public bool LessThan(Pointer b)
		{
			return ToInt64() < b.ToInt64();
		}

		public bool LessThanOrEqual(Pointer b)
		{
			return ToInt64() <= b.ToInt64();
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

		//public Pointer AlignDown(uint align)
		//{
		//	return new Pointer((long)Alignment.AlignDown(this.ToInt64(), align));
		//}

		//public Pointer AlignUp(uint align)
		//{
		//	return new Pointer((long)Alignment.AlignUp(this.ToInt64(), align));
		//}
	}
}
