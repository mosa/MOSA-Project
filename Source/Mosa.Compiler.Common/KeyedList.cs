// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class KeyedList<T, V>
	{
		public readonly Dictionary<T, List<V>> Collection;

		public Dictionary<T, List<V>>.KeyCollection Keys { get { return Collection.Keys; } }

		public List<V> this[T index] { get { return Collection.ContainsKey(index) ? Collection[index] : null; } }

		public KeyedList()
		{
			Collection = new Dictionary<T, List<V>>();
		}

		public void Add(T key, V value)
		{
			if (!Collection.TryGetValue(key, out List<V> list))
			{
				list = new List<V>();
				Collection.Add(key, list);
			}

			list.Add(value);
		}

		public void AddIfNew(T key, V value)
		{
			if (!Collection.TryGetValue(key, out List<V> list))
			{
				list = new List<V>();

				if (!list.Contains(value))
				{
					Collection.Add(key, list);
				}
			}

			list.Add(value);
		}

		public List<V> Get(T key)
		{
			Collection.TryGetValue(key, out List<V> list);

			return list;
		}
	}
}
