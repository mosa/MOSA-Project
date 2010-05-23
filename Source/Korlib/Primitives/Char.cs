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
	public struct Char
	{
		public const char MaxValue = (char)0xffff;
		public const char MinValue = (char)0;

		internal char m_value;

		public override string ToString()
		{
			return new String(m_value, 1);
		}
	}
}
