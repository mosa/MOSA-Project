using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public abstract class EqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
{
	public static EqualityComparer<T> Default
	{
		get
		{
			throw null;
		}
	}

	public static EqualityComparer<T> Create(Func<T?, T?, bool> equals, Func<T, int>? getHashCode = null)
	{
		throw null;
	}

	public abstract bool Equals(T? x, T? y);

	public abstract int GetHashCode([DisallowNull] T obj);

	bool IEqualityComparer.Equals(object? x, object? y)
	{
		throw null;
	}

	int IEqualityComparer.GetHashCode(object obj)
	{
		throw null;
	}
}
