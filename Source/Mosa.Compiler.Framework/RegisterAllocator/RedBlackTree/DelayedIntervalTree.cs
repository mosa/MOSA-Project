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
		private bool delayedAdd = false;

		private int delayedDeleteStart;
		private int delayedDeleteEnd;

		private int delayedAddStart;
		private int delayedAddEnd;
		private T delayedAddValue;

		private void FlushDelete()
		{
			if (delayedDelete)
			{
				tree.Remove(delayedDeleteStart, delayedDeleteEnd);
				delayedDelete = false;
			}
		}

		private void FlushAdd()
		{
			if (delayedAdd)
			{
				tree.Add(delayedAddStart, delayedAddEnd, delayedAddValue);
				delayedAdd = false;
			}
		}

		private static bool Contains(int start, int end, int at)
		{
			return at >= start && at < end;
		}

		public bool Contains(int start, int end)
		{
			if (delayedDelete && start >= delayedDeleteStart && end < delayedDeleteEnd)
			{
				return false;
			}

			if (delayedAdd && (Contains(delayedAddStart, delayedAddEnd, start) || Contains(delayedAddStart, delayedAddEnd, end)))
			{
				return true;
			}

			FlushDelete();

			return tree.Contains(start, end);
		}

		public bool Contains(int at)
		{
			if (delayedAdd && Contains(delayedAddStart, delayedAddEnd, at))
			{
				return true;
			}

			if (delayedDelete && Contains(delayedDeleteStart, delayedDeleteEnd, at))
			{
				return false;
			}

			return tree.Contains(at);
		}

		public void Add(int start, int end, T value)
		{
			if (delayedDelete && (Contains(delayedDeleteStart, delayedDeleteEnd, start) || Contains(delayedDeleteStart, delayedDeleteEnd, end)))
			{
				tree.Replace(start, end, value);
				delayedDelete = false;
				return;
			}

			//tree.Add(start, end, value);

			FlushAdd();

			delayedAdd = true;
			delayedAddStart = start;
			delayedAddEnd = end;
			delayedAddValue = value;
		}

		public void Remove(int start, int end)
		{
			if (delayedAdd && (Contains(delayedAddStart, delayedAddEnd, start) || Contains(delayedAddStart, delayedAddEnd, end)))
			{
				delayedAdd = false;
				return;
			}

			FlushDelete();

			delayedDelete = true;
			delayedDeleteStart = start;
			delayedDeleteEnd = end;
		}

		public T SearchFirstOverlapping(int start, int end)
		{
			FlushDelete();
			FlushAdd();
			return tree.SearchFirstOverlapping(start, end);
		}

		public T SearchFirstOverlapping(int at)
		{
			FlushDelete();
			FlushAdd();
			return tree.SearchFirstOverlapping(at);
		}

		public List<T> Search(int at)
		{
			FlushDelete();
			FlushAdd();
			return tree.Search(at);
		}

		public List<T> Search(int start, int end)
		{
			FlushDelete();
			FlushAdd();
			return tree.Search(start, end);
		}

		public IEnumerator<T> GetEnumerator()
		{
			FlushDelete();
			FlushAdd();
			return tree.GetEnumerator();
		}
	}
}
