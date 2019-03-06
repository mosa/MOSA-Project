// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	/// <summary>
	/// Tree capable of adding arbitrary intervals and performing search queries on them
	/// </summary>
	public sealed class DelayedIntervalTree<T> where T : class
	{
		private IntervalTree<T> tree = new IntervalTree<T>();

		private bool delayedDelete = false;

		private int delayedDeleteStart;
		private int delayedDeleteEnd;

		private void FlushDelete()
		{
			if (delayedDelete)
			{
				tree.Remove(delayedDeleteStart, delayedDeleteEnd);
				delayedDelete = false;
			}
		}

		//private static bool Contains(int start, int end, int start2, int end2)
		//{
		//	return Contains(start, end, start2) || Contains(start, end, end2);
		//}

		private static bool Contains(int start, int end, int at)
		{
			return at >= start && at < end;
		}

		public bool Contains(int start, int end)
		{
			if (delayedDelete && Contains(delayedDeleteStart, delayedDeleteEnd, start) && Contains(delayedDeleteStart, delayedDeleteEnd, end))
			{
				return false;
			}

			FlushDelete();
			return tree.Contains(start, end);
		}

		public bool Contains(int at)
		{
			if (delayedDelete && Contains(delayedDeleteStart, delayedDeleteEnd, at))
			{
				return false;
			}

			FlushDelete();
			return tree.Contains(at);
		}

		public void Add(int start, int end, T value)
		{
			if (!delayedDelete)
			{
				tree.Add(start, end, value);
				return;
			}

			if (Contains(delayedDeleteStart, delayedDeleteEnd, start) || Contains(delayedDeleteStart, delayedDeleteEnd, end))
			{
				tree.Replace(start, end, value);
				delayedDelete = false;
				return;
			}

			FlushDelete();
			tree.Add(start, end, value);
		}

		public void Remove(int start, int end)
		{
			FlushDelete();

			delayedDelete = true;
			delayedDeleteStart = start;
			delayedDeleteEnd = end;
		}

		public T SearchFirstOverlapping(int start, int end)
		{
			FlushDelete();
			return tree.SearchFirstOverlapping(start, end);
		}

		public T SearchFirstOverlapping(int at)
		{
			FlushDelete();
			return tree.SearchFirstOverlapping(at);
		}

		public List<T> Search(int at)
		{
			FlushDelete();
			return tree.Search(at);
		}

		public List<T> Search(int start, int end)
		{
			FlushDelete();
			return tree.Search(start, end);
		}

		public IEnumerator<T> GetEnumerator()
		{
			FlushDelete();
			return tree.GetEnumerator();
		}
	}
}
