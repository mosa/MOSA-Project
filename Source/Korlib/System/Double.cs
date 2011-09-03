/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// 
	/// </summary>
	public struct Double
	{
		public const double Epsilon = 4.9406564584124650e-324;
		public const double MaxValue = 1.7976931348623157e308;
		public const double MinValue = -1.7976931348623157e308;
		public const double NaN = 0.0d / 0.0d;
		public const double NegativeInfinity = -1.0d / 0.0d;
		public const double PositiveInfinity = 1.0d / 0.0d;

		internal double _value;

		public static bool IsNaN(double d)
		{
#pragma warning disable 1718
			return (d != d);
#pragma warning restore
		}

		public static bool IsNegativeInfinity(double d)
		{
			return d == NegativeInfinity;
		}

		public static bool IsPositiveInfinity(double d)
		{
			return d == PositiveInfinity;
		}

		public static bool IsInfinity(double d)
		{
			return (d == PositiveInfinity || d == NegativeInfinity);
		}

		public int CompareTo(double value)
		{
			if (IsPositiveInfinity(_value))
				if (IsPositiveInfinity(value))
					return 0;
			if (IsNegativeInfinity(_value))
				if (IsNegativeInfinity(value))
					return 0;

			if (IsNaN(value)) if (IsNaN(_value))
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

<<<<<<< HEAD
		public bool Equals(double value)
		{
			//return Equals((object)obj);
			if (IsNaN(value))
				return IsNaN(_value);

			return (value == _value);
		}

		//public override bool Equals(object obj)
		//{
		//    double value = (double)obj;

		//    if (IsNaN(value))
		//        return IsNaN(_value);

		//    return (value == _value);
		//}

		public override int GetHashCode()
		{
			return (int)_value;
=======
		public static bool IsNaN(float f)
		{
#pragma warning disable 1718
			return (f != f);
#pragma warning restore
>>>>>>> 477a0f502b2f82cd1ed0f94fd3c7c2b9f6a2fe7f
		}
	}
}
