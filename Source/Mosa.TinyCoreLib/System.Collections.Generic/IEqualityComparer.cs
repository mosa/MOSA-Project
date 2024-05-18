using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public interface IEqualityComparer<in T>
{
	bool Equals(T? x, T? y);

	int GetHashCode([DisallowNull] T obj);
}
