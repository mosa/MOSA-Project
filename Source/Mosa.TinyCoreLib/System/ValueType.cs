using System.Diagnostics.CodeAnalysis;

namespace System;

public abstract class ValueType
{
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
