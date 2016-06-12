// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class DerivedNewObjectFixture : TestFixture
	{
		[Fact]
		public void WithoutArgs()
		{
			Assert.Equal(Mosa.Test.Collection.DerivedNewObjectTests.WithoutArgs(), Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithoutArgs"));
		}

		[Fact]
		public void WithOneArg()
		{
			Assert.Equal(Mosa.Test.Collection.DerivedNewObjectTests.WithOneArg(), Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithOneArg"));
		}

		[Fact]
		public void WithTwoArgs()
		{
			Assert.Equal(Mosa.Test.Collection.DerivedNewObjectTests.WithTwoArgs(), Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithTwoArgs"));
		}

		[Fact]
		public void WithThreeArgs()
		{
			Assert.Equal(Mosa.Test.Collection.DerivedNewObjectTests.WithThreeArgs(), Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithThreeArgs"));
		}
	}
}
