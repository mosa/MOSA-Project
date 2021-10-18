// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	[Serializable]
	public struct Byte: IComparable, IComparable<byte>, IEquatable<byte>
	{
		internal byte m_value;

		public const byte MinValue = 0;
		public const byte MaxValue = 255;

		public override bool Equals(object obj)
		{
			if (this.GetType() == obj.GetType() && obj is Byte)
			{
				return (this.m_value == ((Byte)obj).m_value);
			}
			else
			{
				return false;
			}
		}

		public bool Equals(byte value)
		{
			return (m_value == value);
		}

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is byte)) { throw new ArgumentException("Argument Type Must Be Byte", "value"); }

			if (m_value < (((byte)value).m_value)) return -1;

			if (m_value > (((byte)value).m_value)) return 1;

			return 0;
		}

		public int CompareTo(byte value)
		{
			if (m_value < value) return -1;

			if (m_value > value) return 1;

			return 0;
		}

		public override int GetHashCode()
		{
			return m_value;
		}

		public override string ToString()
		{
			return int.CreateString(m_value, false, false);
		}

		public string ToString(string format)
		{
			return int.CreateString(m_value, false, true);
		}
	}
}
