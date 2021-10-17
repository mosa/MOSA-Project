// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Double
	/// </summary>
	public struct Double: IComparable, IComparable<double>
	{
		public const double Epsilon = 4.9406564584124650e-324d;
		public const double MaxValue = 1.7976931348623157e308d;
		public const double MinValue = -1.7976931348623157e308d;
		public const double NaN = 0.0d / 0.0d;
		public const double NegativeInfinity = -1.0d / 0.0d;
		public const double PositiveInfinity = 1.0d / 0.0d;

		internal const double NegativeZero = -0.0;

		internal double m_value;

		public static bool IsNaN(double d)
		{
#pragma warning disable 1718
			return (d != d);
#pragma warning restore 1718
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegativeInfinity(double d)
		{
			return (d < 0.0d && (d == NegativeInfinity || d == PositiveInfinity));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPositiveInfinity(double d)
		{
			return (d > 0.0d && (d == NegativeInfinity || d == PositiveInfinity));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(double d)
		{
			return (d == PositiveInfinity || d == NegativeInfinity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegative(double d)
		{
			return IsNegativeInfinity(d) || IsPositiveInfinity(d);
		}

		public int CompareTo(object value)
		{
			if (value == null) { return 1; }

			if (!(value is double))
			{
				throw new ArgumentException("Argument Type Must Be Double", "value");
			}

			double d_value = ((double)value).m_value;

			if (IsPositiveInfinity(m_value) && IsPositiveInfinity(d_value))
				return 0;

			if (IsNegativeInfinity(m_value) && IsNegativeInfinity(d_value))
				return 0;

			if (IsNaN(d_value))
			{
				if (IsNaN(m_value))
					return 0;
				else
					return 1;
			}

			if (IsNaN(m_value))
			{
				if (IsNaN(d_value))
					return 0;
				else
					return -1;
			}

			if (m_value > d_value) { return 1; }
			if (m_value < d_value) { return -1; }

			return 0;
		}

		public int CompareTo(double value)
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
			if (!(obj is Double)) { return false; }

			double value = ((double)obj).m_value;

			if (m_value == value) { return true; }

			return (IsNaN(m_value) && IsNaN(value));
		}

		public bool Equals(double value)
		{
			if (m_value == value) { return true; }

			return (IsNaN(m_value) && IsNaN(value));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] // 64-bit constants make the IL unusually large that makes the inliner to reject the method
		public override int GetHashCode()
		{
			long bits = BitConverter.DoubleToInt64Bits(m_value);

			if (((bits - 1) & 0x7FFFFFFFFFFFFFFF) >= 0x7FF0000000000000)
			{
				bits &= 0x7FF0000000000000;
			}

			return unchecked((int)bits) ^ ((int)(bits >> 32));
		}
	}
}
