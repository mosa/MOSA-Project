// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Test.Collection.x86.xUnit;
using System;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class Program
	{
		private delegate void Test();

		private static void Main(string[] args)
		{
			var fixture = new ForeachFixture();

			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			//fixture.TestCompiler.DebugOutput = true;
			//DoTest(fixture.ForeachU1, "no: variable promotion");

			//fixture.TestCompiler.Reset();
			fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = true;
			fixture.TestCompiler.DebugOutput = true;
			DoTest(fixture.ForeachU1, "normal-all");

			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSSA = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			//DoTest(fixture.ForeachU1, "no: optimizations, inline, sparse, ssa, variable promotion");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//DoTest(fixture.ForeachU1, "no optimization");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//DoTest(fixture.ForeachU1, "no: optimizations, inline");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = false;
			//DoTest(fixture.ForeachU1, "no: optimizations, inline, sparse");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSSA = false;
			//DoTest(fixture.ForeachU1, "no: optimizations, inline, sparse, ssa");

			return;
		}

		private static bool DoTest(Test test, string note = null)
		{
			try
			{
				Console.WriteLine("Testing: " + test.Method.ToString());
				if (note != null)
				{
					Console.WriteLine("Note: " + note);
				}

				test.Invoke();

				Console.WriteLine("=> OK");
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("=> ERROR");
				return false;
			}
		}

		private static void Test14()
		{
			var fixture = new BoxingFixture();

			fixture.BoxI4(10);
		}

		//private static void Test13()
		//{
		//	var fixture = new _ExceptionHandlingFixture();

		//	fixture.ExceptionTest8();
		//}

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

			fixture.XorU4U4(65535, 1);
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
