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

		public int CompareTo(long value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(long obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((long)obj) == _value;
		}

		public override int GetHashCode()
		{
			return (int)_value;
		}
	}
}