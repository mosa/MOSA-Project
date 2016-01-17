// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public static class ConvI2Tests
	{
		public static bool ConvI2_I1(short expect, sbyte a)
		{
			return expect == a;
		}

		public static bool ConvI2_I2(short expect, short a)
		{
			return expect == a;
		}

		public static bool ConvI2_I4(short expect, int a)
		{
			return expect == ((short)a);
		}

		public static bool ConvI2_I8(short expect, long a)
		{
			return expect == ((short)a);
		}

		public static bool ConvI2_R4(short expect, float a)
		{
			return expect == ((short)a);
		}

		public static bool ConvI2_R8(short expect, double a)
		{
			return expect == ((short)a);
		}
	}
}
