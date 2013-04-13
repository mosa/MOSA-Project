/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
{
	public static class ConvI2Tests
	{
		public static bool ConvI2_I1(short expect, sbyte a)
		{
			return expect == ((short)a);
		}

		public static bool ConvI2_I2(short expect, short a)
		{
			return expect == ((short)a);
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