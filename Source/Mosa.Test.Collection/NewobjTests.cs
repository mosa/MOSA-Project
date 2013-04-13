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
	public class TestNewobjBaseClass
	{
		public int Test()
		{
			return 5;
		}
	}

	public class TestNewobjDerivedClass : TestNewobjBaseClass
	{
		private readonly int int32;

		public TestNewobjDerivedClass()
		{
			this.int32 = 0;
		}

		public TestNewobjDerivedClass(int value)
		{
			this.int32 = value;
		}

		public TestNewobjDerivedClass(int v1, int v2)
		{
			this.int32 = v1 + v2;
		}

		public TestNewobjDerivedClass(int v1, int v2, int v3)
		{
			this.int32 = (v1 * v2) + v3;
		}

		public static bool NewobjWithoutArgs()
		{
			TestNewobjDerivedClass d = new TestNewobjDerivedClass();
			return d != null;
		}

		public static bool NewobjTestWithOneArg()
		{
			TestNewobjDerivedClass d = new TestNewobjDerivedClass(42);
			return (d.int32 == 42);
		}

		public static bool NewobjTestWithTwoArgs()
		{
			TestNewobjDerivedClass d = new TestNewobjDerivedClass(42, 3);
			return (d.int32 == 45);
		}

		public static bool NewobjTestWithThreeArgs()
		{
			TestNewobjDerivedClass d = new TestNewobjDerivedClass(21, 2, 7);
			return (d.int32 == 49);
		}
	}
}