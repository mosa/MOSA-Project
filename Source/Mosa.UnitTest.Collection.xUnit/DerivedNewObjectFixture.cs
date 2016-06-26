// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class DerivedNewObjectFixture : TestFixture
	{
		[Fact]
		public void WithoutArgs()
		{
			Assert.Equal(Mosa.UnitTest.Collection.DerivedNewObjectTests.WithoutArgs(), Run<bool>("Mosa.UnitTest.Collection.DerivedNewObjectTests.WithoutArgs"));
		}

		[Fact]
		public void WithOneArg()
		{
			Assert.Equal(Mosa.UnitTest.Collection.DerivedNewObjectTests.WithOneArg(), Run<bool>("Mosa.UnitTest.Collection.DerivedNewObjectTests.WithOneArg"));
		}

		[Fact]
		public void WithTwoArgs()
		{
			Assert.Equal(Mosa.UnitTest.Collection.DerivedNewObjectTests.WithTwoArgs(), Run<bool>("Mosa.UnitTest.Collection.DerivedNewObjectTests.WithTwoArgs"));
		}

		[Fact]
		public void WithThreeArgs()
		{
			Assert.Equal(Mosa.UnitTest.Collection.DerivedNewObjectTests.WithThreeArgs(), Run<bool>("Mosa.UnitTest.Collection.DerivedNewObjectTests.WithThreeArgs"));
		}
	}
}
