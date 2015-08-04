// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class LinkedListFixture : X86TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.Create(), Run<bool>("Mosa.Test.Collection.LinkedListTests.Create"));
		}

		[Fact]
		public void EmptySize()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.EmptySize(), Run<bool>("Mosa.Test.Collection.LinkedListTests.EmptySize"));
		}

		[Fact]
		public void Size1()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.Size1(), Run<bool>("Mosa.Test.Collection.LinkedListTests.Size1"));
		}

		[Fact]
		public void First1()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.First1(), Run<bool>("Mosa.Test.Collection.LinkedListTests.First1"));
		}

		[Fact]
		public void First2()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.First2(), Run<bool>("Mosa.Test.Collection.LinkedListTests.First2"));
		}

		[Fact]
		public void Last1()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.Last1(), Run<bool>("Mosa.Test.Collection.LinkedListTests.Last1"));
		}

		[Fact]
		public void Last2()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.Last2(), Run<bool>("Mosa.Test.Collection.LinkedListTests.Last2"));
		}

		[Fact]
		public void PopulateList()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.PopulateList(), Run<bool>("Mosa.Test.Collection.LinkedListTests.PopulateList"));
		}

		[Fact]
		public void Foreach()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.Foreach(), Run<int>("Mosa.Test.Collection.LinkedListTests.Foreach"));
		}

		[Fact]
		public void ForeachNested()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.ForeachNested(), Run<int>("Mosa.Test.Collection.LinkedListTests.ForeachNested"));
		}

		[Fact]
		public void ForeachBreak()
		{
			Assert.Equal(Mosa.Test.Collection.LinkedListTests.ForeachBreak(), Run<int>("Mosa.Test.Collection.LinkedListTests.ForeachBreak"));
		}
	}
}
