// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Double
	/// </summary>
	public struct Double
	{
		public const double Epsilon = 4.9406564584124650e-324d;
		public const double MaxValue = 1.7976931348623157e308d;
		public const double MinValue = -1.7976931348623157e308d;
		public const double NaN = 0.0d / 0.0d;
		public const double NegativeInfinity = -1.0d / 0.0d;
		public const double PositiveInfinity = 1.0d / 0.0d;

		internal const double NegativeZero = -0.0;

		internal double _value;

		public static bool IsNaN(double d)
		{
#pragma warning disable 1718
			return (d != d);
#pragma warning restore
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

		public int CompareTo(double value)
		{
			if (IsPositiveInfinity(_value) && IsPositiveInfinity(value))
				return 0;
			if (IsNegativeInfinity(_value) && IsNegativeInfinity(value))
				return 0;

			if (IsNaN(value))
				if (IsNaN(_value))
					return 0;
				else
					return 1;

			if (IsNaN(_value))
				if (IsNaN(value))
					return 0;
				else
					return -1;

			if (_value > value)
				return 1;
			else if (_value < value)
				return -1;
			else
				return 0;
		}

		public bool Equals(double value)
		{
			if (IsNaN(value))
				return IsNaN(_value);

			return (value == _value);
		}

		public override bool Equals(object obj)
		{
			double value = (double)obj;

			if (IsNaN(value))
				return IsNaN(_value);

			return (value == _value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] // 64-bit constants make the IL unusually large that makes the inliner to reject the method
		public override int GetHashCode()
		{
			var bits = BitConverter.DoubleToInt64Bits(_value);

			if (((bits - 1) & 0x7FFFFFFFFFFFFFFF) >= 0x7FF0000000000000)
			{
				bits &= 0x7FF0000000000000;
			}

			return unchecked((int)bits) ^ ((int)(bits >> 32));
		}
	}
}
