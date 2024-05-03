// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// Single
/// </summary>
[Serializable]
public struct Single: IComparable, IComparable<float>, IEquatable<float>
{
	internal float m_value;

	public const float Epsilon = 1.4e-45f;
	public const float MaxValue = 3.40282346638528859e38f;
	public const float MinValue = -3.40282346638528859e38f;
	public const float NaN = 0.0f / 0.0f;
	public const float PositiveInfinity = 1.0f / 0.0f;
	public const float NegativeInfinity = -1.0f / 0.0f;

	internal const float NegativeZero = (float)-0.0;

	public static bool IsNaN(float s)
	{
#pragma warning disable 1718
		return (s != s);
#pragma warning restore 1718
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNegativeInfinity(float s)
	{
		return (s < 0.0f && (s == NegativeInfinity || s == PositiveInfinity));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPositiveInfinity(float s)
	{
		return (s > 0.0f && (s == NegativeInfinity || s == PositiveInfinity));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInfinity(float s)
	{
		return (s == PositiveInfinity || s == NegativeInfinity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNegative(float s)
	{
		return IsNegativeInfinity(s) || IsPositiveInfinity(s);
	}

	public int CompareTo(object value)
	{
		if (value == null) { return 1; }

		if (!(value is float))
		{
			throw new ArgumentException("Argument Type Must Be Single", "value");
		}

		float f_value = ((float)value).m_value;

		if (IsPositiveInfinity(m_value) && IsPositiveInfinity(f_value))
			return 0;

		if (IsNegativeInfinity(m_value) && IsNegativeInfinity(f_value))
			return 0;

		if (IsNaN(f_value))
		{
			if (IsNaN(m_value))
				return 0;
			else
				return 1;
		}

		if (IsNaN(m_value))
		{
			if (IsNaN(f_value))
				return 0;
			else
				return -1;
		}

		if (m_value < f_value) { return -1; }
		if (m_value > f_value) { return 1; }

		return 0;
	}

	public int CompareTo(float value)
	{
		if (IsPositiveInfinity(m_value) && IsPositiveInfinity(value))
			return 0;

		if (IsNegativeInfinity(m_value) && IsNegativeInfinity(value))
			return 0;

		if (IsNaN(value))
		{
			if (IsNaN(m_value))
				return 0;
			else
				return 1;
		}

		if (IsNaN(m_value))
		{
			if (IsNaN(value))
				return 0;
			else
				return -1;
		}

		if (m_value > value) { return 1; }
		if (m_value < value) { return -1; }

		return 0;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Single)) { return false; }

		float value = ((float)obj).m_value;

		if (m_value == value) { return true; }

		return (IsNaN(m_value) && IsNaN(value));
	}

	public bool Equals(float value)
	{
		if (m_value == value) { return true; }

		return (IsNaN(m_value) && IsNaN(value));
	}

	public override int GetHashCode()
	{
		var bits = BitConverter.SingleToInt32Bits(m_value);

		if (((bits - 1) & 0x7FFFFFFF) >= 0x7F800000)
		{
			bits &= 0x7F800000;
		}

		return bits;
	}
}
