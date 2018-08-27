// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class ConvI8Tests
	{
		[MosaUnitTest("I8", "I1")]
		public static bool ConvI8_I1(long expect, sbyte a)
		{
			return expect == a;
		}

		[MosaUnitTest("I8", "I2")]
		public static bool ConvI8_I2(long expect, short a)
		{
			return expect == a;
		}

		[MosaUnitTest("I8", "I4")]
		public static bool ConvI8_I4(long expect, int a)
		{
			return expect == a;
		}

		[MosaUnitTest("I8", "I8")]
		public static bool ConvI8_I8(long expect, long a)
		{
			return expect == a;
		}

		//TODO:
		//public static bool ConvI8_R4(long expect, float a)
		//{
		//    return expect == ((long)a);
		//}

		//TODO:
		//public static bool ConvI8_R8(long expect, double a)
		//{
		//    return expect == ((long)a);
		//}
	}
}
