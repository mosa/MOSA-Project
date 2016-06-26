// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class InterfaceFixture : TestFixture
	{
		[Fact]
		public void InterfaceTest1()
		{
			Assert.Equal(InterfaceTests.InterfaceTest1(), Run<int>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest1"));
		}

		[Fact]
		public void InterfaceTest2()
		{
			Assert.Equal(InterfaceTests.InterfaceTest2(), Run<int>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest2"));
		}

		[Fact]
		public void InterfaceTest3()
		{
			Assert.Equal(InterfaceTests.InterfaceTest3(), Run<int>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest3"));
		}

		[Fact]
		public void InterfaceTest4()
		{
			Assert.Equal(InterfaceTests.InterfaceTest4(), Run<int>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest4"));
		}

		[Fact]
		public void InterfaceTest5()
		{
			Assert.Equal(InterfaceTests.InterfaceTest5(), Run<bool>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest5"));
		}

		[Fact]
		public void InterfaceTest6()
		{
			Assert.Equal(InterfaceTests.InterfaceTest6(), Run<bool>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest6"));
		}

		[Fact]
		public void InterfaceTest7()
		{
			Assert.Equal(InterfaceTests.InterfaceTest7(), Run<bool>("Mosa.UnitTest.Collection.InterfaceTests.InterfaceTest7"));
		}
	}
}
