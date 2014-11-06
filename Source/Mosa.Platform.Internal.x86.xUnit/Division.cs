/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.Platform.Internal.x86
{
	public unsafe struct Long
	{
		public uint _lo;
		public int _hi;
	}

	public unsafe struct ULong
	{
		public uint _lo;
		public uint _hi;
	}

	public unsafe static class Division
	{

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the quotient. */

		public static ulong udiv64(ulong n, ulong d)
		{
			ulong quotient;
			ulong remainder;
			DivUmod(n, d, out quotient, out remainder);
			return quotient;
		}

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the remainder. */

		public static ulong umod64(ulong n, ulong d)
		{
			ulong quotient;
			ulong remainder;
			DivUmod(n, d, out quotient, out remainder);
			return remainder;
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the quotient. */

		public static long sdiv64(long n, long d)
		{
			long quotient;
			long remainder;
			DivMod(n, d, out quotient, out remainder);
			return quotient;
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the remainder. */

		public static long smod64(long n, long d)
		{
			long quotient;
			long remainder;
			DivMod(n, d, out quotient, out remainder);
			return remainder;
		}

		public static void DivUmod(ulong dividend, ulong divisor, out ulong quotient, out ulong remainder)
		{
			bool isFlipped = false;
			quotient = dividend;
			remainder = 0;
			for (int i = 0; i < 65; i++)
			{
				// Left shift Remainder:Quotient by 1
				remainder <<= 1;
				if (isFlipped)
				{
					fixed (ulong* ptr = &remainder)
					{
						(*((ULong*)ptr))._lo |= 1;
					}
				}
				fixed (ulong* ptr = &quotient)
				{
					isFlipped = ((*((ULong*)ptr))._hi & 0x80000000) == 0x80000000;
				}
				quotient <<= 1;

				if (remainder >= divisor)
				{
					remainder -= divisor;
					quotient++;
				}
			}
		}

		public unsafe static void DivMod(long dividend, long divisor, out long quotient, out long remainder)
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
				// Left shift Remainder:Quotient by 1
				uRemainder <<= 1;
				if (isFlipped)
				{
					(*((ULong*)&uRemainder))._lo |= 1;
				}
				isFlipped = ((*((ULong*)&uQuotient))._hi & 0x80000000) == 0x80000000;
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
				fixed (long* ptr = &quotient)
				{
					int oldSign = (((*((Long*)ptr))._hi & 0x80000000) == 0x80000000) ? -1 : 1;
					if ((oldSign == -1 && quotientSign == -1))
						quotient += 1;

					quotient *= quotientSign;
				}
			}
			if (remainder != 0)
			{
				fixed (long* ptr = &remainder)
				{
					int oldSign = (((*((Long*)ptr))._hi & 0x80000000) == 0x80000000) ? -1 : 1;
					if ((oldSign == -1 && remainderSign == -1))
						remainder += 1;

					remainder *= remainderSign;
				}
			}
		}
	}
}