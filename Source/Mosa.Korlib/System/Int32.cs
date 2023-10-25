// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

/// <summary>
/// Int32
/// </summary>
[Serializable]
public struct Int32: IComparable, IComparable<int>, IEquatable<int>
{
	internal int m_value;

	public const int MaxValue = 0x7fffffff;
	public const int MinValue = -2147483648;

	public int CompareTo(object value)
	{
		if (value == null) { return 1; }

		if (!(value is int)) { throw new ArgumentException("Argument Type Must Be Int32", "value"); }

		int i_value = ((int)value).m_value;

		if (m_value < i_value) return -1;
		if (m_value > i_value) return 1;

		return 0;
	}

	public int CompareTo(int value)
	{
		if (m_value < value) return -1;
		if (m_value > value) return 1;

		return 0;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is int)) { return false; }

		return m_value == ((int)obj).m_value;
	}

	public bool Equals(int obj)
	{
		return m_value == obj;
	}

	public override string ToString()
	{
		return Numbers.Int32ToString(m_value);
	}

	public string ToString(string format)
	{		
		// TODO: Actual formats
		return Numbers.Int32ToString(m_value);
	}

	public override int GetHashCode()
	{
		return m_value;
	}

	public static bool TryParse(string s, out int result)
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

	public static int Parse(string s)
	{
		const string digits = "0123456789";
		int result = 0;

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
			result = (result * 10) + ind;
		}

		if (neg) result *= -1;

		return result;
	}
}
