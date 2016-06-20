// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class ValueTypeFixture : TestFixture
	{
		[Fact]
		public void TestValueTypeVariable()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeVariable"));
		}

		[Fact]
		public void TestValueTypeStaticField()
		{
			Assert.Equal(ValueTypeTests.TestValueTypeStaticField(), Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeStaticField"));
		}

		[Fact]
		public void TestValueTypeInstanceField()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeInstanceField"));
		}

		[Fact]
		public void TestValueTypeParameter()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeParameter"));
		}

		[Fact]
		public void TestValueTypeReturnValue()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeReturnValue"));
		}

		[Fact]
		public void TestValueTypeBox()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeBox"));
		}

		[Fact]
		public void TestValueTypeInstanceMethod()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeInstanceMethod"));
		}

		[Fact]
		public void TestValueTypeVirtualMethod()
		{
			Assert.Equal(Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeVirtualMethod(), Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeVirtualMethod"));
		}

		[Fact]
		public void TestValueTypePassByRef()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypePassByRef"));
		}

		[Fact]
		public void TestValueTypePassByRefModify()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypePassByRefModify"));
		}

		[Fact]
		public void TestValueTypeArray()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeArray"));
		}

		[Fact]
		public void TestValueTypeArrayByRef()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.ValueTypeTests.TestValueTypeArrayByRef"));
		}
	}
}
