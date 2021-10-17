// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct UInt64: IComparable, IComparable<ulong>
	{
		public const ulong MaxValue = 0xffffffffffffffff;
		public const ulong MinValue = 0;

		internal ulong m_value;

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is ulong)) { throw new ArgumentException("Argument Type Must Be UInt64", "value"); }

			ulong u_value = ((ulong)value).m_value;

			if (m_value < u_value) return -1;
			if (m_value > u_value) return 1;

			return 0;
		}

		public int CompareTo(ulong value)
		{
			if (m_value < value) return -1;
			if (m_value > value) return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ulong)) { return false; }

			return m_value == ((ulong)obj).m_value;
		}

		public bool Equals(ulong value)
		{
			return m_value == value;
		}

		public override int GetHashCode()
		{
			return ((int)m_value) ^ (int)(m_value >> 32);
		}

		public override unsafe string ToString()
		{
			int count = 0;
			var tmp = m_value;
			do
			{
				tmp /= 10;
				count++;
			} while (tmp != 0);

			var s = String.InternalAllocateString(count);
			var temp = m_value;

			for (int i = count - 1; i >= 0; i--)
			{
				s.first_char[i] = (char)((temp % 10) + 0x30);
				temp /= 10;
			}

			return s;
		}
	}
}
