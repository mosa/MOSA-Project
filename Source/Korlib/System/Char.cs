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

		internal char _value;

		public static bool IsUpper(char c)
		{
			unsafe
			{
				var value = (ushort)c;
				return value >= 65 && value <= 90;
			}
		}

		public static bool IsUpper(string s, int index)
		{
			return IsUpper(s[index]);
		}

		public override string ToString()
		{
			return new String(_value, 1);
		}

		public static bool IsUpper(char c)
		{
			//HACK - US only
			return (c >= 'A' && c <= 'Z');
		}
	}
}
