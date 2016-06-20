// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class LinkedListFixture : TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.Create(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.Create"));
		}

		[Fact]
		public void EmptySize()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.EmptySize(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.EmptySize"));
		}

		[Fact]
		public void Size1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.Size1(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.Size1"));
		}

		[Fact]
		public void First1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.First1(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.First1"));
		}

		[Fact]
		public void First2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.First2(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.First2"));
		}

		[Fact]
		public void Last1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.Last1(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.Last1"));
		}

		[Fact]
		public void Last2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.Last2(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.Last2"));
		}

		[Fact]
		public void PopulateList()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.PopulateList(), Run<bool>("Mosa.UnitTest.Collection.LinkedListTests.PopulateList"));
		}

		[Fact]
		public void Foreach()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.Foreach(), Run<int>("Mosa.UnitTest.Collection.LinkedListTests.Foreach"));
		}

		[Fact]
		public void ForeachNested()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.ForeachNested(), Run<int>("Mosa.UnitTest.Collection.LinkedListTests.ForeachNested"));
		}

		[Fact]
		public void ForeachBreak()
		{
			Assert.Equal(Mosa.UnitTest.Collection.LinkedListTests.ForeachBreak(), Run<int>("Mosa.UnitTest.Collection.LinkedListTests.ForeachBreak"));
		}
	}
}
