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

	public static class ConvI8Tests
	{
		public static bool ConvI8_I1(long expect, sbyte a)
		{
			return expect == ((long)a);
		}

		public static bool ConvI8_I2(long expect, short a)
		{
			return expect == ((long)a);
		}

		public static bool ConvI8_I4(long expect, int a)
		{
			return expect == ((long)a);
		}

		public static bool ConvI8_I8(long expect, long a)
		{
			return expect == ((long)a);
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