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
	public enum SeekOrigin : int
	{
		/// <summary>
		/// Begin
		/// </summary>
		Begin = 0,
		/// <summary>
		/// Current
		/// </summary>
		Current = 1,
		/// <summary>
		/// End
		/// </summary>
		End = 2,
	}
}
