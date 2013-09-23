/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Test.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	
	public class Int32Fixture
	{

		private TestCompiler SetupCompiler()
		{
			var testCompiler = new TestCompiler(new X86Platform());
			testCompiler.EnableSSA = true;
			testCompiler.EnableSSAOptimizations = true;

			return testCompiler;
		}

		[Theory]
		[InlineData((int)0, (int)0)]
		[InlineData((int)1, (int)1)]
		public void AddI4I4(int a, int b)
		{
			var testCompiler = SetupCompiler();

			Assert.Equal(Int32Tests.AddI4I4(a, b), testCompiler.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", a, b));
		}
	}
}