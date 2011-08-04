using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using System;

namespace Mosa.HelloWorld.Tests
{
	public class ExceptionTest : KernelTest
	{
		public static void Test()
		{
			Screen.Color = Colors.Gray;
			Screen.Write(" EX:");

			PrintResult(ExceptionTest1());
			PrintResult(ExceptionTest2());
			PrintResult(ExceptionTest3());
		}

		public static bool ExceptionTest1()
		{
			int a = 10;
			try
			{
				a = a + 1;
			}
			finally
			{
				a = a + 3;
			}

			a = a + 7;

			return (a == 21);
		}

		public static bool ExceptionTest2()
		{
			int a = 10;
			int b = 13;
			try
			{
				a = a + 1;
			}
			finally
			{
				b = b + a;
			}

			b = b + 3;
			a = a + 3;

			int c = b + a;

			return (c == 41);
		}


		public static bool ExceptionTest3()
		{
			int a = 10;
			try
			{
				try
				{
					a = a + 1;
				}
				finally
				{
					a = a + 100;
				}
			}
			finally
			{
				a = a + 3;
			}

			a = a + 7;

			return (a == 121);
		}
	}
}
