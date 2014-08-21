/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

//Source:
// http://www.cs.usfca.edu/~benson/cs326/pintos/pintos/src/lib/arithmetic.c

//License:
// http://www.stanford.edu/class/cs140/projects/pintos/pintos_14.html#SEC172

//Pintos, including its documentation, is subject to the following license:
//Copyright © 2004, 2005, 2006 Board of Trustees, Leland Stanford Jr. University. All rights reserved.
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//Code derived from Nachos is subject to the following license:
//Copyright © 1992-1996 The Regents of the University of California. All rights reserved.
//Permission to use, copy, modify, and distribute this software and its documentation for any purpose, without fee, and without written agreement is hereby granted, provided that the above copyright notice and the following two paragraphs appear in all copies of this software.
//IN NO EVENT SHALL THE UNIVERSITY OF CALIFORNIA BE LIABLE TO ANY PARTY FOR DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE UNIVERSITY OF CALIFORNIA HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//THE UNIVERSITY OF CALIFORNIA SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE SOFTWARE PROVIDED HEREUNDER IS ON AN "AS IS" BASIS, AND THE UNIVERSITY OF CALIFORNIA HAS NO OBLIGATION TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.

namespace Mosa.Platform.Internal.x86
{
	public static class Division
	{
		/* Returns the number of leading zero bits in X,  which must be nonzero. */

		public static int nlz(uint x)
		{
			int n = 0;

			if (x <= 0x0000FFFF)
			{
				n += 16;
				x <<= 16;
			}
			if (x <= 0x00FFFFFF)
			{
				n += 8;
				x <<= 8;
			}
			if (x <= 0x0FFFFFFF)
			{
				n += 4;
				x <<= 4;
			}
			if (x <= 0x3FFFFFFF)
			{
				n += 2;
				x <<= 2;
			}
			if (x <= 0x7FFFFFFF)
				n++;

			return n;
		}

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the quotient. */

		public static ulong udiv64(ulong n, ulong d)
		{
			if ((d >> 32) == 0)
			{
				ulong b = 1L << 32;
				uint n1 = (uint)(n >> 32);
				uint n0 = (uint)n;
				uint d0 = (uint)d;

				return Native.Div(b * (n1 % d0) + n0, d0) + b * (n1 / d0);
			}
			else
			{
				if (n < d)
					return 0;
				else
				{
					uint d1 = (uint)(d >> 32);
					int s = nlz(d1);
					ulong q = Native.Div(n >> 1, (uint)((d << s) >> 32)) >> (31 - s);
					return n - (q - 1) * d < d ? q - 1 : q;
				}
			}
		}

		/* Divides unsigned 64-bit N by unsigned 64-bit D and returns the remainder. */

		public static uint umod64(ulong n, ulong d)
		{
			return (uint)(n - d * udiv64(n, d));
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the quotient. */

		public static long sdiv64(long n, long d)
		{
			ulong n_abs = (ulong)(n >= 0 ? n : -n);
			ulong d_abs = (ulong)(d >= 0 ? d : -d);
			ulong q_abs = udiv64(n_abs, d_abs);
			return (n < 0) == (d < 0) ? (long)q_abs : -(long)q_abs;
		}

		/* Divides signed 64-bit N by signed 64-bit D and returns the remainder. */

		public static int smod64(long n, long d)
		{
			return (int)(n - d * sdiv64(n, d));
		}
	}
}