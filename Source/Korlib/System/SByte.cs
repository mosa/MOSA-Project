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
	public struct SByte
	{
		public const sbyte MinValue = -128;
		public const sbyte MaxValue = 127;

		internal sbyte m_value;

		public override string ToString()
		{
			return Int32.CreateString((uint)m_value, true, false);
		}
	}
}
