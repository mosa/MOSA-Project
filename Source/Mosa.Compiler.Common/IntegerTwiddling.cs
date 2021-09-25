// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	public static class IntegerTwiddling
	{
		public static bool IsAddOverflow(ulong a, ulong b)
		{
			return (b > 0) && (a > (ulong.MaxValue - b));
		}

		public static bool IsAddOverflow(uint a, uint b)
		{
			return (b > 0) && (a > (uint.MaxValue - b));
		}

		public static bool IsAddOverflow(int a, int b)
		{
			if (a > 0 && b > 0)
				if (b > (int.MaxValue - a))
					return true;

			if (a < 0 && b < 0)
				if (b < (int.MinValue - a))
					return true;

			return false;
		}

		public static bool IsAddOverflow(long a, long b)
		{
			if (a > 0 && b > 0)
				if (b > (long.MaxValue - a))
					return true;

			if (a < 0 && b < 0)
				if (b < (long.MinValue - a))
					return true;

			return false;
		}

		public static bool IsAddOverflow(uint a, uint b, bool carry)
		{
			if (IsAddOverflow(a, b))
				return true;

			if (carry & (a + b) == uint.MaxValue)
				return true;

			return false;
		}

		public static bool IsMultiplyOverflow(uint a, uint b)
		{
			var r = (ulong)a * (ulong)b;

			return r > uint.MaxValue;
		}

		public static bool IsMultiplyOverflow(int a, int b)
		{
			var z = a * b;
			return (b < 0 && a == int.MinValue) | (b != 0 && z / b != a);
		}

		public static bool IsMultiplyOverflow(ulong a, ulong b)
		{
			if (a == 0 | b == 0)
				return false;

			var r = a * b;
			var r2 = r / b;

			return r2 == a;
		}

		public static bool IsMultiplyOverflow(long a, long b)
		{
			var z = a * b;
			return (b < 0 && a == long.MinValue) | (b != 0 && z / b != a);
		}

		public static bool HasSignBitSet32(int a)
		{
			return a <= 0;
		}

		public static bool HasSignBitSet32(uint a)
		{
			return HasSignBitSet32((int)a);
		}

		public static bool HasSignBitSet64(long a)
		{
			return a <= 0;
		}

		public static bool HasSignBitSet64(ulong a)
		{
			return HasSignBitSet64((long)a);
		}
	}
}
