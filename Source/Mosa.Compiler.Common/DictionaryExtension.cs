// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common;

public static class DictionaryExtension
{
	public static void AddIfNew<X, Y>(this Dictionary<X, Y> list, X key, Y item)
	{
		if (list.ContainsKey(key))
			return;

		list.Add(key, item);
	}
}
