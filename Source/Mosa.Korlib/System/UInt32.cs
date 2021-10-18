﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	[Serializable]
	public struct UInt32: IComparable, IComparable<uint>, IEquatable<uint>
	{
		internal uint m_value;

		public const uint MaxValue = 0xffffffff;
		public const uint MinValue = 0;

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is uint)) { throw new ArgumentException("Argument Type Must Be UInt32", "value"); }

			uint u_value = ((uint)value).m_value;

			if (m_value < u_value) return -1;
			if (m_value > u_value) return 1;

			return 0;
		}

		public int CompareTo(uint value)
		{
			if (m_value < value) return -1;
			if (m_value > value) return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is uint)) { return false; }

			return m_value == ((uint)obj).m_value;
		}

		public bool Equals(uint value)
		{
			return m_value == value;
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

		public static uint Parse(string s)
		{
			if (s == null)
				throw new ArgumentNullException();

			if (s.Length == 0)
				throw new FormatException();

			uint result;
			if (TryParse(s, out result))
				return result;

			throw new FormatException();
		}

		public static bool TryParse(string s, out uint result)
		{
			int len = s.Length;
			uint n = 0;
			result = 0;
			var i = 0;
			while (i < len)
			{
				if (n > (0xFFFFFFFF / 10))
				{
					return false;
				}
				n *= 10;
				if (s[i] != '\0')
				{
					uint newN = n + (uint)(s[i] - '0');

					// Detect an overflow here...
					if (newN < n)
					{
						return false;
					}
					n = newN;
				}
				i++;
			}
			result = n;
			return true;
		}
	}
}
