// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

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
		if (obj is Byte)
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

	public static bool TryParse(string s, out byte result)
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

	public static byte Parse(string s)
	{
		const string digits = "0123456789";
		byte result = 0;

		for (int i = 0; i < s.Length; i++)
		{
			int ind = digits.IndexOf(s[i]);
			if (ind == -1)
			{
				throw new Exception("Format is incorrect");
			}
			result = (byte)(result * 10 + ind);
		}

		return result;
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
