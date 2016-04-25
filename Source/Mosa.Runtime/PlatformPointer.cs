// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	/// <summary>
	/// Platform independent pointer that will be automatically converted to the appropriately sized pointer during MOSA compilation.
	/// This special type is unsafe and should be used with caution as it has no pre-determined size until MOSA compilation.
	/// No public constructors are available because this special type can only be created through operators.
	/// TODO: add compiler stage that converts the operator methods into single/few instructions.
	/// </summary>
	public unsafe struct Ptr
	{
		/// <summary>
		/// Address this pointer points to.
		/// Automatically sized during MOSA compilation
		/// </summary>
		private void* _value;

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

		public static int Size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return sizeof(void*); }
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
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2._value);
			else
				return new Ptr((ulong)value1._value + (ulong)value2._value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, Ptr value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2._value);
			else
				return new Ptr((ulong)value1._value - (ulong)value2._value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, Ptr value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2._value);
			else
				return new Ptr((ulong)value1._value & (ulong)value2._value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator ~(Ptr value)
		{
			if (Size == 4)
				return new Ptr(~((uint)value._value));
			else
				return new Ptr(~((ulong)value._value));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator ++(Ptr value)
		{
			if (Size == 4)
				return new Ptr((uint)value._value + 1);
			else
				return new Ptr((ulong)value._value + 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator --(Ptr value)
		{
			if (Size == 4)
				return new Ptr((uint)value._value - 1);
			else
				return new Ptr((ulong)value._value - 1);
		}

		#region uint

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(uint value)
		{
			return new Ptr(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint(Ptr value)
		{
			return (uint)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, uint value2)
		{
			if (Size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, uint value2)
		{
			if (Size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, uint value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, uint value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, uint value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion uint

		#region uint*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(uint* value)
		{
			return new Ptr(value);
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
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, uint* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, uint* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion uint*

		#region ulong

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(ulong value)
		{
			return new Ptr(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator ulong(Ptr value)
		{
			return (ulong)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, ulong value2)
		{
			if (Size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, ulong value2)
		{
			if (Size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, ulong value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, ulong value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, ulong value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion ulong

		#region ulong*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(ulong* value)
		{
			return new Ptr(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong* (Ptr value)
		{
			return (ulong*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, ulong* value2)
		{
			if (Size == 4)
				return ((uint)value1._value == (uint)value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, ulong* value2)
		{
			if (Size == 4)
				return ((uint)value1._value != (uint)value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, ulong* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, ulong* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, ulong* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion ulong*

		#region int

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(int value)
		{
			return new Ptr((uint)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int(Ptr value)
		{
			return (int)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, int value2)
		{
			if (Size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, int value2)
		{
			if (Size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, int value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, int value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, int value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion int

		#region int*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(int* value)
		{
			return new Ptr(value);
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
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, int* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, int* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion int*

		#region long

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Ptr(long value)
		{
			return new Ptr((ulong)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator long(Ptr value)
		{
			return (long)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, long value2)
		{
			if (Size == 4)
				return ((uint)value1._value == value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, long value2)
		{
			if (Size == 4)
				return ((uint)value1._value != value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, long value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, long value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, long value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion long

		#region long*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(long* value)
		{
			return new Ptr(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator long* (Ptr value)
		{
			return (long*)value._value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Ptr value1, long* value2)
		{
			if (Size == 4)
				return ((uint)value1._value == (uint)value2);
			else
				return ((ulong)value1._value == (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Ptr value1, long* value2)
		{
			if (Size == 4)
				return ((uint)value1._value != (uint)value2);
			else
				return ((ulong)value1._value != (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator +(Ptr value1, long* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, long* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, long* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion long*

		#region void*

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Ptr(void* value)
		{
			return new Ptr(value);
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
			if (Size == 4)
				return new Ptr((uint)value1._value + (uint)value2);
			else
				return new Ptr((ulong)value1._value + (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator -(Ptr value1, void* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value - (uint)value2);
			else
				return new Ptr((ulong)value1._value - (ulong)value2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ptr operator &(Ptr value1, void* value2)
		{
			if (Size == 4)
				return new Ptr((uint)value1._value & (uint)value2);
			else
				return new Ptr((ulong)value1._value & (ulong)value2);
		}

		#endregion void*
	}
}
