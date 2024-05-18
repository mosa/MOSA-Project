using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

public static class Nullable
{
	public static int Compare<T>(T? n1, T? n2) where T : struct
	{
		throw null;
	}

	public static bool Equals<T>(T? n1, T? n2) where T : struct
	{
		throw null;
	}

	public static Type? GetUnderlyingType(Type nullableType)
	{
		throw null;
	}

	public static ref readonly T GetValueRefOrDefaultRef<T>([In] ref T? nullable) where T : struct
	{
		throw null;
	}
}
public struct Nullable<T> where T : struct
{
	private T value;

	private int _dummyPrimitive;

	public readonly bool HasValue
	{
		get
		{
			throw null;
		}
	}

	public readonly T Value
	{
		get
		{
			throw null;
		}
	}

	public Nullable(T value)
	{
		throw null;
	}

	public override bool Equals(object? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public readonly T GetValueOrDefault()
	{
		throw null;
	}

	public readonly T GetValueOrDefault(T defaultValue)
	{
		throw null;
	}

	public static explicit operator T(T? value)
	{
		throw null;
	}

	public static implicit operator T?(T value)
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
