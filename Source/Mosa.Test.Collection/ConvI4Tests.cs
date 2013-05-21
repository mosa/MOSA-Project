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
	public static class ConvI4Tests
	{
		public static bool ConvI4_I1(int expect, sbyte a)
		{
			return expect == ((int)a);
		}

		public static bool ConvI4_I2(int expect, short a)
		{
			return expect == ((int)a);
		}

		public static bool ConvI4_I4(int expect, int a)
		{
			return expect == ((int)a);
		}

		public static bool ConvI4_I8(int expect, long a)
		{
			return expect == ((int)a);
		}

		public static bool ConvI4_R4(int expect, float a)
		{
			return expect == ((int)a);
		}

		public static bool ConvI4_R8(int expect, double a)
		{
			return expect == ((int)a);
		}
	}
}