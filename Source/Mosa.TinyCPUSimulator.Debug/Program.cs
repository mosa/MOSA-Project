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
			var fixture = new ExceptionHandlingFixture();

			fixture.TestCompiler.DebugOutput = true;

			fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = true;
			DoTest(fixture.ExceptionTest2, "normal-all");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//DoTest(fixture.NotEqual1, "no optimizations, inline, promotion");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = true;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//DoTest(fixture.NotEqual1, "no optimizations, inline");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = true;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = true;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//DoTest(fixture.NotEqual1, "no optimizations");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = true;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = true;
			//DoTest(fixture.NotEqual1, "no promotion");
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
	}
}
