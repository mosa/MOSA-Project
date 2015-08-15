// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Test.Collection.x86.xUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class TraceListener : System.Diagnostics.TraceListener
	{
		private List<string> output = new List<string>();

		public string Output
		{
			get
			{
				var builder = new StringBuilder();
				foreach (var m in output)
					builder.Append(m);
				return builder.ToString();
			}
		}

		public override void Write(string message)
		{
			output.Add(message);
		}

		public override void WriteLine(string message)
		{
			output.Add(message);
			output.Add("\r\n");
		}
	}

	internal class Program
	{
		private static void Main(string[] args)
		{
			var listener = new TraceListener();
			try
			{
				System.Diagnostics.Debug.Listeners.Add(listener);

				//Test5a();
				//Test5b();
				//Test5c();
				//Test3();
				//Test4();
				//Test12();

				//Test13();

				Test14();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			System.Diagnostics.Debugger.Launch();
			System.Diagnostics.Debug.Write(listener.Output);

			//Console.ReadLine();
		}

		private static void Test14()
		{
			var fixture = new BoxingFixture();

			fixture.BoxI4(10);
		}

		private static void Test13()
		{
			var fixture = new _ExceptionHandlingFixture();

			fixture.ExceptionTest8();
		}

		private static void Test12()
		{
			var fixture = new BooleanFixture();

			fixture.LdelemaB(10, true);
		}

		private static void Test11()
		{
			var fixture = new FibonacciFixture();

			fixture.Fibonacci(3);
		}

		private static void Test10()
		{
			var fixture = new DelegateFixture();

			fixture.CallDelegateVoid1();
		}

		private static void Test9()
		{
			var fixture = new ArrayLayoutFixture();

			fixture.ArrayR4();
		}

		private static void Test8()
		{
			var fixture = new DoubleFixture();

			fixture.IsNaN(Double.NaN);
		}

		private static void Test7()
		{
			var fixture = new DoubleFixture();

			fixture.CeqR8R8(200, 100);
		}

		private static void Test6()
		{
			var fixture = new DoubleFixture();

			fixture.AddR8R8(200, 100);
		}

		private static void Test5a()
		{
			var fixture = new UInt32Fixture();

			fixture.AddU4U4(1, 2);
		}

		private static void Test5b()
		{
			var fixture = new UInt16Fixture();

			fixture.AddU2U2(1, 2);
		}

		private static void Test5c()
		{
			var fixture = new UInt8Fixture();

			fixture.AddU1U1(1, 2);
		}

		private static void Test4()
		{
			var fixture = new Int16Fixture();

			fixture.LdelemI2(0, -32768);
		}

		private static void Test3()
		{
			var fixture = new UInt64Fixture();

			fixture.DivU8U8(18446744073709551615, 4294967294);
		}

		private static void Test1()
		{
			var test = new TestCPUx86();

			test.RunTest();
		}
	}
}
