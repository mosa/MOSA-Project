// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class NullCheckOptimizationTests
	{
		public static bool NullTest1()
		{
			var o = new object();

			if (o == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public static bool NullTest2()
		{
			var o = new object();

			if (o != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
