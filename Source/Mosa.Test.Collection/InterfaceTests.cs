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
	}

}
