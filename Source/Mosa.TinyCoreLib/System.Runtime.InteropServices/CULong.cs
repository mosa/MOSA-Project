using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

[CLSCompliant(false)]
public readonly struct CULong : IEquatable<CULong>
{
	private readonly int _dummyPrimitive;

	public UIntPtr Value
	{
		get
		{
			throw null;
		}
	}

	public CULong(uint value)
	{
		throw null;
	}

	public CULong(UIntPtr value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public bool Equals(CULong other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
