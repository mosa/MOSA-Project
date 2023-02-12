// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Math;

internal static class Multiplication
{
	/* Multiplies 64-bit A by 64-bit B and returns the result and carry. */

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong Mul64Carry(ulong a, ulong b, out bool carry)
	{
		var r = a * b;

		if (a == 0 || b == 0)
		{
			carry = false;
		}
		else
		{
			carry = ulong.MaxValue / a < b;
		}

		return r;
	}

	/* Multiplies 64-bit A by 64-bit B and returns the result and overflow. */

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static long Mul64Overflow(long a, long b, out bool overflow)
	{
		var r = a * b;

		if (a == 0 || b == 0)
		{
			overflow = false;
		}
		else
		{
			overflow = (b < 0 && a == long.MinValue) | (b != 0 && r / b != a);
		}

		return r;
	}
}
