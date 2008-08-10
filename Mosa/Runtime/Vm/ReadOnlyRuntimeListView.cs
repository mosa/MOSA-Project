/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// Provides a read-only view of fields in a type.
    /// </summary>
    /// <remarks>
    /// This list is a read-only view of the this.Items list.
    /// </remarks>
    public abstract class ReadOnlyRuntimeListView<ItemType> :
        IList<ItemType>, IList where ItemType: IEquatable<ItemType>
    {
        #region Data members

        /// <summary>
        /// Holds the total number of fields in this list.
        /// </summary>
        private int _count;

        /// <summary>
        /// Holds the index of the first _stackFrameIndex in this list.
        /// </summary>
        private int _firstItem;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Used to initialize RuntimeFieldList.Empty.
        /// </summary>
        protected ReadOnlyRuntimeListView()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RuntimeFieldList"/>.
        /// </summary>
        /// <param name="firstField">The index of the first _stackFrameIndex. May not be negative.</param>
        /// <param name="count">The number of fields in the list. Must be larger than zero.</param>
        public ReadOnlyRuntimeListView(int firstField, int count)
        {
            if (0 > firstField)
                throw new ArgumentOutOfRangeException(@"firstField", firstField, @"May not be negative.");
            if (0 > count)
                throw new ArgumentOutOfRangeException(@"count", count, @"Must be larger than zero.");

            _firstItem = firstField;
            _count = count;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the items of the list.
        /// </summary>
        protected abstract ItemType[] Items { get; }

        #endregion // Properties

        #region IList<ItemType> Members

        int IList<ItemType>.IndexOf(ItemType item)
        {
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                if (true == item.Equals(items[_firstItem + i]))
                    return i;
            }

            return -1;
        }

        void IList<ItemType>.Insert(int index, ItemType item)
        {
            throw new NotSupportedException();
        }

        void IList<ItemType>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        ItemType IList<ItemType>.this[int index]
        {
            get
            {
                if (0 > index || index >= _count)
                    throw new ArgumentOutOfRangeException(@"index");

                return this.Items[_firstItem + index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion // IList<ItemType> Members

        #region ICollection<ItemType> Members

        void ICollection<ItemType>.Add(ItemType item)
        {
            throw new NotSupportedException();
        }

        void ICollection<ItemType>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<ItemType>.Contains(ItemType item)
        {
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                if (true == item.Equals(items[_firstItem + i]))
                    return true;
            }

            return false;
        }

        void ICollection<ItemType>.CopyTo(ItemType[] array, int arrayIndex)
        {
            if (null == array)
                throw new ArgumentNullException(@"array");
            if (arrayIndex + _count > array.Length)
                throw new ArgumentException(@"Insufficient array space.");

            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                array[arrayIndex+i] = items[_firstItem + i];
            }
        }

        int ICollection<ItemType>.Count
        {
            get { return _count; }
        }

        bool ICollection<ItemType>.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<ItemType>.Remove(ItemType item)
        {
            throw new NotSupportedException();
        }

        #endregion // ICollection<RuntimeField> Members

        #region IEnumerable<ItemType> Members

        IEnumerator<ItemType> IEnumerable<ItemType>.GetEnumerator()
        {
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                yield return items[_firstItem + i];
            }
        }

        #endregion // IEnumerable<RuntimeField> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                yield return items[_firstItem + i];
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
            if (!(value is ItemType))
                throw new ArgumentException(@"Wrong value type.", @"value");

            ItemType item = (ItemType)value;
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                if (true == item.Equals(items[_firstItem + i]))
                    return true;
            }

            return false;
        }

        int IList.IndexOf(object value)
        {
            if (null == value)
                throw new ArgumentNullException(@"value");
            if (!(value is ItemType))
                throw new ArgumentException(@"Wrong value type.", @"value");

            ItemType item = (ItemType)value;
            ItemType[] items = this.Items;
            for (int i = 0; i < _count; i++)
            {
                if (true == item.Equals(items[_firstItem + i]))
                    return i;
            }

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
                return this.Items[_firstItem + index];
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
            if (null == array)
                throw new ArgumentNullException(@"array");
            if (index + _count > array.Length)
                throw new ArgumentException(@"Insufficient array space.");

            ItemType[] results = array as ItemType[];
            ItemType[] items = this.Items;
            if (null == results)
                throw new ArgumentException(@"Wrong array type.");
            for (int i = 0; i < _count; i++)
            {
                results[index+i] = items[_firstItem + i];
            }
        }

        int ICollection.Count
        {
            get { return _count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return this.Items.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return this.Items.SyncRoot; }
        }

        #endregion // ICollection Members
    }
}
