// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

/// <summary>
///
/// </summary>
[Serializable]
public struct Int64: IComparable, IComparable<long>, IEquatable<long>
{
	internal long m_value;

	public const long MaxValue = 0x7fffffffffffffff;
	public const long MinValue = -9223372036854775808;

	public int CompareTo(object value)
	{
		if (value == null) { return 1; }

		if (!(value is long)) { throw new ArgumentException("Argument Type Must Be Int64", "value"); }

		long l_value = ((long)value).m_value;

		if (m_value < l_value) return -1;
		if (m_value > l_value) return 1;

		return 0;
	}

	public int CompareTo(long value)
	{
		if (m_value < value) return -1;
		if (m_value > value) return 1;

		return 0;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is long)) { return false; }

		return m_value == ((long)obj).m_value;
	}

	public bool Equals(long obj)
	{
		return m_value == obj;
	}

	public override int GetHashCode()
	{
		return unchecked((int)m_value) ^ (int)(m_value >> 32);
	}

	public static bool TryParse(string s, out long result)
	{
		try
		{
			result = Parse(s);
			return true;
		}
		catch
		{
			result = 0;
			return false;
		}
	}

	public static long Parse(string s)
	{
		const string digits = "0123456789";
		long result = 0;

		int z = 0;
		bool neg = false;

		if (s.Length >= 1)
		{
			if (s[0] == '+') z = 1;
			if (s[0] == '-')
			{
				z = 1;
				neg = true;
			}
		}

		for (int i = z; i < s.Length; i++)
		{
			int ind = digits.IndexOf(s[i]);
			if (ind == -1)
			{
				throw new Exception("Format is incorrect");
			}
			result = result * 10 + ind;
		}

		if (neg) result *= -1;

		return result;
	}
}
