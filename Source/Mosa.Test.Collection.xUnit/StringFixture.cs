// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class StringFixture : TestFixture
	{
		[Fact]
		public void CheckLength()
		{
			Assert.Equal(StringTests.CheckLength(), Run<int>("Mosa.Test.Collection.StringTests.CheckLength"));
		}

		[Fact]
		public void CheckFirstCharacter()
		{
			Assert.Equal(StringTests.CheckFirstCharacter(), Run<char>("Mosa.Test.Collection.StringTests.CheckFirstCharacter"));
		}

		[Fact]
		public void CheckLastCharacter()
		{
			Assert.Equal(StringTests.CheckLastCharacter(), Run<char>("Mosa.Test.Collection.StringTests.CheckLastCharacter"));
		}

		[Fact]
		public void LastCharacterMustMatch()
		{
			Assert.Equal(StringTests.LastCharacterMustMatch(), Run<char>("Mosa.Test.Collection.StringTests.LastCharacterMustMatch"));
		}

		[Fact]
		public void ConcatTest1()
		{
			Assert.Equal(StringTests.ConcatTest1(), Run<bool>("Mosa.Test.Collection.StringTests.ConcatTest1"));
		}

		[Fact]
		public void ConcatTest2()
		{
			Assert.Equal(StringTests.ConcatTest2(), Run<bool>("Mosa.Test.Collection.StringTests.ConcatTest2"));
		}

		[Fact]
		public void Equal1()
		{
			Assert.Equal(StringTests.Equal1(), Run<bool>("Mosa.Test.Collection.StringTests.Equal1"));
		}

		[Fact]
		public void NotEqual1()
		{
			Assert.Equal(StringTests.NotEqual1(), Run<bool>("Mosa.Test.Collection.StringTests.NotEqual1"));
		}
	}
}
