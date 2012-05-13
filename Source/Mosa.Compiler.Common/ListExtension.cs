/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;
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

	}
}
