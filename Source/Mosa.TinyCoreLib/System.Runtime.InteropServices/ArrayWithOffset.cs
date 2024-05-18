using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

public readonly struct ArrayWithOffset : IEquatable<ArrayWithOffset>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ArrayWithOffset(object? array, int offset)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ArrayWithOffset obj)
	{
		throw null;
	}

	public object? GetArray()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int GetOffset()
	{
		throw null;
	}

	public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b)
	{
		throw null;
	}

	public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b)
	{
		throw null;
	}
}
