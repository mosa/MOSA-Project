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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class LinkedList<T> : IEnumerable<T>, ICollection<T>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
		public class LinkedListNode<U>
		{
            /// <summary>
            /// 
            /// </summary>
			public U value;

            /// <summary>
            /// 
            /// </summary>
			public LinkedListNode<U> next;

            /// <summary>
            /// 
            /// </summary>
			public LinkedListNode<U> previous;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="previous"></param>
            /// <param name="next"></param>
			public LinkedListNode(U value, LinkedListNode<U> previous, LinkedListNode<U> next)
			{
				this.value = value;
				this.next = next;
				this.previous = previous;
			}

		}

        /// <summary>
        /// 
        /// </summary>
		protected LinkedListNode<T> first;

        /// <summary>
        /// 
        /// </summary>
		protected int count;

        /// <summary>
        /// 
        /// </summary>
		public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
		public int Count { get { return count; } }

        /// <summary>
        /// 
        /// </summary>
		public LinkedListNode<T> First
		{
			get
			{
				return first;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		protected LinkedListNode<T> last;

        /// <summary>
        /// 
        /// </summary>
		public LinkedListNode<T> Last
		{
			get
			{
				return last;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public LinkedList()
		{
			first = last = null;
		}

        /// <summary>
        /// 
        /// </summary>
		public void Clear()
		{
			first = last = null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public bool Contains(T value)
		{
			return (Find(value) != null);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
		public void Add(T value)
		{
			AddLast(value);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, last, null);
			return AddLast(node);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, null, first);
			return AddFirst(node);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
		public LinkedListNode<T> AddFirst(LinkedListNode<T> node)
		{
			if (first != null)
				first.previous = node;

			first = node;

			count++;
			return node;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public bool Remove(T value)
		{
			LinkedListNode<T> node = Find(value);

			if (node == null)
				return false;

			Remove(node);
			return true;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
		public void RemoveLast()
		{
			if (last == null)
				throw new InvalidOperationException();

			last.previous = null;
			count--;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;
		}


	}
}
