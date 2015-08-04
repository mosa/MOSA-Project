// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class SimpleKeyPriorityQueue<T>
	{
		private readonly LinkedList<Tuple<int, T>> items;

		public int Count { get { return items.Count; } }

		public bool IsEmpty { get { return items.Count == 0; } }

		public SimpleKeyPriorityQueue()
		{
			items = new LinkedList<Tuple<int, T>>();
		}

		public void Enqueue(int priority, T item)
		{
			var keyitem = new Tuple<int, T>(priority, item);

			if (IsEmpty)
			{
				items.AddFirst(keyitem);
				return;
			}

			var at = items.First;

			while (at != null && priority < at.Value.Item1)
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
			return item.Item2;
		}
	}
}
