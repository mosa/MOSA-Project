// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct SByte
	{
		public const sbyte MinValue = -128;
		public const sbyte MaxValue = 127;

		internal sbyte _value;

		public int CompareTo(SByte value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(sbyte obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((sbyte)obj) == _value;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return int.CreateString((uint)_value, true, false);
		}
	}
}
