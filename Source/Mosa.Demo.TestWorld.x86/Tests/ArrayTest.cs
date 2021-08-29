// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Demo.TestWorld.x86.Tests
{
	public class ArrayTest : KernelTest
	{
		public ArrayTest()
			: base("Array")
		{
			testMethods.Add(ArrayTest1);
			testMethods.Add(ArrayTest2);
			testMethods.Add(ArrayTest3);
			testMethods.Add(ArrayIListTest2);
			testMethods.Add(ArrayBoundsCheck);

			// TODO: add more tests
		}

		public static bool ArrayTest1()
		{
			int[] list = new int[3];
			list[0] = 1;
			list[1] = 3;
			list[2] = 5;

			int result = 0;

			for (int i = 0; i < 3; i++)
				result += list[i];

			return result == 9;
		}

		public static bool ArrayTest2()
		{
			int[] list = new int[3];
			list[0] = 1;
			list[1] = 3;
			list[2] = 5;

			int result = 0;
			foreach (var i in list)
				result += i;

			return result == 9;
		}

		public static bool ArrayTest3()
		{
			int[] list = new int[] { 1, 3, 5 };

			int result = 0;
			foreach (var i in list)
				result += i;

			return result == 9;
		}

		public static bool ArrayIListTest2()
		{
			int[] list = new int[] { 1, 3, 5 };
			IList<int> iList = list;

			int result = 0;
			foreach (var i in iList)
				result += i;
			return result == 9;
		}

		public static bool ArrayBoundsCheck()
		{
			int[] myArray = new int[1];
			try
			{
				myArray[1] = 20;
				return false;
			}
			catch
			{
				return true;
			}
		}
	}
}
