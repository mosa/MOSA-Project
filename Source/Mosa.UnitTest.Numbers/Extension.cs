// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTest.Numbers
{
	public static class Extension
	{
		public static void AddIfNew<T>(this IList<T> list, T value)
		{
			if (!list.Contains(value))
				list.Add(value);
		}

		public static void AddIfNew<T>(this IList<T> list, IList<T> values)
		{
			foreach (T value in values)
				list.AddIfNew<T>(value);
		}
	}
}
