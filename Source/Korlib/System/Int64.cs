/*
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
	public struct Int64
	{
		public const long MaxValue = 0x7fffffffffffffff;
		public const long MinValue = -9223372036854775808;

		internal long _value;
	}
}
