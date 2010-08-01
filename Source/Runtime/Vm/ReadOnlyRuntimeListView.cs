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
	public abstract class ReadOnlyRuntimeListView<TItemType> :
		IList<TItemType>, IList where TItemType : IEquatable<TItemType>
	{
		#region Data members

		/// <summary>
		/// Holds the total number of fields in this list.
		/// </summary>
		private int count;

		/// <summary>
		/// Holds the index of the first _stackFrameIndex in this list.
		/// </summary>
		private int firstItem;

		/// <summary>
		/// Holds the static instance of the runtime.
		/// </summary>
		protected ITypeSystem typeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Used to initialize RuntimeFieldList.Empty.
		/// </summary>
		protected ReadOnlyRuntimeListView()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyRuntimeListView{TItemType}"/> class.
		/// </summary>
		/// <param name="firstField">The index of the first _stackFrameIndex. May not be negative.</param>
		/// <param name="count">The number of fields in the list. Must be larger than zero.</param>
		public ReadOnlyRuntimeListView(int firstField, int count, ITypeSystem typeSystem)
		{
			if (0 > firstField)
				throw new ArgumentOutOfRangeException(@"firstField", firstField, @"May not be negative.");
			if (0 > count)
				throw new ArgumentOutOfRangeException(@"count", count, @"Must be larger than zero.");

			firstItem = firstField;
			this.count = count;
			this.typeSystem = typeSystem;

			System.Diagnostics.Debug.Assert(typeSystem != null);
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the items of the list.
		/// </summary>
		protected abstract TItemType[] Items { get; }

		#endregion // Properties

		#region IList<TItemType> Members

		int IList<TItemType>.IndexOf(TItemType item)
		{
			TItemType[] items = Items;

			if (items == null)
				return -1;

			for (int i = 0; i < count; i++)
			{
				if (item.Equals(items[firstItem + i]))
					return i;
			}

			return -1;
		}

		void IList<TItemType>.Insert(int index, TItemType item)
		{
			throw new NotSupportedException();
		}

		void IList<TItemType>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		TItemType IList<TItemType>.this[int index]
		{
			get
			{
				if (0 > index || index >= count)
					throw new ArgumentOutOfRangeException(@"index");

				return Items[firstItem + index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion // IList<TItemType> Members

		#region ICollection<TItemType> Members

		void ICollection<TItemType>.Add(TItemType item)
		{
			throw new NotSupportedException();
		}

		void ICollection<TItemType>.Clear()
		{
			throw new NotSupportedException();
		}

		bool ICollection<TItemType>.Contains(TItemType item)
		{
			TItemType[] items = Items;

			if (items == null)
				return false;

			for (int i = 0; i < count; i++)
			{
				if (item.Equals(items[firstItem + i]))
					return true;
			}

			return false;
		}

		void ICollection<TItemType>.CopyTo(TItemType[] array, int arrayIndex)
		{
			if (null == array)
				throw new ArgumentNullException(@"array");
			if (arrayIndex + count > array.Length)
				throw new ArgumentException(@"Insufficient array space.");

			TItemType[] items = Items;
			for (int i = 0; i < count; i++)
			{
				array[arrayIndex + i] = items[firstItem + i];
			}
		}

		int ICollection<TItemType>.Count
		{
			get { return count; }
		}

		bool ICollection<TItemType>.IsReadOnly
		{
			get { return true; }
		}

		bool ICollection<TItemType>.Remove(TItemType item)
		{
			throw new NotSupportedException();
		}

		#endregion // ICollection<RuntimeField> Members

		#region IEnumerable<TItemType> Members

		IEnumerator<TItemType> IEnumerable<TItemType>.GetEnumerator()
		{
			TItemType[] items = Items;
			if (items != null)
			{
				for (int i = 0; i < count; i++)
				{
					yield return items[firstItem + i];
				}
			}
		}

		#endregion // IEnumerable<RuntimeField> Members

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			TItemType[] items = Items;
			if (items != null)
			{
				for (int i = 0; i < count; i++)
				{
					yield return items[firstItem + i];
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
			if (!(value is TItemType))
				throw new ArgumentException(@"Wrong value type.", @"value");

			TItemType item = (TItemType)value;
			TItemType[] items = Items;

			if (items == null)
				return false;

			for (int i = 0; i < count; i++)
				if (item.Equals(items[firstItem + i]))
					return true;

			return false;
		}

		int IList.IndexOf(object value)
		{
			if (null == value)
				throw new ArgumentNullException(@"value");
			if (!(value is TItemType))
				throw new ArgumentException(@"Wrong value type.", @"value");

			TItemType item = (TItemType)value;
			TItemType[] items = Items;

			if (items == null)
				return -1;

			for (int i = 0; i < count; i++)
				if (item.Equals(items[firstItem + i]))
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
				return Items[firstItem + index];
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

			TItemType[] results = array as TItemType[];
			TItemType[] items = Items;
			if (null == results)
				throw new ArgumentException(@"Wrong array type.");
			for (int i = 0; i < count; i++)
			{
				results[index + i] = items[firstItem + i];
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
