// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct Int16
	{
		public const short MaxValue = 32767;
		public const short MinValue = -32768;

		internal short _value;

		public int CompareTo(short value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(short obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((short)obj) == _value;
		}

		public override string ToString()
		{
			return Int32.CreateString((uint)_value, true, false);
		}

		public override int GetHashCode()
		{
			return _value;
		}
	}
}
