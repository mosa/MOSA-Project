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

		public int CompareTo(char value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(char obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((char)obj) == _value;
		}

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

		public override int GetHashCode()
		{
			return (int)_value;
		}
	}
}
