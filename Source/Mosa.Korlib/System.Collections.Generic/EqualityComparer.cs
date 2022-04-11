// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
 
namespace System.Collections.Generic
{
	[Serializable]
	public abstract partial class EqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
	{
		// TODO: Implement
		// public static EqualityComparer<T> Default is runtime-specific
 
		public abstract bool Equals(T x, T y);
		public abstract int GetHashCode([DisallowNull] T obj);
 
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null) return 0;
			if (obj is T) return GetHashCode((T)obj);

			throw new ArgumentException("Invalid argument for comparison");
		}
 
		bool IEqualityComparer.Equals(object x, object y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			if ((x is T) && (y is T)) return Equals((T)x, (T)y);
			
			throw new ArgumentException("Invalid argument for comparison");
		}
 
#if !CORERT
		internal virtual int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int endIndex = startIndex + count;
			for (int i = startIndex; i < endIndex; i++)
			{
				if (Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}
 
		internal virtual int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int endIndex = startIndex - count + 1;
			for (int i = startIndex; i >= endIndex; i--)
			{
				if (Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}
#endif
	}
 
	// The methods in this class look identical to the inherited methods, but the calls
	// to Equal bind to IEquatable<T>.Equals(T) instead of Object.Equals(Object)
	[Serializable]
	// Needs to be public to support binary serialization compatibility
	public sealed partial class GenericEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				if (y != null) return x.Equals(y);
				return false;
			}
			if (y != null) return false;
			return true;
		}
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode([DisallowNull] T obj) => obj.GetHashCode()/* ?? 0*/;
 
		// Equals method for the comparer itself.
		// If in the future this type is made sealed, change the is check to obj != null && GetType() == obj.GetType().
		public override bool Equals([NotNullWhen(true)] object obj) =>
			obj is GenericEqualityComparer<T>;
 
		// If in the future this type is made sealed, change typeof(...) to GetType().
		public override int GetHashCode() =>
			typeof(GenericEqualityComparer<T>).GetHashCode();
	}
 
	[Serializable]
	// Needs to be public to support binary serialization compatibility
	// TODO
	public sealed partial class NullableEqualityComparer<T> : EqualityComparer<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(T x, T y)
		{
			/*if (x.HasValue)
			{
				if (y.HasValue) return x.value.Equals(y.value);
				return false;
			}
			if (y.HasValue) return false;
			return true;*/

			if (x != null)
			{
				if (y != null) return x.Equals(y);
				return false;
			}
			if (y != null) return false;
			return true;
		}
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode(T obj) => obj.GetHashCode();
 
		// Equals method for the comparer itself.
		public override bool Equals([NotNullWhen(true)] object obj) =>
			obj != null && GetType() == obj.GetType();
 
		public override int GetHashCode() =>
			GetType().GetHashCode();
	}
 
	[Serializable]
	// Needs to be public to support binary serialization compatibility
	public sealed partial class ObjectEqualityComparer<T> : EqualityComparer<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				if (y != null) return x.Equals(y);
				return false;
			}
			if (y != null) return false;
			return true;
		}
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode([DisallowNull] T obj) => obj.GetHashCode()/* ?? 0*/;
 
		// Equals method for the comparer itself.
		public override bool Equals([NotNullWhen(true)] object obj) =>
			obj != null && GetType() == obj.GetType();
 
		public override int GetHashCode() =>
			GetType().GetHashCode();
	}
 
	[Serializable]
	// Needs to be public to support binary serialization compatibility
	public sealed partial class ByteEqualityComparer : EqualityComparer<byte>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(byte x, byte y)
		{
			return x == y;
		}
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode(byte b)
		{
			return b.GetHashCode();
		}
 
		// Equals method for the comparer itself.
		public override bool Equals([NotNullWhen(true)] object obj) =>
			obj != null && GetType() == obj.GetType();
 
		public override int GetHashCode() =>
			GetType().GetHashCode();
	}
 
	[Serializable]
	// Needs to be public to support binary serialization compatibility
	public sealed partial class EnumEqualityComparer<T> : EqualityComparer<T> where T : Enum
	{
		public EnumEqualityComparer() { }
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(T x, T y)
		{
			return RuntimeHelpers.EnumEquals(x, y);
		}
 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}
 
		// Equals method for the comparer itself.
		public override bool Equals([NotNullWhen(true)] object obj) =>
			obj != null && GetType() == obj.GetType();
 
		public override int GetHashCode() =>
			GetType().GetHashCode();
	}
}