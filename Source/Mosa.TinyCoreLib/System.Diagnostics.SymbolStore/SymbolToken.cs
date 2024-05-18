using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics.SymbolStore;

public readonly struct SymbolToken : IEquatable<SymbolToken>
{
	private readonly int _dummyPrimitive;

	public SymbolToken(int val)
	{
		throw null;
	}

	public bool Equals(SymbolToken obj)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int GetToken()
	{
		throw null;
	}

	public static bool operator ==(SymbolToken a, SymbolToken b)
	{
		throw null;
	}

	public static bool operator !=(SymbolToken a, SymbolToken b)
	{
		throw null;
	}
}
