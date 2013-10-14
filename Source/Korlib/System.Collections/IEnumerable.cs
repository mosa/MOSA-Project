/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Collections
{
	/// <summary>
	///
	/// </summary>
	public interface IEnumerable
	{
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		IEnumerator GetEnumerator();
	}
}