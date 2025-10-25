using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Internal;

public class SpecificEqualityComparer<T>(Func<T, T, bool> equals, Func<T, int>? getHashCode) : EqualityComparer<T>
{
	public override bool Equals(T? x, T? y) => equals(x, y);

	public override int GetHashCode([DisallowNull] T obj) => getHashCode is null ? throw new NotSupportedException() : getHashCode(obj);
}
