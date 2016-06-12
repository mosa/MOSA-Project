// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class ListFixture : TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Create(), Run<bool>("Mosa.Test.Collection.ListTests.Create"));
		}

		[Fact]
		public void EmptySize()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.EmptySize(), Run<bool>("Mosa.Test.Collection.ListTests.EmptySize"));
		}

		[Fact]
		public void Add1()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Add1(), Run<bool>("Mosa.Test.Collection.ListTests.Add1"));
		}

		[Fact]
		public void Add2()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Add2(), Run<bool>("Mosa.Test.Collection.ListTests.Add2"));
		}

		[Fact]
		public void Index1()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Index1(), Run<bool>("Mosa.Test.Collection.ListTests.Index1"));
		}

		[Fact]
		public void Index2()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Index2(), Run<bool>("Mosa.Test.Collection.ListTests.Index2"));
		}

		[Fact]
		public void IndexOf1()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.IndexOf1(), Run<bool>("Mosa.Test.Collection.ListTests.IndexOf1"));
		}

		[Fact]
		public void IndexOf2()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.IndexOf2(), Run<bool>("Mosa.Test.Collection.ListTests.IndexOf2"));
		}

		[Fact]
		public void Remove1()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Remove1(), Run<bool>("Mosa.Test.Collection.ListTests.Remove1"));
		}

		[Fact]
		public void Remove2()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Remove2(), Run<bool>("Mosa.Test.Collection.ListTests.Remove2"));
		}

		[Fact]
		public void PopulateList()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.PopulateList(), Run<bool>("Mosa.Test.Collection.ListTests.PopulateList"));
		}

		[Fact]
		public void Foreach()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.Foreach(), Run<int>("Mosa.Test.Collection.ListTests.Foreach"));
		}

		[Fact]
		public void ForeachNested()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.ForeachNested(), Run<int>("Mosa.Test.Collection.ListTests.ForeachNested"));
		}

		[Fact]
		public void ForeachBreak()
		{
			Assert.Equal(Mosa.Test.Collection.ListTests.ForeachBreak(), Run<int>("Mosa.Test.Collection.ListTests.ForeachBreak"));
		}
	}
}
