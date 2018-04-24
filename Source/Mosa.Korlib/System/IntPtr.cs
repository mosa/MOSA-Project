// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// A platform-specific type that is used to represent a pointer or a handle.
	/// </summary>
	[Serializable]
	public unsafe struct IntPtr
	{
		/// <summary>
		///
		/// </summary>
		private void* _value;

		public static readonly IntPtr Zero;

		public IntPtr(int value)
		{
			_value = (void*)value;
		}

		public IntPtr(long value)
		{
			_value = (void*)value;
		}

		unsafe public IntPtr(void* value)
		{
			_value = value;
		}

		public static int Size
		{
			get { return sizeof(void*); }
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IntPtr))
				return false;

			return ((IntPtr)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return (int)_value;
		}

		public int ToInt32()
		{
			return (int)_value;
		}

		public long ToInt64()
		{
			return (long)_value;
		}

		unsafe public void* ToPointer()
		{
			return _value;
		}

		//override public string ToString()
		//{
		//    return ToString(null);
		//}

		//public string ToString(string format)
		//{
		//    if (Size == 4)
		//        return ((int)_value).ToString(format);
		//    else
		//        return ((long)_value).ToString(format);
		//}

		public static bool operator ==(IntPtr value1, IntPtr value2)
		{
			return (value1._value == value2._value);
		}

		public static bool operator !=(IntPtr value1, IntPtr value2)
		{
			return (value1._value != value2._value);
		}

		public static explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		public static explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		unsafe public static explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		public static explicit operator int(IntPtr value)
		{
			return (int)value._value;
		}

		public static explicit operator long(IntPtr value)
		{
			return value.ToInt64();
		}

		unsafe public static explicit operator void* (IntPtr value)
		{
			return value._value;
		}
	}
}
