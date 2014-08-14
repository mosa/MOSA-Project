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

	public unsafe static class Division
	{

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the quotient. */

		public static long udiv64(long n, long d)
		{
			long quotient;
			long remainder;
			DivUmod(n, d, out quotient, out remainder);
			return quotient;
		}

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the remainder. */

		public static long umod64(long n, long d)
		{
			long quotient;
			long remainder;
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

		public static void DivUmod(long dividend, long divisor, out long quotient, out long remainder)
		{
			quotient = dividend;
			remainder = 0;
			for (int i = 0; i < 64; i++)
			{
				// Left shift Remainder:Quotient by 1
				remainder <<= 1;
				if (quotient < 0)
				{
					fixed (long* ptr = &remainder)
					{
						(*((Long*)ptr))._lo |= 1;
					}
				}
				quotient <<= 1;

				if (remainder >= divisor)
				{
					remainder -= divisor;
					quotient++;
				}
			}
		}

		public static void DivMod(long dividend, long divisor, out long quotient, out long remainder)
		{
			// Determine the sign of the results and make the operands positive.
			int remainderSign = 1;
			int quotientSign = 1;
			if (dividend < 0)
			{
				dividend = -dividend;
				remainderSign = -1;
			}
			if (divisor < 0)
			{
				divisor = -divisor;
				quotientSign = -1;
			}
			quotientSign *= remainderSign;

			quotient = dividend;
			remainder = 0;
			for (int i = 0; i < 64; i++)
			{
				// Left shift Remainder:Quotient by 1
				remainder <<= 1;
				if (quotient < 0)
				{
					fixed (long* ptr = &remainder)
					{
						(*((Long*)ptr))._lo |= 1;
					}
				}
				quotient <<= 1;

				if (remainder >= divisor)
				{
					remainder -= divisor;
					quotient++;
				}
			}

			// Adjust sign of the results.
			fixed (long* ptr = &quotient)
			{
				(*((Long*)ptr))._hi *= quotientSign;
			}
			fixed (long* ptr = &remainder)
			{
				(*((Long*)ptr))._hi *= remainderSign;
			}
		}
	}
}