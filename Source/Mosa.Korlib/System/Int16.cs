// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

/// <summary>
///
/// </summary>
[Serializable]
public struct Int16: IComparable, IComparable<short>, IEquatable<short>
{
	internal short m_value;

	public const short MaxValue = 32767;
	public const short MinValue = -32768;

	public int CompareTo(object value)
	{
		if (value == null) { return 1; }

		if (!(value is short)) { throw new ArgumentException("Argument Type Must Be Int16", "value"); }

		short s_value = ((short)value).m_value;

		if (m_value < s_value) return -1;
		if (m_value > s_value) return 1;

		return 0;
	}

	public int CompareTo(short value)
	{
		if (m_value < value) return -1;
		if (m_value > value) return 1;

		return 0;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is short)) { return false; }

		return m_value == ((short)obj).m_value;
	}

	public bool Equals(short obj)
	{
		return m_value == obj;
	}

	public override string ToString()
	{
		return int.CreateString((uint)m_value, true, false);
	}

	public override int GetHashCode()
	{
		return m_value;
	}

	public static bool TryParse(string s, out short result)
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

	public static short Parse(string s)
	{
		const string digits = "0123456789";
		short result = 0;

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
			result = (short)(result * 10 + ind);
		}

		if (neg) result *= -1;

		return result;
	}
}
