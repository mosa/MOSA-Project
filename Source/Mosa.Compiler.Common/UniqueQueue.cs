// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class UniqueQueue<T>
	{
		private readonly Queue<T> queue;
		private readonly HashSet<T> set;

		public int Count { get { return queue.Count; } }

		public UniqueQueue()
		{
			queue = new Queue<T>();
			set = new HashSet<T>();
		}

		public void Enqueue(T item)
		{
			if (!set.Contains(item))
			{
				set.Add(item);
				queue.Enqueue(item);
			}
		}

		public T Dequeue()
		{
			if (queue.Count == 0)
				return default(T);

			var item = queue.Dequeue();
			set.Remove(item);
			return item;
		}

		public bool Contains(T item)
		{
			return set.Contains(item);
		}
	}
}
