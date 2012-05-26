/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.HelloWorld.x86.Tests
{
	public class ExceptionTest : KernelTest
	{
		public ExceptionTest()
			: base("EX")
		{
			testMethods.Add(ExceptionTest1);
			testMethods.Add(ExceptionTest2);
			testMethods.Add(ExceptionTest3);
			testMethods.Add(ExceptionTest4);
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

		public static bool ExceptionTest4()
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

			return (a == 232);
		}

		//public static bool ExceptionTest5()
		//{
		//    int a = 10;

		//    try
		//    {
		//        a = a + 2;
		//        throw new System.Exception();
		//    }
		//    catch
		//    {
		//        a = a + 50;
		//    }

		//    a = a + 7;

		//    return (a == 69);
		//}
	}
}
