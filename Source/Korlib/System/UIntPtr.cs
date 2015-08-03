// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct UIntPtr
	{
		/// <summary>
		/// This is 32-bit specific :(
		/// </summary>
		internal uint _value;

		public static int Size { get { return 4; } }

		public UIntPtr(uint value)
		{
			_value = value;
		}

		public unsafe UIntPtr(void* value)
		{
			_value = (uint)value;
		}

		public static explicit operator UIntPtr(uint value)
		{
			return new UIntPtr(value);
		}

		//public static explicit operator UIntPtr(int value)
		//{
		//	return new UIntPtr((uint)value);
		//}

		public static explicit operator uint(UIntPtr value)
		{
			return value._value;
		}

		public static unsafe explicit operator UIntPtr(void* value)
		{
			return new UIntPtr(value);
		}

		public static unsafe explicit operator void*(UIntPtr value)
		{
			return value.ToPointer();
		}

		public unsafe static bool operator ==(UIntPtr value1, UIntPtr value2)
		{
			return value1._value == value2._value;
		}

		public unsafe static bool operator !=(UIntPtr value1, UIntPtr value2)
		{
			return value1._value != value2._value;
		}

		public unsafe override int GetHashCode()
		{
			return (int)_value & 0x7fffffff;
		}

		public unsafe override bool Equals(Object obj)
		{
			if (obj is UIntPtr)
			{
				return (_value == ((UIntPtr)obj)._value);
			}
			return false;
		}

		public unsafe uint ToUInt32()
		{
			return _value; ;
		}

		public unsafe override String ToString()
		{
			return _value.ToString();
		}

		public static UIntPtr Add(UIntPtr pointer, int offset)
		{
			return pointer + offset;
		}

		public static UIntPtr operator +(UIntPtr pointer, int offset)
		{
			return new UIntPtr(pointer.ToUInt32() + (uint)offset);
		}

		public static UIntPtr Subtract(UIntPtr pointer, int offset)
		{
			return pointer - offset;
		}

		public static UIntPtr operator -(UIntPtr pointer, int offset)
		{
			return new UIntPtr(pointer.ToUInt32() - (uint)offset);
		}

		public unsafe void* ToPointer()
		{
			return (void*)_value;
		}
	}
}