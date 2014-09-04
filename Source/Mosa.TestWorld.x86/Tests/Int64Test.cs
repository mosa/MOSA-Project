/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.TestWorld.x86.Tests
{
	public class Int64Test : KernelTest
	{
		public Int64Test()
			: base("Int64")
		{
			testMethods.Add(Int64Test1);
			testMethods.Add(Int64Test2);
			testMethods.Add(Int64Test3);
			testMethods.Add(Int64Test4);
			testMethods.Add(Int64Test5);
			testMethods.Add(Int64Test6);
			testMethods.Add(Int64Test7);
			testMethods.Add(Int64Test8);
			testMethods.Add(Int64Test9);
			testMethods.Add(Int64Test10);
		}

		public static bool Int64Test1()
		{
			ulong var1 = 32uL;
			ulong var2 = 16uL;
			return (var1 / var2) == 2uL;
		}

		public static bool Int64Test2()
		{
			ulong var1 = 222222222222uL;
			ulong var2 = 2uL;
			return (var1 / var2) == 111111111111uL;
		}

		public static bool Int64Test3()
		{
			ulong var1 = 666666666663uL;
			ulong var2 = 5uL;
			return (var1 % var2) == 3uL;
		}

		public static bool Int64Test4()
		{
			long var1 = 32L;
			long var2 = 16L;
			return (var1 / var2) == 2L;
		}

		public static bool Int64Test5()
		{
			long var1 = 222222222222L;
			long var2 = 2L;
			return (var1 / var2) == 111111111111L;
		}

		public static bool Int64Test6()
		{
			long var1 = 666666666663L;
			long var2 = 5L;
			return (var1 % var2) == 3L;
		}

		public static bool Int64Test7()
		{
			ulong var1 = 666666666663uL;
			ulong var2 = 5uL;
			return (var1 + var2) == 666666666668uL;
		}

		public static bool Int64Test8()
		{
			long var1 = 666666666663L;
			long var2 = 5L;
			return (var1 + var2) == 666666666668L;
		}

		public static bool Int64Test9()
		{
			ulong var1 = 666666666663uL;
			ulong var2 = 5uL;
			return (var1 - var2) == 666666666658uL;
		}

		public static bool Int64Test10()
		{
			long var1 = 666666666663L;
			long var2 = 5L;
			return (var1 - var2) == 666666666658L;
		}
	}
}