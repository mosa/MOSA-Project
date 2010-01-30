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

namespace System.Collections
{
	/// <summary>
	/// 
	/// </summary>
    public interface ICollection : IEnumerable
    {
		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
        int Count { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is synchronized.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is synchronized; otherwise, <c>false</c>.
		/// </value>
		bool IsSynchronized { get; }

		/// <summary>
		/// Gets the sync root.
		/// </summary>
		/// <value>The sync root.</value>
		object SyncRoot { get; }

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		void CopyTo(Array array, int index);
    }
}
