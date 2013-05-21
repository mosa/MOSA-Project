/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Collections.Generic
{
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IList<T> : ICollection<T>
	{
		/// <summary>
		/// Indexes of the item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		int IndexOf(T item);

		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		void Insert(int index, T item);

		/// <summary>
		/// Removes at.
		/// </summary>
		/// <param name="index">The index.</param>
		void RemoveAt(int index);

		/// <summary>
		/// Gets or sets the T at the specified index.
		/// </summary>
		/// <value></value>
		T this[int index]
		{
			get;
			set;
		}
	}
}