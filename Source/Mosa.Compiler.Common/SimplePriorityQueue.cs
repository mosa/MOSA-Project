/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class SimplePriorityQueue<T> where T : IComparable<T>
	{
		private readonly LinkedList<T> items;

		public bool IsEmpty { get { return items.Count == 0; } }

		public SimplePriorityQueue()
		{
			items = new LinkedList<T>();
		}

		public void Enqueue(T item)
		{
			if (IsEmpty)
			{
				items.AddFirst(item);
				return;
			}

			LinkedListNode<T> at = items.First;

			while (at != null && at.Value.CompareTo(item) < 0)
			{
				at = at.Next;
			}

			if (at == null)
			{
				items.AddLast(item);
			}
			else
			{
				items.AddBefore(at, item);
			}
		}

		public T Dequeue()
		{
			T item = items.First.Value;
			items.RemoveFirst();
			return item;
		}

	}
}
