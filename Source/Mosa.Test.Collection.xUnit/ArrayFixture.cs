// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class ArrayFixture : TestFixture
	{
		[Fact]
		public void BoundsCheck()
		{
			Assert.Equal(Mosa.Test.Collection.ArrayTest.BoundsCheck(), Run<bool>("Mosa.Test.Collection.ArrayTest.BoundsCheck"));
		}
	}
}
