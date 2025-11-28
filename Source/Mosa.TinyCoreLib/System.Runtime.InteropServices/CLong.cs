using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

[CLSCompliant(false)]
public readonly struct CLong : IEquatable<CLong>
{
	private readonly int _dummyPrimitive;

	public IntPtr Value
	{
		get
		{
			throw null;
		}
	}

	public CLong(int value)
	{
		throw null;
	}

	public CLong(IntPtr value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public bool Equals(CLong other)
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
