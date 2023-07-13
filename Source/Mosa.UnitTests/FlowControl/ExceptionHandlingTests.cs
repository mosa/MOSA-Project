// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.FlowControl;

public static class _ExceptionHandlingTests
{
	[MosaUnitTest]
	public static int ExceptionTest0()
	{
		var a = 10;

		try
		{
			a = a + 10;
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

	[MosaUnitTest]
	public static int ExceptionTest1()
	{
		var a = 10;

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

	[MosaUnitTest]
	public static int ExceptionTest2()
	{
		var a = 10;

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

	[MosaUnitTest]
	public static int ExceptionTest3()
	{
		var a = 10;

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

	[MosaUnitTest]
	public static int ExceptionTest4()
	{
		var a = 10;

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

	[MosaUnitTest]
	public static int ExceptionTest5()
	{
		var a = 10;

		try
		{
			var b = ExceptionTest5Exception();
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

	[MosaUnitTest]
	public static int ExceptionTest6()
	{
		var a = 10;

		try
		{
			var b = ExceptionTest6Exception();
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

	[MosaUnitTest]
	public static int ExceptionTest7()
	{
		var a = 10;

		try
		{
			var b = ExceptionTest7Thru();
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

	[MosaUnitTest]
	public static int ExceptionTest8()
	{
		var a = 10;

		try
		{
			var b = ExceptionTest8Finally();
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
		var a = 0;

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

	[MosaUnitTest]
	public static ulong ExceptionTest9()
	{
		ulong n = 1;

		try
		{
			n = n + 20;

			try
			{
				n = n + 300;
				ExceptionTest9Exception();
			}
			finally
			{
				n = n + 4000;
			}
		}
		catch (SystemException ex)
		{
			// There should never be a 50000
			n = n + 50000;
		}
		catch (TestExceptionType ex)
		{
			n = n + 600000;
		}
		finally
		{
			n = n + 7000000;
		}

		return n;
	}

	private static void ExceptionTest9Exception()
	{
		throw new TestExceptionType();
	}

	private class TestExceptionType : Exception
	{
	}

	[MosaUnitTest]
	public static ulong ExceptionTest10()
	{
		ulong n = 1;

		try
		{
			n = n + 20;

			try
			{
				n = n + 300;
				ExceptionTest10Exception();
			}
			finally
			{
				n = n + 4000;
			}
		}
		catch
		{
			n = n + 50000;
		}
		finally
		{
			n = n + 600000;
		}

		return n;
	}

	private static void ExceptionTest10Exception()
	{
		throw new Exception();
	}

	[MosaUnitTest]
	public static int ExceptionTest11()
	{
		var a = 10;

		try
		{
			a = a + 2;

			throw new Exception();
		}
		catch
		{
			a = a + 50;
		}

		a = a + 7;

		return a;
	}

	[MosaUnitTest]
	public static int ExceptionTest12()
	{
		var a = 10;

		try
		{
			a = a + 2;

			throw new Exception();
		}
		catch
		{
			a = a + 50;
		}
		finally
		{
			a = a + 25;
		}

		a = a + 7;

		return a;
	}

	[MosaUnitTest]
	public static int ExceptionTest13()
	{
		try
		{
			var a = ExceptionTest13b();
		}
		catch
		{
			return 123;
		}

		return 321;
	}

	private static int ExceptionTest13b()
	{
		var a = 10;

		try
		{
			a = a + 2;

			throw new Exception();
		}
		finally
		{
			a = a + 25;
		}

#pragma warning disable CS0162 // Unreachable code detected
		a = a + 7;
#pragma warning restore CS0162 // Unreachable code detected

		return a;
	}

	//public static int _ExceptionTest20()
	//{
	//	bool DoCatch = true;
	//	int counter = 1;
	//	try
	//	{
	//		try
	//		{
	//			throw new System.Exception();
	//		}
	//		finally
	//		{
	//			counter = counter * 10;
	//			DoCatch = false;
	//		}
	//	}
	//	catch when (DoCatch)
	//	{
	//		counter = counter + 1000;
	//	}

	//	return counter;
	//}
}
