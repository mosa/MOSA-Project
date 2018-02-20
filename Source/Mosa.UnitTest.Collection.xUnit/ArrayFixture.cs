// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class ArrayFixture : TestFixture
	{
		[Fact]
		public void BoundsCheck()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ArrayTest.BoundsCheck(), Run<bool>("Mosa.UnitTest.Collection.ArrayTest.BoundsCheck"));
		}

		//[Fact]
		//public void MultiDimensionalArray()
		//{
		//	Assert.Equal(Mosa.UnitTest.Collection.ArrayTest.MultiDimensionalArray2D(), Run<bool>("Mosa.UnitTest.Collection.ArrayTest.MultiDimensionalArray2D"));
		//}
	}
}
