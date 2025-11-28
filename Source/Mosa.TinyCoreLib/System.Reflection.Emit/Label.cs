using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Emit;

public readonly struct Label : IEquatable<Label>
{
	private readonly int _dummyPrimitive;

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(Label obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(Label a, Label b)
	{
		throw null;
	}

	public static bool operator !=(Label a, Label b)
	{
		throw null;
	}
}
