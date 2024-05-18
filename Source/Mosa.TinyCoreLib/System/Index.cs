using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Index : IEquatable<Index>
{
	private readonly int _dummyPrimitive;

	public static Index End
	{
		get
		{
			throw null;
		}
	}

	public bool IsFromEnd
	{
		get
		{
			throw null;
		}
	}

	public static Index Start
	{
		get
		{
			throw null;
		}
	}

	public int Value
	{
		get
		{
			throw null;
		}
	}

	public Index(int value, bool fromEnd = false)
	{
		throw null;
	}

	public bool Equals(Index other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static Index FromEnd(int value)
	{
		throw null;
	}

	public static Index FromStart(int value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int GetOffset(int length)
	{
		throw null;
	}

	public static implicit operator Index(int value)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
