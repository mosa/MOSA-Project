// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.TestWorld.x86.Tests
{
	public class ArrayTest : KernelTest
	{
		public ArrayTest()
			: base("Array")
		{
			testMethods.Add(GenericInterfaceTest);
			testMethods.Add(ArrayBoundsCheck);

			// TODO: add more tests
		}

		public static bool GenericInterfaceTest()
		{
			int[] list = new int[] { 1, 3, 5 };
			IList<int> iList = list;

			int result = 0;
			foreach (var i in iList)
				result += i;
			return result == 9;
		}

		public static bool GenericInterfaceTest2()
		{
			return GenericInterfaceTest2a() == 9;
		}

		private static int GenericInterfaceTest2a()
		{
			int[] list = new int[] { 1, 3, 5 };
			IList<int> iList = list;

			int result = 0;
			foreach (var i in iList)
				result += i;

			return result;
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
