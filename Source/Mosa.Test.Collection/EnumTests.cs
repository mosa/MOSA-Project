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
	public enum TestEnum
	{
		ItemA = 5,
		ItemB
	}

	public static class TestEnumClass
	{
		public static bool AMustBe5()
		{
			return 5 == (int)TestEnum.ItemA;
		}
	}
}