// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	[Serializable]
	public struct SByte: IComparable, IComparable<sbyte>, IEquatable<sbyte>
	{
		internal sbyte m_value;

		public const sbyte MinValue = -128;
		public const sbyte MaxValue = 127;

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is sbyte)) { throw new ArgumentException("Argument Type Must Be SByte", "value"); }

			sbyte s_value = ((sbyte)value).m_value;

			if (m_value < s_value) return -1;
			if (m_value > s_value) return 1;

			return 0;
		}

		public int CompareTo(sbyte value)
		{
			if (m_value < value) return -1;
			if (m_value > value) return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is sbyte)) { return false; }

			return m_value == ((sbyte)obj).m_value;
		}

		public bool Equals(sbyte obj)
		{
			return m_value == obj;
		}

		public override int GetHashCode()
		{
			return m_value;
		}

		public override string ToString()
		{
			return int.CreateString((uint)m_value, true, false);
		}
	}
}
