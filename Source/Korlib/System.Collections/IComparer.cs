/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Collections
{
	/// <summary>
	/// Interface for "System.Collections.IComparer"
	/// </summary>
	public interface IComparer
	{
		int Compare(object x, object y);
	}
}