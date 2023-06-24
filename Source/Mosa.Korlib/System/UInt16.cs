// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

/// <summary>
///
/// </summary>
[Serializable]
public struct UInt16: IComparable, IComparable<ushort>, IEquatable<ushort>
{
	internal ushort m_value;

	public const ushort MaxValue = 0xffff;
	public const ushort MinValue = 0;

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

	public static ushort Parse(string s)
	{
		const string digits = "0123456789";
		ushort result = 0;

		for (int i = 0; i < s.Length; i++)
		{
			int ind = digits.IndexOf(s[i]);
			if (ind == -1)
			{
				throw new Exception("Format is incorrect");
			}
			result = (ushort)(result * 10 + ind);
		}

		return result;
	}
}
