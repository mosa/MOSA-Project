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

namespace System.Collections.Generic
{
    /// <summary>
    /// Defines methods to manipulate generic collections.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the collection.
    /// </typeparam>
    public interface ICollection<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection&lt;T&gt;
        /// is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        /// <param name="item">
        /// The object to add to the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </param>
        void Add(T item);

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        void Clear();
   
        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection&lt;T&gt; contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection&lt;T&gt;.</param>
        /// <returns>
        /// true if item is found in the System.Collections.Generic.ICollection&lt;T&gt; otherwise,
        /// false.
        /// </returns>
        bool Contains(T item);

		/// <summary>
		/// 
		/// </summary>
		void CopyTo(T[] array, int arrayIndex);

		/// <summary>
		/// 
		/// </summary>
		bool Remove(T item);
    }
}
