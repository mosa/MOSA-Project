// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public static class ListExtension
	{
		/// <summary>
		/// Adds if new.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static void AddIfNew<T>(this List<T> list, T item)
		{
			if (list.Contains(item))
				return;

			list.Add(item);
		}

		public static void AddIfNew<T>(this HashSet<T> list, T item)
		{
			if (list.Contains(item))
				return;

			list.Add(item);
		}

		/// <summary>
		/// Determines whether the two lists' elements are equal.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="other">The other list.</param>
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
