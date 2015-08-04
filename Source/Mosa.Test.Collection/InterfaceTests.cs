// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public interface IInterfaceA
	{
		int A();
	}

	public interface IInterfaceAB
	{
		int A();

		int B();
	}

	public class InterfaceTestClass : IInterfaceA, IInterfaceAB
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

	public class InterfaceTestClassA : IInterfaceA
	{
		public int A()
		{
			return 1;
		}
	}

	public class InterfaceTestClassB : InterfaceTestClassA, IInterfaceAB
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

	public static class InterfaceTests
	{
		public static int InterfaceTest1()
		{
			InterfaceTestClass tc = new InterfaceTestClass();
			return tc.B();
		}

		public static int InterfaceTest2()
		{
			InterfaceTestClass tc = new InterfaceTestClass();
			IInterfaceAB b = tc;
			return b.B();
		}

		public static int InterfaceTest3()
		{
			InterfaceTestClass tc = new InterfaceTestClass();
			IInterfaceAB b = tc;
			return b.A();
		}

		public static int InterfaceTest4()
		{
			InterfaceTestClassB tc = new InterfaceTestClassB();
			IInterfaceA a = tc;
			return a.A();
		}
	}
}
