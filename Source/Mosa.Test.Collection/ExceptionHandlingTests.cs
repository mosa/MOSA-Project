/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Test.Collection
{
	public static class ExceptionHandlingTests
	{
		public static int TryFinally1()
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

			return a;
		}

		public static int TryFinally2()
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

			return c;
		}

		public static int TryFinally3()
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

			return a;
		}

		public static int TryFinally4()
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

			return a;
		}

		public static int TryFinally5()
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

			return a;
		}

		public static int TryFinally6()
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

			return a;
		}

		public static int TryFinally7()
		{
			// This doesn't actually generate a try/finally
			// but it uses the Leave instruction outside of protected regions
			// so we must test that the compiler can handle this scenario
			return TryFinally7Part();
		}

		private static int TryFinally7Part()
		{
			int[] b = new int[10];
			for (int i = 0; i < b.Length; i++)
			{
				b[i] = 100 * i;
			}

			foreach (var j in b)
			{
				if (j == 900)
					return j;
			}
			return 0;
		}

		public static int ExceptionTest1()
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

			return a;
		}

		public static int ExceptionTest2()
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
			finally
			{
				a = a + 2000;
			}

			a = a + 7;

			return a;
		}

		public static int ExceptionTest3()
		{
			int a = 10;

			try
			{
				try
				{
					a = a + 2;
				}
				catch
				{
					a = a + 50;
				}
				finally
				{
					a = a + 2000;
					throw new Exception();
				}
			}
			catch
			{
				a = a + 51;
			}
			finally
			{
				a = a + 5000;
			}

			a = a + 7;

			return a;
		}

		public static int ExceptionTest4()
		{
			int a = 10;

			try
			{
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
				finally
				{
					a = a + 2000;
					throw new Exception();
				}
			}
			catch
			{
				a = a + 51;
			}
			finally
			{
				a = a + 5000;
			}

			a = a + 7;

			return a;
		}

		public static int ExceptionTest5()
		{
			int a = 10;

			try
			{
				int b = ExceptionTest5Exception();
				a = a + b;
			}
			catch
			{
				a = a + 20;
			}

			return a;
		}

		private static int ExceptionTest5Exception()
		{
			throw new Exception();
		}

		public static int ExceptionTest6()
		{
			int a = 10;

			try
			{
				int b = ExceptionTest6Exception();
				a = a + b;
			}
			catch
			{
				a = a + 20;
			}
			finally
			{
				a = a + 40;
			}

			return a;
		}

		private static int ExceptionTest6Exception()
		{
			try
			{
				throw new Exception();
			}
			catch
			{
				throw new Exception();
			}
		}

		public static int ExceptionTest7()
		{
			int a = 10;

			try
			{
				int b = ExceptionTest7Thru();
				a = a + b;
			}
			catch
			{
				a = a + 20;
			}
			finally
			{
				a = a + 40;
			}

			return a;
		}

		private static int ExceptionTest7Thru()
		{
			return ExceptionTest7Exception();
		}

		private static int ExceptionTest7Exception()
		{
			try
			{
				throw new Exception();
			}
			catch
			{
				throw new Exception();
			}
		}

		public static int ExceptionTest8()
		{
			int a = 10;

			try
			{
				int b = ExceptionTest8Finally();
				a = a + b;
			}
			catch
			{
				a = a + 20;
			}
			finally
			{
				a = a + 40;
			}

			return a;
		}

		private static int ExceptionTest8Finally()
		{
			int a = 0;

			try
			{
				a = ExceptionTest8Exception();
			}
			finally
			{
				a = a + 90;
			}

			return a;
		}

		private static int ExceptionTest8Exception()
		{
			try
			{
				throw new Exception();
			}
			catch
			{
				throw new Exception();
			}
		}

		public static ulong ExceptionTest9()
		{
			ulong n = 1;

			try
			{
				try
				{
					n = n + 10;
				}
				catch
				{
					n = n + 100;
				}
				finally
				{
					n = n + 1000;
				}

				n = n + 10000;

				try
				{
					n = n + 100000;
					ExceptionTest0Exception();
				}
				catch
				{
					try
					{
						n = n + 1000000;
						ExceptionTest0Exception();
						n = n * 23;
					}
					finally
					{
						n = n + 10000000;
					}
				}
				finally
				{
					n = n + 100000000;
					ExceptionTest0Exception();
				}
			}
			catch
			{
				n = n + 1000000000;
			}
			finally
			{
				n = n + 10000000000;
			}

			return n;
		}

		static void ExceptionTest0Exception()
		{
			throw new Exception();
		}
	}
}
