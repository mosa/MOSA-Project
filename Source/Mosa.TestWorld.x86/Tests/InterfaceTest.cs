// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TestWorld.x86.Tests
{
	public class InterfaceTest : KernelTest
	{
		public InterfaceTest()
			: base("Interface")
		{
			testMethods.AddLast(InterfaceTest1);
			testMethods.AddLast(InterfaceTest2);
			testMethods.AddLast(InterfaceTest3);
			testMethods.AddLast(InterfaceTest4);
		}

		public static bool InterfaceTest1()
		{
			TestClass tc = new TestClass();
			bool result = tc.B() == 3;
			return result;
		}

		public static bool InterfaceTest2()
		{
			TestClass tc = new TestClass();
			IInterfaceAB b = tc;
			bool result = (b.B() == 3);
			return result;
		}

		public static bool InterfaceTest3()
		{
			TestClass tc = new TestClass();
			IInterfaceAB b = tc;
			bool result = (b.A() == 2);
			return result;
		}

		public static bool InterfaceTest4()
		{
			TestClassB tc = new TestClassB();
			IInterfaceA a = tc;
			bool result = (a.A() == 1);
			return result;
		}
	}

	public interface IInterfaceA
	{
		int A();
	}

	public interface IInterfaceAB
	{
		int A();

		int B();
	}

	public class TestClass : IInterfaceA, IInterfaceAB
	{
		public int A()
		{
			return 1;
		}

		int IInterfaceAB.A()
		{
			return 2;
		}

		public int B()
		{
			return 3;
		}
	}

	public class TestClassA : IInterfaceA
	{
		public int A()
		{
			return 1;
		}
	}

	public class TestClassB : TestClassA, IInterfaceAB
	{
		int IInterfaceAB.A()
		{
			return 2;
		}

		public int B()
		{
			return 3;
		}
	}
}
