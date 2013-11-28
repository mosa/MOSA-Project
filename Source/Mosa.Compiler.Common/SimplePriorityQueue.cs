/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class SimpleKeyPriorityQueue<T>
	{
		private readonly LinkedList<KeyValuePair<int, T>> items;

		public int Count { get { return items.Count; } }

		public bool IsEmpty { get { return items.Count == 0; } }

		public SimpleKeyPriorityQueue()
		{
			items = new LinkedList<KeyValuePair<int, T>>();
		}

		public void Enqueue(int priority, T item)
		{
			var keyitem = new KeyValuePair<int, T>(priority, item);

			if (IsEmpty)
			{
				items.AddFirst(keyitem);
				return;
			}

			var at = items.First;

			while (at != null && priority < at.Value.Key)
			{
				at = at.Next;
			}

			if (at == null)
			{
				items.AddLast(keyitem);
			}
			else
			{
				items.AddBefore(at, keyitem);
			}
		}

		public T Dequeue()
		{
			var item = items.First.Value;
			items.RemoveFirst();
			return item.Value;
		}
	}
}