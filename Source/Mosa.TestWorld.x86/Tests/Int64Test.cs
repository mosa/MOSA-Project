// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TestWorld.x86.Tests
{
	public class Int64Test : KernelTest
	{
		public Int64Test()
			: base("Int64")
		{
			testMethods.AddLast(Int64Test1);
			testMethods.AddLast(Int64Test2);
			testMethods.AddLast(Int64Test3);
			testMethods.AddLast(Int64Test4);
			testMethods.AddLast(Int64Test5);
			testMethods.AddLast(Int64Test6);
			testMethods.AddLast(Int64Test7);
			testMethods.AddLast(Int64Test8);
			testMethods.AddLast(Int64Test9);
			testMethods.AddLast(Int64Test10);
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