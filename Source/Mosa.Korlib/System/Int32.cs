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
		return CreateString((uint)m_value, true, false);
	}

	public string ToString(string format)
	{
		return CreateString((uint)m_value, false, true);
	}

	unsafe internal static string CreateString(uint value, bool signed, bool hex)
	{
		int offset = 0;

		uint uvalue = value;
		var divisor = hex ? 16u : 10;
		int length = 0;
		int count = 0;
		uint temp;
		bool negative = false;

		if (value < 0 && !hex && signed)
		{
			count++;
			uvalue = (uint)-value;
			negative = true;
		}

		temp = uvalue;

		do
		{
			temp /= divisor;
			count++;
		}
		while (temp != 0);

		length = count;
		var result = String.InternalAllocateString(length);

		var chars = result.first_char;

		if (negative)
		{
			*(chars + offset) = '-';
			offset++;
			count--;
		}

		for (var i = 0; i < count; i++)
		{
			var remainder = uvalue % divisor;

			if (remainder < 10)
				*(chars + offset + count - 1 - i) = (char)('0' + remainder);
			else
				*(chars + offset + count - 1 - i) = (char)('A' + remainder - 10);

			uvalue /= divisor;
		}

		return result;
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
			result = result * 10 + ind;
		}

		if (neg) result *= -1;

		return result;
	}
}
