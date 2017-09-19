// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class GenericMethodFixture : TestFixture
	{
		[Fact]
		public void MethodTestInt()
		{
			Assert.Equal(GenericMethodTests.MethodTestInt(), Run<int>("Mosa.UnitTest.Collection.GenericMethodTests.MethodTestInt"));
		}

		[Fact]
		public void MethodTestObject()
		{
			Assert.Equal(GenericMethodTests.MethodTestObject(), Run<int>("Mosa.UnitTest.Collection.GenericMethodTests.MethodTestObject"));
		}

		[Fact]
		public void MethodTestString()
		{
			Assert.Equal(GenericMethodTests.MethodTestString(), Run<int>("Mosa.UnitTest.Collection.GenericMethodTests.MethodTestString"));
		}
	}
}
