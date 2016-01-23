// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Test.Collection
{
	public class ArrayTest
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
	}
}
