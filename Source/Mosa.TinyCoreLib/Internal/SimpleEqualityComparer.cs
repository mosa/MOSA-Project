using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Internal;

public class SimpleEqualityComparer<T> : EqualityComparer<T>
{
	public override bool Equals(T? x, T? y)
	{
		if (x is null || y is null)
			return x is null && y is null;

		if (x is IEquatable<T> xEq && y is IEquatable<T> yEq)
			return xEq.Equals(yEq);

		return x.Equals(y);
	}

	public override int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
}
