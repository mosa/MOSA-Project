using System.Collections.Generic;
using System.ComponentModel;

namespace System;

public struct HashCode
{
	private int _dummyPrimitive;

	public void AddBytes(ReadOnlySpan<byte> value)
	{
	}

	public void Add<T>(T value)
	{
	}

	public void Add<T>(T value, IEqualityComparer<T>? comparer)
	{
	}

	public static int Combine<T1>(T1 value1)
	{
		throw null;
	}

	public static int Combine<T1, T2>(T1 value1, T2 value2)
	{
		throw null;
	}

	public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
	{
		throw null;
	}

	public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		throw new NotImplementedException();
	}

	public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
	{
		throw null;
	}

	public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
	{
		throw null;
	}

	public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
	{
		throw null;
	}

	public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes.", true)]
	public override bool Equals(object? obj)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.", true)]
	public override int GetHashCode()
	{
		throw null;
	}

	public int ToHashCode()
	{
		throw null;
	}
}
