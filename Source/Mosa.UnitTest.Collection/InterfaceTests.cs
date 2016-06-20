// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
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

	public class InterfaceTestClassY : IInterfaceA
	{
		int IInterfaceA.A()
		{
			return 2;
		}
	}

	public class InterfaceTestClassZ : InterfaceTestClassY
	{
	}

	public abstract class InterfaceTestClassY2 : IInterfaceA
	{
		int IInterfaceA.A()
		{
			return 2;
		}
	}

	public class InterfaceTestClassZ2 : InterfaceTestClassY2
	{
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

		public static bool InterfaceTest5()
		{
			InterfaceTestClassY y = new InterfaceTestClassY();

			return (y as IInterfaceA) == null;
		}

		public static bool InterfaceTest6()
		{
			InterfaceTestClassZ z = new InterfaceTestClassZ();

			return (z as IInterfaceA) == null;
		}

		public static bool InterfaceTest7()
		{
			InterfaceTestClassZ2 z = new InterfaceTestClassZ2();

			return (z as IInterfaceA) == null;
		}
	}
}
