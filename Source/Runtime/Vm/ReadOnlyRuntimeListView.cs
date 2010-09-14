/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// Provides a read-only view of fields in a type.
	/// </summary>
	/// <remarks>
	/// This list is a read-only view of the this.Items list.
	/// </remarks>
	public abstract class ReadOnlyRuntimeListView<T> :
		IList<T>, IList where T : IEquatable<T>
	{
		#region Data members

		/// <summary>
		/// Holds the total number of fields in this list.
		/// </summary>
		private int count;

		/// <summary>
		/// Holds the index of the first _stackFrameIndex in this list.
		/// </summary>
		private int start;

		/// <summary>
		/// Holds the static instance of the runtime.
		/// </summary>
		protected IModuleTypeSystem moduleTypeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Used to initialize RuntimeFieldList.Empty.
		/// </summary>
		protected ReadOnlyRuntimeListView()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyRuntimeListView&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="start">The index of the first _stackFrameIndex. May not be negative.</param>
		/// <param name="count">The number of fields in the list. Must be larger than zero.</param>
		public ReadOnlyRuntimeListView(IModuleTypeSystem moduleTypeSystem, int start, int count)
		{
			if (0 > start)
				throw new ArgumentOutOfRangeException(@"firstField", start, @"May not be negative.");
			if (0 > count)
				throw new ArgumentOutOfRangeException(@"count", count, @"Must be larger than zero.");

			this.start = start;
			this.count = count;
			this.moduleTypeSystem = moduleTypeSystem;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the items of the list.
		/// </summary>
		protected abstract T[] Items { get; }

		#endregion // Properties

		#region IList<TItemType> Members

		int IList<T>.IndexOf(T item)
		{
			T[] items = Items;

			if (items == null)
				return -1;

			for (int i = 0; i < count; i++)
			{
				if (item.Equals(items[start + i]))
					return i;
			}

			return -1;
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		T IList<T>.this[int index]
		{
			get
			{
				if (0 > index || index >= count)
					throw new ArgumentOutOfRangeException(@"index");

				return Items[start + index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion // IList<TItemType> Members

		#region ICollection<TItemType> Members

		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		bool ICollection<T>.Contains(T item)
		{
			T[] items = Items;

			if (items == null)
				return false;

			for (int i = 0; i < count; i++)
			{
				if (item.Equals(items[start + i]))
					return true;
			}

			return false;
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			if (null == array)
				throw new ArgumentNullException(@"array");
			if (arrayIndex + count > array.Length)
				throw new ArgumentException(@"Insufficient array space.");

			T[] items = Items;
			for (int i = 0; i < count; i++)
			{
				array[arrayIndex + i] = items[start + i];
			}
		}

		int ICollection<T>.Count
		{
			get { return count; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return true; }
		}

		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		#endregion // ICollection<RuntimeField> Members

		#region IEnumerable<TItemType> Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			T[] items = Items;
			if (items != null)
			{
				for (int i = 0; i < count; i++)
				{
					yield return items[start + i];
				}
			}
		}

		#endregion // IEnumerable<RuntimeField> Members

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			T[] items = Items;
			if (items != null)
			{
				for (int i = 0; i < count; i++)
				{
					yield return items[start + i];
				}
			}
		}

		#endregion // IEnumerable Members

		#region IList Members

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Clear()
		{
			throw new NotSupportedException();
		}

		bool IList.Contains(object value)
		{
			if (null == value)
				throw new ArgumentNullException(@"value");
			if (!(value is T))
				throw new ArgumentException(@"Wrong value type.", @"value");

			T item = (T)value;
			T[] items = Items;

			if (items == null)
				return false;

			for (int i = 0; i < count; i++)
				if (item.Equals(items[start + i]))
					return true;

			return false;
		}

		int IList.IndexOf(object value)
		{
			if (null == value)
				throw new ArgumentNullException(@"value");
			if (!(value is T))
				throw new ArgumentException(@"Wrong value type.", @"value");

			T item = (T)value;
			T[] items = Items;

			if (items == null)
				return -1;

			for (int i = 0; i < count; i++)
				if (item.Equals(items[start + i]))
					return i;

			return -1;
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		bool IList.IsFixedSize
		{
			get { return true; }
		}

		bool IList.IsReadOnly
		{
			get { return true; }
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		object IList.this[int index]
		{
			get
			{
				return Items[start + index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion // IList Members

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException(@"array");
			if (index + count > array.Length)
				throw new ArgumentException(@"Insufficient array space.");

			T[] results = array as T[];
			T[] items = Items;
			if (null == results)
				throw new ArgumentException(@"Wrong array type.");
			for (int i = 0; i < count; i++)
			{
				results[index + i] = items[start + i];
			}
		}

		int ICollection.Count
		{
			get { return count; }
		}

		bool ICollection.IsSynchronized
		{
			get { return Items.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return Items.SyncRoot; }
		}

		#endregion // ICollection Members
	}
}
