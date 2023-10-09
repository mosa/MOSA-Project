// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class HasSetExtension
{
	public static void AddIfNew<T>(this HashSet<T> hashSet, T item)
	{
		if (hashSet.Contains(item))
			return;

		hashSet.Add(item);
	}
}
