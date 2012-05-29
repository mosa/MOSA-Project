/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

	}
}
