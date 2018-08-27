// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class ConvI1Tests
	{
		[MosaUnitTest("I1", "I1")]
		public static bool ConvI1_I1(sbyte expect, sbyte a)
		{
			return expect == a;
		}

		[MosaUnitTest("I1", "I2")]
		public static bool ConvI1_I2(sbyte expect, short a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I4")]
		public static bool ConvI1_I4(sbyte expect, int a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I8")]
		public static bool ConvI1_I8(sbyte expect, long a)
		{
			return expect == (sbyte)a;
		}

		public static bool ConvI1_R4(sbyte expect, float a)
		{
			return expect == (sbyte)a;
		}

		public static bool ConvI1_R8(sbyte expect, double a)
		{
			return expect == (sbyte)a;
		}
	}
}
