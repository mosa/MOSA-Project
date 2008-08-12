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
using System.Collections;

namespace Mosa.ClassLib
{
	public class LinkedList<T> : IEnumerable<T>, ICollection<T>
	{
		public class LinkedListNode<U>
		{
			public U value;
			public LinkedListNode<U> next;
			public LinkedListNode<U> previous;

			public LinkedListNode(U value, LinkedListNode<U> previous, LinkedListNode<U> next)
			{
				this.value = value;
				this.next = next;
				this.previous = previous;
			}

		}

		protected LinkedListNode<T> first;
		protected int count;

		public bool IsReadOnly { get { return false; } }
		public int Count { get { return count; } }

		public LinkedListNode<T> First
		{
			get
			{
				return first;
			}
		}

		protected LinkedListNode<T> last;

		public LinkedListNode<T> Last
		{
			get
			{
				return last;
			}
		}

		public LinkedList()
		{
			first = last = null;
		}

		public void Clear()
		{
			first = last = null;
		}

		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> cur = first;

			while (cur != null) {
				if (cur.value.Equals(value))
					return cur;
				cur = cur.next;
			}
			return null;
		}

		public bool Contains(T value)
		{
			return (Find(value) != null);
		}

		public LinkedListNode<T> FindLast(T value)
		{
			LinkedListNode<T> found = null;
			LinkedListNode<T> cur = first;

			while (cur != null) {
				if (cur.value.Equals(value))
					found = cur;
				cur = cur.next;
			}
			return found;
		}

		public void Add(T value)
		{
			AddLast(value);
		}

		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, last, null);
			return AddLast(node);
		}

		public LinkedListNode<T> AddLast(LinkedListNode<T> node)
		{
			if (first == null) {
				first = node;
				last = node;
			}
			else {
				node.previous.next = node;
				last = node;
			}

			count++;
			return node;
		}

		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, null, first);
			return AddFirst(node);
		}

		public LinkedListNode<T> AddFirst(LinkedListNode<T> node)
		{
			if (first != null)
				first.previous = node;

			first = node;

			count++;
			return node;
		}

		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			if (node == null)
				throw new ArgumentNullException();

			LinkedListNode<T> cur = new LinkedListNode<T>(value, node, node.next);

			if (node.next != null)
				node.next.previous = cur;
			node.next = cur;

			if (cur.next == null)
				last = cur;

			count++;
			return cur;
		}

		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			if (node == null)
				throw new ArgumentNullException();

			LinkedListNode<T> cur = new LinkedListNode<T>(value, node.previous, node);

			if (node.previous != null)
				node.previous.next = cur;
			node.previous = cur;

			if (cur.previous == null)
				first = cur;

			count++;
			return cur;
		}

		public bool Remove(T value)
		{
			LinkedListNode<T> node = Find(value);

			if (node == null)
				return false;

			Remove(node);
			return true;
		}

		public void Remove(LinkedListNode<T> node)
		{
			if (node == null)
				throw new InvalidOperationException();

			if (node.previous != null)
				node.previous.next = node.next;

			if (node.next != null)
				node.next.previous = node.previous;

			if (node == first)
				first = node.next;

			if (node == last)
				last = node.previous;

			count--;
		}

		public void RemoveFirst()
		{
			if (first == null)
				throw new InvalidOperationException();

			first = first.next;
			first.previous = null;

			if (first.next == null)
				last = first;

			count--;
		}

		public void RemoveLast()
		{
			if (last == null)
				throw new InvalidOperationException();

			last.previous = null;
			count--;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException();
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException();

			//if (array.Rank != 1)
			//    throw new ArgumentException();
			//if (array.Length - arrayIndex + array.GetLowerBound(0) < count)
			//    throw new ArgumentException();

			LinkedListNode<T> cur = First;
			int index = arrayIndex;

			while (cur != null) {
				array[index++] = cur.value;
				cur = cur.next;

			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;
		}


	}
}
