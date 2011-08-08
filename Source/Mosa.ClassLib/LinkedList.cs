/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using System.Collections.Generic;

namespace Mosa.ClassLib
{
	/// <summary>
	/// Implements a linked list
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
			/// Initializes a new instance of the <see cref="T:LinkedList.U:LinkedListNode"/> class.
			/// </summary>
			/// <param name="value">The value.</param>
			/// <param name="previous">The previous.</param>
			/// <param name="next">The next.</param>
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
		/// Gets a value indicating whether the <see cref="T:ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:ICollection`1"/> is read-only; otherwise, false.
		/// </returns>
		public bool IsReadOnly { get { return false; } }

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The number of elements contained in the <see cref="T:ICollection`1"/>.
		/// </returns>
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
		/// Gets the last.
		/// </summary>
		/// <value>The last.</value>
		public LinkedListNode<T> Last
		{
			get
			{
				return last;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LinkedList"/> class.
		/// </summary>
		public LinkedList()
		{
			first = last = null;
		}

		/// <summary>
		/// Removes all items from the <see cref="T:ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:ICollection`1"/> is read-only.
		/// </exception>
		public void Clear()
		{
			first = last = null;
		}

		/// <summary>
		/// Finds the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> cur = first;
			while (cur != null)
			{
				if (cur.value.Equals(value))
					return cur;
				cur = cur.next;
			}
			return null;
		}

		/// <summary>
		/// Determines whether [contains] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(T value)
		{
			return (Find(value) != null);
		}

		/// <summary>
		/// Finds the last.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> FindLast(T value)
		{
			LinkedListNode<T> found = null;
			LinkedListNode<T> cur = first;

			while (cur != null)
			{
				if (cur.value.Equals(value))
					found = cur;
				cur = cur.next;
			}
			return found;
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Add(T value)
		{
			AddLast(value);
		}

		/// <summary>
		/// Adds the last.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, last, null);
			return AddLast(node);
		}

		/// <summary>
		/// Adds the last.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		public LinkedListNode<T> AddLast(LinkedListNode<T> node)
		{
			if (first == null)
			{
				first = node;
				last = node;
			}
			else
			{
				node.previous.next = node;
				last = node;
			}
			count++;
			return node;
		}

		/// <summary>
		/// Adds the first.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> node = new LinkedListNode<T>(value, null, first);
			return AddFirst(node);
		}

		/// <summary>
		/// Adds the first.
		/// </summary>
		/// <param name="node">The node.</param>
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
		/// Adds the after.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			if (node == null)
				return null;

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
		/// Adds the before.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			if (node == null)
				return null;

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
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
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
		/// Removes the specified node.
		/// </summary>
		/// <param name="node">The node.</param>
		public void Remove(LinkedListNode<T> node)
		{
			if (node == null)
				return;

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
		/// Removes the first.
		/// </summary>
		public void RemoveFirst()
		{
			if (first == null)
				return;

			first = first.next;
			first.previous = null;

			if (first.next == null)
				last = first;

			count--;
		}

		/// <summary>
		/// Removes the last.
		/// </summary>
		public void RemoveLast()
		{
			if (last == null)
				return;

			if (last.previous != null)
				last.previous.next = null;

			count--;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
				return;
			if (arrayIndex < 0)
				return;

			//if (array.Rank != 1)
			//    throw new ArgumentException();
			//if (array.Length - arrayIndex + array.GetLowerBound(0) < count)
			//    throw new ArgumentException();

			LinkedListNode<T> cur = First;

			while (cur != null)
			{
				array[arrayIndex++] = cur.value;
				cur = cur.next;
			}
		}

		/// <summary>
		/// To the array.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			T[] array = new T[this.count];

			LinkedListNode<T> cur = First;
			uint index = 0;

			while (cur != null)
			{
				array[index++] = cur.value;
				cur = cur.next;
			}

			return array;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);/*
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;*/
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);/*
			for (LinkedListNode<T> cur = first; cur != null; cur = cur.next)
				yield return cur.value;*/
		}

		public struct Enumerator : IEnumerator<T>, IEnumerator
		{
			const string VersionKey = "version";
			const string IndexKey = "index";
			const string ListKey = "list";

			LinkedList<T> list;
			LinkedListNode<T> current;
			int index;

			internal Enumerator(LinkedList<T> parent)
			{
				this.list = parent;
				current = null;
				index = -1;
			}

			public T Current
			{
				get
				{
					return current.value;
				}
			}

			object IEnumerator.Current
			{
				get { return null; }
			}

			public bool MoveNext()
			{
				if (list == null)
					return false;

				if (current == null)
					current = list.first;
				else
				{
					current = current.next;
					if (current == list.first)
						current = null;
				}
				if (current == null)
				{
					index = -1;
					return false;
				}
				++index;
				return true;
			}

			void IEnumerator.Reset()
			{
				if (list == null)
					return;

				current = null;
				index = -1;
			}

			public void Dispose()
			{
				if (list == null)
					return;
				current = null;
				list = null;
			}
		}
	}
}
