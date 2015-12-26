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
			var fixture = new StringFixture();

			fixture.TestCompiler.DebugOutput = false;
			DoTest(fixture.NotEqual1, "normal-all");

			//fixture.TestCompiler.Reset();
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableOptimizations = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableSSA = false;
			//fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			//DoTest(fixture.ExceptionTest8, "no: optimizations, inline, sparse, ssa, variable promotion");

			fixture.TestCompiler.Reset();
			fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			DoTest(fixture.NotEqual1, "no inline");

			fixture.TestCompiler.Reset();
			fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			DoTest(fixture.NotEqual1, "no promotion");

			fixture.TestCompiler.Reset();
			fixture.TestCompiler.Compiler.CompilerOptions.EnableVariablePromotion = false;
			fixture.TestCompiler.Compiler.CompilerOptions.EnableInlinedMethods = false;
			DoTest(fixture.NotEqual1, "no promotion, inline");

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
	}
}
