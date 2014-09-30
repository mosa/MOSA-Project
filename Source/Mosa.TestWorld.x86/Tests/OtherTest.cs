/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Collections.Generic;

namespace Mosa.TestWorld.x86.Tests
{
	public class OtherTest : KernelTest
	{
		public OtherTest()
			: base("Other")
		{
			testMethods.Add(OtherTest1);
			testMethods.Add(OtherTest2);
			testMethods.Add(OtherTest3);
			testMethods.Add(ForeachNestedTest);
		}

		private static uint StaticValue = 0x200000;

		public static bool OtherTest1()
		{
			uint x = StaticValue;

			return x == 0x200000;
		}

		public static bool OtherTest2()
		{
			return StaticValue == 0x200000;
		}

		public static bool OtherTest3()
		{
			return 3.Equals(3);
		}

		public static bool ForeachNestedTest()
		{
			var list = Populate();
			var nestedList = Populate();
			int sum = 0;
			int nestedSum = 0;
			int nestedCount = 0;

			foreach (var item in list)
			{
				sum = sum + item;
				foreach (var nestedItem in nestedList)
				{
					nestedSum = nestedSum + nestedItem;
					nestedSum = nestedSum / ++nestedCount;
				}
			}

			return (sum + nestedSum) == 4556;
		}

		private static LinkedList<int> Populate()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			for (int i = 1; i < 10; i++)
			{
				IntList.Add(101 * i);
			}

			return IntList;
		}
	}

	public struct TestStruct
	{
		public byte One;
	}
}