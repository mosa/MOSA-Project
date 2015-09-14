// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class OptimizationFixture : X86TestFixture
	{
		[Fact]
		public void OptimizationTest1()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest1(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest1"));
		}

		[Fact]
		public void OptimizationTest2()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest2(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest2"));
		}

		[Fact]
		public void OptimizationTest3()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest3(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest3"));
		}

		[Fact]
		public void OptimizationTest4()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest4(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest4"));
		}

		[Fact]
		public void OptimizationTest5()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest5(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest5"));
		}

		[Fact]
		public void OptimizationTest6()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest6(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest6"));
		}

		[Fact]
		public void OptimizationTest7()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest7(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest7"));
		}

		[Fact]
		public void OptimizationTest8()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest8(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest8"));
		}

		[Fact]
		public void OptimizationTest9()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest9(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest9"));
		}

		[Fact]
		public void OptimizationTest10()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest10(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest10"));
		}

		[Fact]
		public void OptimizationTest11()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest11(), Run<bool>("Mosa.Test.Collection.OptimizationTest.OptimizationTest11"));
		}

		[Fact]
		public void OptimizationTest12()
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest12(), Run<int>("Mosa.Test.Collection.OptimizationTest.OptimizationTest12"));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void OptimizationTest13(int a)
		{
			Assert.Equal(Mosa.Test.Collection.OptimizationTest.OptimizationTest13(a), Run<int>("Mosa.Test.Collection.OptimizationTest.OptimizationTest13", a));
		}
	}
}
