﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Runtime.x86;

namespace Mosa.Runtime.Math.x86
{
	internal static class Division
	{
		/* Single and Double Remainder */

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static double RemR8(double n, double d)
		{
			if (double.IsInfinity(d))
			{
				if (double.IsInfinity(n))
					return double.NaN;
				else
					return n;
			}
			else if (double.IsInfinity(n))
			{
				return double.NaN;
			}

			if (double.IsNaN(n) || double.IsNaN(d))
				return double.NaN;

			if (d == 0d)
			{
				if (n > 0d)
					return double.PositiveInfinity;
				else
					return double.NegativeInfinity;
			}

			if (n == 0d)
				return n;

			return Native.Remainder(n, d);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static float RemR4(float n, float d)
		{
			if (float.IsInfinity(d))
			{
				if (float.IsInfinity(n))
					return float.NaN;
				else
					return n;
			}
			else if (float.IsInfinity(n))
			{
				return float.NaN;
			}

			if (float.IsNaN(n) || float.IsNaN(d))
				return float.NaN;

			if (d == 0f)
			{
				if (n > 0f)
					return float.PositiveInfinity;
				else
					return float.NegativeInfinity;
			}

			if (n == 0f)
				return n;

			return Native.Remainder(n, d);
		}
	}
}
