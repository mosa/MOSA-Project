// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class NewObjectFixture : TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.UnitTest.Collection.NewObjectTests.Create(), Run<bool>("Mosa.UnitTest.Collection.NewObjectTests.Create"));
		}

		[Fact]
		public void CreateAndCallMethod()
		{
			Assert.Equal(Mosa.UnitTest.Collection.NewObjectTests.CreateAndCallMethod(), Run<bool>("Mosa.UnitTest.Collection.NewObjectTests.CreateAndCallMethod"));
		}
	}
}
