// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct UInt16
	{
		public const ushort MaxValue = 0xffff;
		public const ushort MinValue = 0;

		internal ushort _value;

		public int CompareTo(ushort value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(ushort obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((ushort)obj) == _value;
		}

		public override string ToString()
		{
			return int.CreateString(_value, false, false);
		}

		public string ToString(string format)
		{
			return int.CreateString(_value, false, true);
		}

		public override int GetHashCode()
		{
			return _value;
		}
	}
}
