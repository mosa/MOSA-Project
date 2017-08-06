// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class NullCheckOptimizationFixture : TestFixture
	{
		[Fact]
		public void NullTest1()
		{
			Assert.Equal(Mosa.UnitTest.Collection.NullCheckOptimizationTests.NullTest1(), Run<bool>("Mosa.UnitTest.Collection.NullCheckOptimizationTests.NullTest1"));
		}

		[Fact]
		public void NullTest2()
		{
			Assert.Equal(Mosa.UnitTest.Collection.NullCheckOptimizationTests.NullTest2(), Run<bool>("Mosa.UnitTest.Collection.NullCheckOptimizationTests.NullTest2"));
		}
	}
}
