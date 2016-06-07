// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class ValueTypeFixture : TestFixture
	{
		[Fact]
		public void TestValueTypeVariable()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeVariable"));
		}

		[Fact]
		public void TestValueTypeStaticField()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeStaticField"));
		}

		[Fact]
		public void TestValueTypeInstanceField()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeInstanceField"));
		}

		[Fact]
		public void TestValueTypeParameter()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeParameter"));
		}

		[Fact]
		public void TestValueTypeReturnValue()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeReturnValue"));
		}

		[Fact]
		public void TestValueTypeBox()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeBox"));
		}

		[Fact]
		public void TestValueTypeInstanceMethod()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeInstanceMethod"));
		}

		[Fact]
		public void TestValueTypeVirtualMethod()
		{
			Assert.Equal(Mosa.Test.Collection.ValueTypeTests.TestValueTypeVirtualMethod(), Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeVirtualMethod"));
		}

		[Fact]
		public void TestValueTypePassByRef()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypePassByRef"));
		}

		[Fact]
		public void TestValueTypePassByRefModify()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypePassByRefModify"));
		}

		[Fact]
		public void TestValueTypeArray()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeArray"));
		}

		[Fact]
		public void TestValueTypeArrayByRef()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ValueTypeTests.TestValueTypeArrayByRef"));
		}
	}
}
