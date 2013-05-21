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
	public struct Byte
	{
		public const byte MinValue = 0;
		public const byte MaxValue = 255;

		internal byte _value;

		public int CompareTo(byte value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(byte obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((byte)obj) == _value;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return Int32.CreateString(_value, false, false);
		}

		public string ToString(string format)
		{
			return Int32.CreateString(_value, false, true);
		}
	}
}