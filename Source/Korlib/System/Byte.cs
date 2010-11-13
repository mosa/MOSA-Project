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
	public struct Byte
	{
		public const byte MinValue = 0;
		public const byte MaxValue = 255;

		internal byte _value;

		public override string ToString()
		{
			return Int32.CreateString(_value, false, false);
		}

	}
}
