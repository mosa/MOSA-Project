// Copyright (c) MOSA Project. Licensed under the New BSD License.

// References: Hackers-Delight
// https://github.com/hcs0/Hackers-Delight/blob/master/magic.c.txt
// https://github.com/hcs0/Hackers-Delight/blob/master/magicu.c.txt

namespace Mosa.Compiler.Common;

public static class DivisionMagicNumber
{
	public static (uint M, uint s) GetMagicNumber(int d)
	{
		// Must have 2 <= d <= 2**31-1
		// or -2**31 <= d <= -2.

		int p;
		uint ad, anc, delta, q1, r1, q2, r2, t;

		const uint two31 = 0x80000000;// 2**31.

		ad = (uint)Math.Abs(d);

		t = two31 + ((uint)d >> 31);
		anc = t - 1 - t % ad; ; // Absolute value of nc.
		p = 31; // Init. p.
		q1 = two31 / anc; // Init. q1 = 2**p/|nc|.
		r1 = two31 - q1 * anc;// Init. r1 = rem(2**p, |nc|).

		q2 = two31 / ad; // Init. q2 = 2**p/|d|.
		r2 = two31 - q2 * ad; // Init. r2 = rem(2**p, |d|).
		do
		{
			p = p + 1;
			q1 = 2 * q1; // Update q1 = 2**p/|nc|.
			r1 = 2 * r1; // Update r1 = rem(2**p, |nc|).
			if (r1 >= anc)
			{ // (Must be an uint
				q1 = q1 + 1; // comparison here).
				r1 = r1 - anc;
			}

			q2 = 2 * q2; // Update q2 = 2**p/|d|.
			r2 = 2 * r2; // Update r2 = rem(2**p, |d|).
			if (r2 >= ad)
			{ // (Must be an uint
				q2 = q2 + 1; // comparison here).
				r2 = r2 - ad;
			}

			delta = ad - r2;
		} while (q1 < delta || (q1 == delta && r1 == 0));

		var M = (int)q2 + 1;
		if (d < 0) M = -M; // Magic number and
		var s = p - 32; // shift amount to return.

		return ((uint)M, (uint)s);
	}

	public static (uint M, uint s, bool a) GetMagicNumber(uint d)
	{
		// Must have 1 <= d <= 2**32-1.
		int p;
		uint nc, delta, q1, r1, q2, r2;

		var a = false; // Initialize "add" indicator.
		nc = (uint)(-1 - (-d) % d); // Unsigned arithmetic here.
		p = 31; // Init. p.
		q1 = 0x80000000 / nc; // Init. q1 = 2**p/nc.
		r1 = 0x80000000 - q1 * nc;// Init. r1 = rem(2**p, nc).
		q2 = 0x7FFFFFFF / d; // Init. q2 = (2**p - 1)/d.
		r2 = 0x7FFFFFFF - q2 * d; // Init. r2 = rem(2**p - 1, d).
		do
		{
			p = p + 1;
			if (r1 >= nc - r1)
			{
				q1 = 2 * q1 + 1; // Update q1.
				r1 = 2 * r1 - nc;
			} // Update r1.
			else
			{
				q1 = 2 * q1;
				r1 = 2 * r1;
			}
			if (r2 + 1 >= d - r2)
			{
				if (q2 >= 0x7FFFFFFF) a = true;
				q2 = 2 * q2 + 1; // Update q2.
				r2 = 2 * r2 + 1 - d;
			} // Update r2.
			else
			{
				if (q2 >= 0x80000000) a = true;
				q2 = 2 * q2;
				r2 = 2 * r2 + 1;
			}
			delta = d - 1 - r2;
		} while (p < 64 &&
				(q1 < delta || (q1 == delta && r1 == 0)));

		var M = q2 + 1; // Magic number
		var s = (uint)(p - 32); // and shift amount to return
		return (M, s, a);
	}

	public static (uint M, uint s, bool a) GetMagicNumber2(uint d)
	{
		// Must have 1 <= d <= 2**32-1.
		int p;
		uint p32 = 0u, q, r, delta;

		var a = false; // Initialize "add" indicator.
		p = 31; // Initialize p.
		q = 0x7FFFFFFF / d; // Initialize q = (2**p - 1)/d.
		r = 0x7FFFFFFF - q * d; // Init. r = rem(2**p - 1, d).
		do
		{
			p = p + 1;
			if (p == 32) p32 = 1; // Set p32 = 2**(p-32).
			else p32 = 2 * p32;
			if (r + 1 >= d - r)
			{
				if (q >= 0x7FFFFFFF) a = true;
				q = 2 * q + 1; // Update q.
				r = 2 * r + 1 - d; // Update r.
			}
			else
			{
				if (q >= 0x80000000) a = true;
				q = 2 * q;
				r = 2 * r + 1;
			}
			delta = d - 1 - r;
		} while (p < 64 && p32 < delta);
		var M = q + 1; // Magic number and
		var s = (uint)(p - 32); // shift amount to return
		return (M, s, a); // (magu.a was set above).
	}
}
