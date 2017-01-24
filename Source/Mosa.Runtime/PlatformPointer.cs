// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	/// <summary>
	/// Platform independent pointer that will be automatically converted to the appropriately sized pointer during MOSA compilation.
	/// This special type is unsafe and should be used with caution as it has no pre-determined size until MOSA compilation.
	/// No public constructors are available because this special type can only be created through operators.
	/// TODO: add compiler stage that converts the operator methods into single/few instructions.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Ptr
	{
		/// <summary>
		/// Address this pointer points to.
		/// Automatically sized during MOSA compilation
		/// </summary>
		private void* _value;

		private static uint _size = (uint)sizeof(void*);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Ptr(uint value)
		{
			_value = (void*)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Ptr(ulong value)
		{
			_value = (void*)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Ptr(void* value)
		{
			_value = value;
		}

		public static uint Size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return _size; }
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Ptr))
				return false;

			return ((Ptr)obj)._value == _value;
		}

		public override int GetHashCode()
		{
			return (int)_value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Ptr Dereference(uint i)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr(((uint*)_value)[i]);
			else
				p = new Ptr(((ulong*)_value)[i]);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Increment()
		{
			if (_size == 4)
				_value = (void*)((uint)_value + _size);
			else
				_value = (void*)((ulong)_value + _size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Decrement()
		{
			if (_size == 4)
				_value = (void*)((uint)_value - _size);
			else
				_value = (void*)((ulong)_value - _size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, Ptr value2)
		{
			return (value1._value == value2._value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, Ptr value2)
		{
			return (value1._value != value2._value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, Ptr value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2._value);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2._value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, Ptr value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2._value);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2._value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, Ptr value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2._value);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2._value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator ~(Ptr value)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr(~((uint)value._value));
			else
				p = new Ptr(~((ulong)value._value));
			return p;
		}

		#region uint

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(uint value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint(Ptr value)
		{
			return (uint)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, uint value2)
		{
			if (_size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, uint value2)
		{
			if (_size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, uint value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, uint value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, uint value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion uint

		#region uint*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(uint* value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint* (Ptr value)
		{
			return (uint*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, uint* value2)
		{
			return (value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, uint* value2)
		{
			return (value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, uint* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, uint* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, uint* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion uint*

		#region ulong

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(ulong value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator ulong(Ptr value)
		{
			return (ulong)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, ulong value2)
		{
			if (_size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, ulong value2)
		{
			if (_size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, ulong value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, ulong value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, ulong value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion ulong

		#region ulong*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(ulong* value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong* (Ptr value)
		{
			return (ulong*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, ulong* value2)
		{
			if (_size == 4)
				return ((uint)value1._value == (uint)value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, ulong* value2)
		{
			if (_size == 4)
				return ((uint)value1._value != (uint)value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, ulong* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, ulong* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, ulong* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion ulong*

		#region int

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(int value)
		{
			Ptr p = new Ptr((uint)value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int(Ptr value)
		{
			return (int)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, int value2)
		{
			if (_size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, int value2)
		{
			if (_size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, int value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, int value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, int value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion int

		#region int*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(int* value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int* (Ptr value)
		{
			return (int*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, int* value2)
		{
			return (value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, int* value2)
		{
			return (value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, int* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, int* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, int* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion int*

		#region long

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(long value)
		{
			Ptr p = new Ptr((ulong)value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator long(Ptr value)
		{
			return (long)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, long value2)
		{
			if (_size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, long value2)
		{
			if (_size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, long value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, long value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, long value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion long

		#region long*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(long* value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator long* (Ptr value)
		{
			return (long*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, long* value2)
		{
			if (_size == 4)
				return ((uint)value1._value == (uint)value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, long* value2)
		{
			if (_size == 4)
				return ((uint)value1._value != (uint)value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, long* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, long* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, long* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion long*

		#region void*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(void* value)
		{
			Ptr p = new Ptr(value);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator void* (Ptr value)
		{
			return value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, void* value2)
		{
			return (value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, void* value2)
		{
			return (value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, void* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value + (uint)value2);
			else
				p = new Ptr((ulong)value1._value + (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, void* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value - (uint)value2);
			else
				p = new Ptr((ulong)value1._value - (ulong)value2);
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, void* value2)
		{
			Ptr p;
			if (_size == 4)
				p = new Ptr((uint)value1._value & (uint)value2);
			else
				p = new Ptr((ulong)value1._value & (ulong)value2);
			return p;
		}

		#endregion void*
	}
}
