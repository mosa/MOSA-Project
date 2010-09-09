using Mosa.Platforms.x86;
using Mosa.Kernel;
using Mosa.Kernel.X86;
using System;

namespace Mosa.HelloWorld.Tests
{
	public static class InterfaceTest
	{
		public static void Test()
		{
			Screen.SetCursor(23, 22);
			Screen.Write("Interface Test: ");

			PrintResult(InterfaceTest1());
			PrintResult(InterfaceTest2());
			PrintResult(InterfaceTest3());
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
			InterfaceB b = tc;
			bool result =  (b.B() == 3);
			return result;
		}

		public static bool InterfaceTest3()
		{
			TestClass tc = new TestClass();
			InterfaceB b = tc;
			bool result = (b.A() == 2);
			return result;
		}

		public static void PrintResult(bool flag)
		{
			byte color = Screen.Color;
			if (flag)
			{
				Screen.Color = Colors.Green;
				Screen.Write("+");
			}
			else
			{
				Screen.Color = Colors.Red;
				Screen.Write("X");
			}
			Screen.Color = color;
		}
	}

	public interface InterfaceA
	{
		int A();
	}

	public interface InterfaceB
	{
		int A();
		int B();
	}

	public class TestClass : InterfaceA, InterfaceB
	{
		public int A()
		{
			return 1;
		}

		int InterfaceB.A()
		{
			return 2;
		}

		public int B()
		{
			return 3;
		}
		
	}
}
