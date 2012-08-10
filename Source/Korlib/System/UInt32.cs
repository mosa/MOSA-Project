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
	public struct UInt32
	{
		public const uint MaxValue = 0xffffffff;
		public const uint MinValue = 0;

		internal uint _value;

		public int CompareTo(uint value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(uint obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((uint)obj) == _value;
		}

		public override string ToString()
		{
			return Int32.CreateString(_value, false, false);
		}

		public string ToString(string format)
		{
			return Int32.CreateString(_value, false, true);
		}

		public override int GetHashCode()
		{
			return (int)_value;
		}
	}
}
