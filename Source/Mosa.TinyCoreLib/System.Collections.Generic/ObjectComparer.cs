using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public sealed class ObjectComparer<T> : Comparer<T>
{
	public override int Compare(T? x, T? y)
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
}
