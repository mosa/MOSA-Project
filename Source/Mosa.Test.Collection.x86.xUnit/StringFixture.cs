// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class StringFixture : X86TestFixture
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
	}
}
