// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public static class SortedSetExtension
	{
		/// <summary>
		/// Adds if new.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static void AddIfNew<T>(this SortedSet<T> list, T item)
		{
			if (list.Contains(item))
				return;

			list.Add(item);
		}
	}
}
