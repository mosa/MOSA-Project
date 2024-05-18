using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public sealed class ObjectEqualityComparer<T> : EqualityComparer<T>
{
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override bool Equals(T? x, T? y)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override int GetHashCode([DisallowNull] T obj)
	{
		throw null;
	}
}
