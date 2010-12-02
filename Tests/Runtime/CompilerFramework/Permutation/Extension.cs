/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Mosa.Runtime.CompilerFramework.Permutation
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

		public static IEnumerable<int> UpTo(this int start, int end)
		{
			while (start <= end)
				yield return start++;
		}

	}
}
