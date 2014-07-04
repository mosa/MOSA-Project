/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class SSAOptimizationFixture : X86TestFixture
	{
		[Fact]
		public void OptimizationTest1()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest1(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest1"));
		}

		[Fact]
		public void OptimizationTest2()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest2(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest2"));
		}

		[Fact]
		public void OptimizationTest3()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest3(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest3"));
		}

		[Fact]
		public void OptimizationTest4()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest4(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest4"));
		}

		[Fact]
		public void OptimizationTest5()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest5(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest5"));
		}

		[Fact]
		public void OptimizationTest6()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest6(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest6"));
		}

		[Fact]
		public void OptimizationTest7()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest7(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest7"));
		}

		[Fact]
		public void OptimizationTest8()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest8(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest8"));
		}

		[Fact]
		public void OptimizationTest9()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest9(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest9"));
		}

		[Fact]
		public void OptimizationTest10()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest10(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest10"));
		}

		[Fact]
		public void OptimizationTest11()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest11(), Run<bool>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest11"));
		}

		[Fact]
		public void OptimizationTest12()
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest12(), Run<int>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest12"));
		}

		[Theory]
		[PropertyData("I4")]
		public void OptimizationTest13(int a)
		{
			Assert.Equal(Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest13(a), Run<int>("Mosa.Test.Collection.SSAOptimizationTest.OptimizationTest13", a));
		}
	}
}