// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Math
{
	internal static class Division
	{
		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the quotient. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong udiv64(ulong n, ulong d)
		{
			DivUMod(n, d, out ulong quotient, out _);
			return quotient;
		}

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the remainder. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong umod64(ulong n, ulong d)
		{
			DivUMod(n, d, out _, out ulong remainder);
			return remainder;
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the quotient. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long sdiv64(long n, long d)
		{
			DivMod(n, d, out long quotient, out _);
			return quotient;
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the remainder. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long smod64(long n, long d)
		{
			DivMod(n, d, out _, out long remainder);
			return remainder;
		}

		/* Divides unsigned 32-bit N by unsigned 32-bit D and returns the quotient. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint udiv32(uint n, uint d)  // udivsi3
		{
			if (d == 0)
			{
				throw new DivideByZeroException();
			}
			else
			{
				DivUMod(n, d, out uint quotient, out _);
				return quotient;
			}
		}

		/* Divides unsigned 32-bit N by unsigned 32-bit D and returns the remainder. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint umod32(uint n, uint d)  // umodsi3
		{
			if (d == 0)
			{
				throw new DivideByZeroException();
			}
			else
			{
				DivUMod(n, d, out _, out uint remainder);
				return remainder;
			}
		}

		/* Divides signed 32-bit N by signed 32-bit D and returns the quotient. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int sdiv32(int a, int b)  // divsi3
		{
			int s_a = a >> 31;          // s_a = a < 0 ? -1 : 0
			int s_b = b >> 31;          // s_b = b < 0 ? -1 : 0

			a = (a ^ s_a) - s_a;        // negate if s_a == -1
			b = (b ^ s_b) - s_b;        // negate if s_b == -1
			s_a ^= s_b;                 // sign of quotient

			DivUMod((uint)a, (uint)b, out uint quotient, out _);

			return (int)(quotient ^ s_a - s_a);   // negate if s_a == -1
		}

		/* Divides signed 32-bit N by signed 32-bit D and returns the remainder. */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int smod32(int a, int b)  // modsi3
		{
			return a - sdiv32(a, b) * b;
		}

		private static void DivUMod(uint dividend, uint divisor, out uint quotient, out uint remainder)
		{
			uint n = dividend;
			uint d = divisor;

			uint bit = 1;
			uint res = 0;

			while ((d < n && bit != 0) && ((d & (1L << 31)) == 0))
			{
				d <<= 1;
				bit <<= 1;
			}

			while (bit != 0)
			{
				if (n >= d)
				{
					n -= d;
					res |= bit;
				}
				bit >>= 1;
				d >>= 1;
			}

			remainder = n;
			quotient = res;
		}

		private static void DivUMod(ulong dividend, ulong divisor, out ulong quotient, out ulong remainder)
		{
			bool isFlipped = false;
			quotient = dividend;
			remainder = 0;
			for (int i = 0; i < 65; i++)
			{
				remainder <<= 1;

				if (isFlipped)
				{
					remainder |= 1;
				}

				isFlipped = (quotient & 0x8000000000000000) != 0;

				quotient <<= 1;

				if (remainder >= divisor)
				{
					remainder -= divisor;
					quotient++;
				}
			}
		}

		private static void DivMod(long dividend, long divisor, out long quotient, out long remainder)
		{
			// Catch divide by zero and just return zero
			if (dividend == 0 || divisor == 0)
			{
				quotient = 0;
				remainder = 0;
				return;
			}

			// Catch divide by same number
			if (dividend == divisor)
			{
				quotient = 1;
				remainder = 0;
				return;
			}

			// Catch divide by one
			if (divisor == 1 || divisor == -1)
			{
				quotient = dividend * divisor;
				remainder = 0;
				return;
			}

			// Catch divide by same number but opposite sign
			if (dividend == (divisor * -1))
			{
				quotient = -1;
				remainder = 0;
				return;
			}

			// Determine the sign of the results and make the operands positive.
			int remainderSign = 1;
			int quotientSign = 1;
			ulong uQuotient = (ulong)dividend;
			ulong uRemainder = 0;
			ulong uDivisor = (ulong)divisor;
			bool isFlipped = false;
			if (dividend < 0)
			{
				if (dividend == -9223372036854775808)
					uQuotient = (ulong)dividend;
				else
					uQuotient = (ulong)-dividend;
				remainderSign = -1;
			}
			if (divisor < 0)
			{
				if (divisor == -9223372036854775808)
					uDivisor = (ulong)divisor;
				else
					uDivisor = (ulong)-divisor;
				quotientSign = -1;
			}

			quotientSign *= remainderSign;

			for (int i = 0; i < 65; i++)
			{
				uRemainder <<= 1;
				if (isFlipped)
				{
					uRemainder |= 1;
				}

				isFlipped = (uQuotient & 0x8000000000000000) != 0;

				uQuotient <<= 1;

				if (uRemainder >= uDivisor)
				{
					uRemainder -= uDivisor;
					uQuotient++;
				}
			}
			unchecked
			{
				quotient = (long)uQuotient;
				remainder = (long)uRemainder;
			}

			// Adjust sign of the results.
			if (quotient != 0)
			{
				int oldSign = ((ulong)quotient & 0x8000000000000000) != 0 ? -1 : 1;

				if (oldSign == -1 && quotientSign == -1)
					quotient++;

				quotient *= quotientSign;
			}

			if (remainder != 0)
			{
				int oldSign = ((ulong)remainder & 0x8000000000000000) != 0 ? -1 : 1;
				if (oldSign == -1 && remainderSign == -1)
					remainder++;

				remainder *= remainderSign;
			}
		}
	}
}
