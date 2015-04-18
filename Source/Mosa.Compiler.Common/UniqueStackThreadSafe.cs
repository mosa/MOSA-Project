/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class UniqueStackThreadSafe<T>
	{
		private readonly Stack<T> stack;
		private readonly HashSet<T> set;

		public int Count { get { return stack.Count; } }

		public UniqueStackThreadSafe()
		{
			stack = new Stack<T>();
			set = new HashSet<T>();
		}

		public void Push(T item)
		{
			lock (stack)
			{
				if (!set.Contains(item))
				{
					set.Add(item);
					stack.Push(item);
				}
			}
		}

		public T Pop()
		{
			lock (stack)
			{
				if (stack.Count == 0)
					return default(T);

				var item = stack.Pop();
				set.Remove(item);
				return item;
			}
		}
	}
}
