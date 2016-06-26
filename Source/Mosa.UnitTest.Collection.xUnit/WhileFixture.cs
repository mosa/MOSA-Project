// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class WhileFixture : TestFixture
	{
		#region WhileIncI4 test

		[Theory]
		[InlineData(0, 20)]
		[InlineData(-20, 0)]
		[InlineData(-100, 100)]
		public void WhileIncI4(int start, int limit)
		{
			Assert.Equal<int>(WhileTests.WhileIncI4(start, limit), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileIncI4", start, limit));
		}

		#endregion WhileIncI4 test

		#region WhileDecI4 test

		[Theory]
		[InlineData(20, 0)]
		[InlineData(0, -20)]
		[InlineData(100, -100)]
		public void WhileDecI4(int start, int limit)
		{
			Assert.Equal<int>(WhileTests.WhileDecI4(start, limit), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileDecI4", start, limit));
		}

		#endregion WhileDecI4 test

		#region WhileFalse() test

		[Fact]
		public void WhileFalse()
		{
			Assert.False(Run<bool>("Mosa.UnitTest.Collection.WhileTests.WhileFalse"));
		}

		#endregion WhileFalse() test

		#region WhileContinueBreak() test

		[Fact]
		public void WhileContinueBreak()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.WhileTests.WhileContinueBreak"));
		}

		#endregion WhileContinueBreak() test

		#region WhileContinueBreak2B() test

		[Fact]
		public void WhileContinueBreak2B()
		{
			Assert.Equal<int>(WhileTests.WhileContinueBreak2B(), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileContinueBreak2B"));
		}

		#endregion WhileContinueBreak2B() test

		#region WhileContinueBreak2() test

		[Fact]
		public void WhileContinueBreak2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.WhileTests.WhileContinueBreak2"));
		}

		#endregion WhileContinueBreak2() test

		#region WhileOverflowIncI1 test

		[Theory]
		[InlineData((byte)254, (byte)1)]
		[InlineData(byte.MaxValue, byte.MinValue)]
		public void WhileOverflowIncI1(byte start, byte limit)
		{
			Assert.Equal<int>(WhileTests.WhileOverflowIncI1(start, limit), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileOverflowIncI1", start, limit));
		}

		#endregion WhileOverflowIncI1 test

		#region WhileOverflowDecI1 test

		[Theory]
		[InlineData((byte)1, (byte)254)]
		[InlineData(byte.MinValue, byte.MaxValue)]
		public void WhileOverflowDecI1(byte start, byte limit)
		{
			Assert.Equal<int>(WhileTests.WhileOverflowDecI1(start, limit), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileOverflowDecI1", start, limit));
		}

		#endregion WhileOverflowDecI1 test

		#region WhileNestedEqualsI4 test

		[Theory]
		[InlineData(2, 3, 0, 20)]
		[InlineData(0, 1, 100, 200)]
		[InlineData(1, 0, -100, 100)]
		[InlineData(int.MaxValue, int.MinValue, -2, 3)]
		public void WhileNestedEqualsI4(int a, int b, int start, int limit)
		{
			Assert.Equal<int>(WhileTests.WhileNestedEqualsI4(a, b, start, limit), Run<int>("Mosa.UnitTest.Collection.WhileTests.WhileNestedEqualsI4", a, b, start, limit));
		}

		#endregion WhileNestedEqualsI4 test
	}
}
