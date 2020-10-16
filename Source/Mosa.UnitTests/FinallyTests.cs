// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests
{
	public static class FinallyTests
	{
		[MosaUnitTest]
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

		[MosaUnitTest]
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

		[MosaUnitTest]
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

		[MosaUnitTest]
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

		[MosaUnitTest]
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

		[MosaUnitTest]
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

		[MosaUnitTest]
		public static int TryFinally7()
		{
			// This doesn't actually generate a try/finally
			// but it uses the Leave instruction outside of protected regions
			// so we must test that the compiler can handle this scenario
			return TryFinally7Part();
		}

		[MosaUnitTest]
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

		[MosaUnitTest(Series = "I4I4")]
		public static int TryFinally8(int a, int b)
		{
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
	}
}
