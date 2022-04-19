// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Basic
{
	public static class ConvEdgeTests
	{
		[MosaUnitTest(Series = "R4")]
		public static long ConvR4I8(float a)
		{
			return (long)a;
		}

		[MosaUnitTest(Series = "R4")]
		public static ulong ConvR4U8(float a)
		{
			return (ulong)a;
		}

		[MosaUnitTest(Series = "R8")]
		public static long ConvR8I8(double a)
		{
			return (long)a;
		}

		[MosaUnitTest(Series = "R8")]
		public static ulong ConvR8U8(double a)
		{
			return (ulong)a;
		}
	}
}
