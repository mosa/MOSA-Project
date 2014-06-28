/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Collections;

namespace Mosa.Compiler.Common
{
	public class KeyedList<T, V>
	{
		public readonly Dictionary<T, List<V>> Collection;

		public Dictionary<T, List<V>>.KeyCollection Keys { get { return Collection.Keys; } }

		public List<V> this[T index] { get { return Collection[index]; } }

		public KeyedList()
		{
			Collection = new Dictionary<T, List<V>>();
		}

		public void Add(T key, V value)
		{
			List<V> list;

			if (!Collection.TryGetValue(key, out list))
			{
				list = new List<V>();
				Collection.Add(key, list);
			}

			list.Add(value);
		}


	}
}