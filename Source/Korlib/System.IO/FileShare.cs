/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.IO
{
	/// <summary>
	/// 
	/// </summary>
	public enum FileShare
	{
		/// <summary>
		/// None
		/// </summary>
		None,
		/// <summary>
		/// Read
		/// </summary>
		Read,
		/// <summary>
		/// Write
		/// </summary>
		Write,
		/// <summary>
		/// ReadWrite
		/// </summary>
		ReadWrite,
		/// <summary>
		/// Delete
		/// </summary>
		Delete,
		/// <summary>
		/// Inheritable
		/// </summary>
		Inheritable
	}
}
