// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct UInt16: IComparable, IComparable<ushort>
	{
		public const ushort MaxValue = 0xffff;
		public const ushort MinValue = 0;

		internal ushort m_value;

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is ushort)) { throw new ArgumentException("Argument Type Must Be UInt16", "value"); }

			ushort u_value = ((ushort)value).m_value;

			if (m_value < u_value) return -1;
			if (m_value > u_value) return 1;

			return 0;
		}

		public int CompareTo(ushort value)
		{
			if (m_value < value) return -1;
		    if (m_value > value) return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ushort)) { return false; }

			return m_value == ((ushort)obj).m_value;
		}

		public bool Equals(ushort obj)
		{
			return m_value == obj;
		}

		public override string ToString()
		{
			return int.CreateString(m_value, false, false);
		}

		public string ToString(string format)
		{
			return int.CreateString(m_value, false, true);
		}

		public override int GetHashCode()
		{
			return (int)m_value;
		}
	}
}
