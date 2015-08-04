// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.TestWorld.x86.Tests
{
	public class ExceptionTest : KernelTest
	{
		public ExceptionTest()
			: base("Exception")
		{
			testMethods.AddLast(ExceptionTest1);
			testMethods.AddLast(ExceptionTest2);
			testMethods.AddLast(ExceptionTest3);
			testMethods.AddLast(ExceptionTest4);
			testMethods.AddLast(ExceptionTest5);
			testMethods.AddLast(ExceptionTest6);
			testMethods.AddLast(ExceptionTest7);
			testMethods.AddLast(ExceptionTest8);
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

		public static bool ExceptionTest5()
		{
			int a = 10;

			try
			{
				a = a + 2;

				//throw new System.Exception();
			}
			catch
			{
				a = a + 50;
			}

			a = a + 7;

			return (a == 19);
		}

		public static bool ExceptionTest6()
		{
			int a = 10;

			try
			{
				a = a + 20;
			}
			catch
			{
				a = a + 30;
			}
			finally
			{
				a = a + 40;
			}

			a = a + 50;

			return (a == 120);
		}

		public static bool ExceptionTest7()
		{
			int a = 10;

			try
			{
				a = a + 15;
				try
				{
					a = a + 20;
				}
				catch
				{
					try
					{
						a = a + 30;
					}
					catch
					{
						a = a + 40;
					}
					a = a + 50;
				}
				a = a + 55;
			}
			catch
			{
				a = a + 40;
			}

			a = a + 60;

			return (a == 160);
		}

		public static bool ExceptionTest8()
		{
			int a = 10;

			try
			{
				a = a + 2;

				if (a > 0)
					throw new Exception();

				a = a + 1000;
			}
			catch
			{
				a = a + 50;
			}

			a = a + 7;

			return (a == 69);
		}
	}
}
