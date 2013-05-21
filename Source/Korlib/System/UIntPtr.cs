﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct UIntPtr
	{
		/// <summary>
		/// This is 32-bit specific :(
		/// </summary>
		internal uint _value;

		public static int Size { get { return 4; } }
	}
}