// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class StringFixture : TestFixture
	{
		[Fact]
		public void CheckLength()
		{
			Assert.Equal(StringTests.CheckLength(), Run<int>("Mosa.UnitTest.Collection.StringTests.CheckLength"));
		}

		[Fact]
		public void CheckFirstCharacter()
		{
			Assert.Equal(StringTests.CheckFirstCharacter(), Run<char>("Mosa.UnitTest.Collection.StringTests.CheckFirstCharacter"));
		}

		[Fact]
		public void CheckLastCharacter()
		{
			Assert.Equal(StringTests.CheckLastCharacter(), Run<char>("Mosa.UnitTest.Collection.StringTests.CheckLastCharacter"));
		}

		[Fact]
		public void LastCharacterMustMatch()
		{
			Assert.Equal(StringTests.LastCharacterMustMatch(), Run<char>("Mosa.UnitTest.Collection.StringTests.LastCharacterMustMatch"));
		}

		[Fact]
		public void ConcatTest1()
		{
			Assert.Equal(StringTests.ConcatTest1(), Run<bool>("Mosa.UnitTest.Collection.StringTests.ConcatTest1"));
		}

		[Fact]
		public void ConcatTest2()
		{
			Assert.Equal(StringTests.ConcatTest2(), Run<bool>("Mosa.UnitTest.Collection.StringTests.ConcatTest2"));
		}

		[Fact]
		public void Equal1()
		{
			Assert.Equal(StringTests.Equal1(), Run<bool>("Mosa.UnitTest.Collection.StringTests.Equal1"));
		}

		[Fact]
		public void NotEqual1()
		{
			Assert.Equal(StringTests.NotEqual1(), Run<bool>("Mosa.UnitTest.Collection.StringTests.NotEqual1"));
		}
	}
}
