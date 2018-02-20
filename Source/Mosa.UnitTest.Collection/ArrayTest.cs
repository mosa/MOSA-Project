// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTest.Collection
{
	public static class ArrayTest
	{
		public static bool BoundsCheck()
		{
			var myArray = new int[1];
			try
			{
				myArray[1] = 20;
				return false;
			}
			catch (IndexOutOfRangeException ex)
			{
				return true;
			}
		}

		//public static bool MultiDimensionalArray2D()
		//{
		//	var d = new int[5, 5];

		//	return true;
		//}

		//public static bool MultiDimensionalArray3D()
		//{
		//	var d = new int[5, 5, 5];

		//	return true;
		//}
	}
}
