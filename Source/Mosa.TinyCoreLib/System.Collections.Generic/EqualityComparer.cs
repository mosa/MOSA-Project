using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public abstract class EqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
{
	public static EqualityComparer<T> Default { get; } = new Internal.SimpleEqualityComparer<T>();

	public static EqualityComparer<T> Create(Func<T?, T?, bool> equals, Func<T, int>? getHashCode = null)
	{
		ArgumentNullException.ThrowIfNull(equals);

		return new Internal.SpecificEqualityComparer<T>(equals, getHashCode);
	}

	public abstract bool Equals(T? x, T? y);

	public abstract int GetHashCode([DisallowNull] T obj);

	bool IEqualityComparer.Equals(object? x, object? y)
	{
		if (x is null || y is null)
			return false;

		if (x is T a)
		{
			if (y is T b)
				return a.Equals(b);

			Internal.Exceptions.EqualityComparer.ThrowInvalidTypeException(nameof(y));
			return false;
		}

		Internal.Exceptions.EqualityComparer.ThrowInvalidTypeException(nameof(x));
		return false;
	}

	int IEqualityComparer.GetHashCode(object obj)
	{
		// Implementation detail: Even if "obj" is a value type, we accept null and just return 0.
		// This is different from the documentation which tells us to throw an exception if "obj"
		// is a reference type and "obj" is null.
		if (obj is null) return 0;
		if (obj is T value) return GetHashCode(value);

		Internal.Exceptions.EqualityComparer.ThrowInvalidTypeException(nameof(obj));
		return 0;
	}
}
