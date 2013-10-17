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
	public struct Single
	{
		public const float Epsilon = 1.4e-45f;
		public const float MaxValue = 3.40282346638528859e38f;
		public const float MinValue = -3.40282346638528859e38f;
		public const float NaN = 0.0f / 0.0f;
		public const float PositiveInfinity = 1.0f / 0.0f;
		public const float NegativeInfinity = -1.0f / 0.0f;

		internal float _value;

		public static bool IsNaN(float s)
		{
#pragma warning disable 1718
			return (s != s);
#pragma warning restore
		}

		public static bool IsNegativeInfinity(float s)
		{
			return (s < 0.0f && (s == NegativeInfinity || s == PositiveInfinity));
		}

		public static bool IsPositiveInfinity(double s)
		{
			return (s > 0.0f && (s == NegativeInfinity || s == PositiveInfinity));
		}

		public static bool IsInfinity(float s)
		{
			return (s == PositiveInfinity || s == NegativeInfinity);
		}

		public int CompareTo(float value)
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

		public bool Equals(float value)
		{
			//return Equals((object)obj);
			if (IsNaN(value))
				return IsNaN(_value);

			return (value == _value);
		}

		//public override bool Equals(object obj)
		//{
		//    float value = (float)obj;

		//    if (IsNaN(value))
		//        return IsNaN(_value);

		//    return (value == _value);
		//}

		public override int GetHashCode()
		{
			return (int)_value;
		}
	}
}