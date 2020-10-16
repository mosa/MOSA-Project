// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public static class ListExtension
	{
		public static void AddIfNew<T>(this List<T> list, T item)
		{
			if (list.Contains(item))
				return;

			list.Add(item);
		}

		public static bool SequenceEquals<T>(this IList<T> list, IList<T> other)
		{
			if (list.Count != other.Count)
				return false;

			var comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < list.Count; i++)
			{
				if (!comparer.Equals(list[i], other[i]))
					return false;
			}
			return true;
		}
	}
}
