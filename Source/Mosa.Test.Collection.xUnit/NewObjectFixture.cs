// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class NewObjectFixture : TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.Test.Collection.NewObjectTests.Create(), Run<bool>("Mosa.Test.Collection.NewObjectTests.Create"));
		}

		[Fact]
		public void CreateAndCallMethod()
		{
			Assert.Equal(Mosa.Test.Collection.NewObjectTests.CreateAndCallMethod(), Run<bool>("Mosa.Test.Collection.NewObjectTests.CreateAndCallMethod"));
		}
	}
}
