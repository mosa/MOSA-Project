// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class Fixture_Enum : TestFixture
	{

		[Fact]
		public void EnumU1_Conversion()
		{
			Assert.Equal(EnumU1.PositiveConversion(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveConversion"));
		}

		[Fact]
		public void EnumU1_PlusOne1()
		{
			Assert.Equal(EnumU1.PositivePlusOne1(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePlusOne1"));
		}

		[Fact]
		public void EnumU1_PlusOne2()
		{
			Assert.Equal(EnumU1.PositivePlusOne2(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePlusOne2"));
		}

		[Fact]
		public void EnumU1_MinusOne1()
		{
			Assert.Equal(EnumU1.PositiveMinusOne1(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveMinusOne1"));
		}

		[Fact]
		public void EnumU1_MinusOne2()
		{
			Assert.Equal(EnumU1.PositiveMinusOne2(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveMinusOne2"));
		}

		[Fact]
		public void EnumU1_Shl()
		{
			Assert.Equal(EnumU1.PositiveShl(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveShl"));
		}

		[Fact]
		public void EnumU1_Shr()
		{
			Assert.Equal(EnumU1.PositiveShr(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveShr"));
		}

		[Fact]
		public void EnumU1_Mul2()
		{
			Assert.Equal(EnumU1.PositiveMul2(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveMul2"));
		}

		[Fact]
		public void EnumU1_Div2()
		{
			Assert.Equal(EnumU1.PositiveDiv2(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveDiv2"));
		}

		[Fact]
		public void EnumU1_Rem2()
		{
			Assert.Equal(EnumU1.PositiveRem2(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveRem2"));
		}

		[Fact]
		public void EnumU1_AssignPlusOne()
		{
			Assert.Equal(EnumU1.PositiveAssignPlusOne(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU1_AssignMinusOne()
		{
			Assert.Equal(EnumU1.PositiveAssignMinusOne(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU1_Preincrement()
		{
			Assert.Equal(EnumU1.PositivePreincrement(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePreincrement"));
		}

		[Fact]
		public void EnumU1_Predecrement()
		{
			Assert.Equal(EnumU1.PositivePredecrement(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePredecrement"));
		}

		[Fact]
		public void EnumU1_Postincrement()
		{
			Assert.Equal(EnumU1.PositivePostincrement(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePostincrement"));
		}

		[Fact]
		public void EnumU1_Postdecrement()
		{
			Assert.Equal(EnumU1.PositivePostdecrement(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU1_And()
		{
			Assert.Equal(EnumU1.PositiveAnd(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveAnd"));
		}

		[Fact]
		public void EnumU1_Or()
		{
			Assert.Equal(EnumU1.PositiveOr(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveOr"));
		}

		[Fact]
		public void EnumU1_XOr()
		{
			Assert.Equal(EnumU1.PositiveXOr(), Run<byte>("Mosa.UnitTest.Collection.EnumU1.PositiveXOr"));
		}

		[Fact]
		public void EnumU1_Equal1()
		{
			Assert.Equal(EnumU1.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveEqual1"));
		}

		[Fact]
		public void EnumU1_Equal2()
		{
			Assert.Equal(EnumU1.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveEqual2"));
		}

		[Fact]
		public void EnumU1_Equal3()
		{
			Assert.Equal(EnumU1.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveEqual3"));
		}

		[Fact]
		public void EnumU1_NotEqual1()
		{
			Assert.Equal(EnumU1.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU1_NotEqual2()
		{
			Assert.Equal(EnumU1.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU1_NotEqual3()
		{
			Assert.Equal(EnumU1.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU1_GreaterThan1()
		{
			Assert.Equal(EnumU1.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU1_GreaterThan2()
		{
			Assert.Equal(EnumU1.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU1_GreaterThan3()
		{
			Assert.Equal(EnumU1.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU1_LessThan1()
		{
			Assert.Equal(EnumU1.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU1_LessThan2()
		{
			Assert.Equal(EnumU1.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU1_LessThan3()
		{
			Assert.Equal(EnumU1.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumU1.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumU1.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumU1.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual1()
		{
			Assert.Equal(EnumU1.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual2()
		{
			Assert.Equal(EnumU1.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual3()
		{
			Assert.Equal(EnumU1.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU1.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumU2_Conversion()
		{
			Assert.Equal(EnumU2.PositiveConversion(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveConversion"));
		}

		[Fact]
		public void EnumU2_PlusOne1()
		{
			Assert.Equal(EnumU2.PositivePlusOne1(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePlusOne1"));
		}

		[Fact]
		public void EnumU2_PlusOne2()
		{
			Assert.Equal(EnumU2.PositivePlusOne2(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePlusOne2"));
		}

		[Fact]
		public void EnumU2_MinusOne1()
		{
			Assert.Equal(EnumU2.PositiveMinusOne1(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveMinusOne1"));
		}

		[Fact]
		public void EnumU2_MinusOne2()
		{
			Assert.Equal(EnumU2.PositiveMinusOne2(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveMinusOne2"));
		}

		[Fact]
		public void EnumU2_Shl()
		{
			Assert.Equal(EnumU2.PositiveShl(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveShl"));
		}

		[Fact]
		public void EnumU2_Shr()
		{
			Assert.Equal(EnumU2.PositiveShr(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveShr"));
		}

		[Fact]
		public void EnumU2_Mul2()
		{
			Assert.Equal(EnumU2.PositiveMul2(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveMul2"));
		}

		[Fact]
		public void EnumU2_Div2()
		{
			Assert.Equal(EnumU2.PositiveDiv2(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveDiv2"));
		}

		[Fact]
		public void EnumU2_Rem2()
		{
			Assert.Equal(EnumU2.PositiveRem2(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveRem2"));
		}

		[Fact]
		public void EnumU2_AssignPlusOne()
		{
			Assert.Equal(EnumU2.PositiveAssignPlusOne(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU2_AssignMinusOne()
		{
			Assert.Equal(EnumU2.PositiveAssignMinusOne(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU2_Preincrement()
		{
			Assert.Equal(EnumU2.PositivePreincrement(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePreincrement"));
		}

		[Fact]
		public void EnumU2_Predecrement()
		{
			Assert.Equal(EnumU2.PositivePredecrement(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePredecrement"));
		}

		[Fact]
		public void EnumU2_Postincrement()
		{
			Assert.Equal(EnumU2.PositivePostincrement(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePostincrement"));
		}

		[Fact]
		public void EnumU2_Postdecrement()
		{
			Assert.Equal(EnumU2.PositivePostdecrement(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU2_And()
		{
			Assert.Equal(EnumU2.PositiveAnd(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveAnd"));
		}

		[Fact]
		public void EnumU2_Or()
		{
			Assert.Equal(EnumU2.PositiveOr(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveOr"));
		}

		[Fact]
		public void EnumU2_XOr()
		{
			Assert.Equal(EnumU2.PositiveXOr(), Run<ushort>("Mosa.UnitTest.Collection.EnumU2.PositiveXOr"));
		}

		[Fact]
		public void EnumU2_Equal1()
		{
			Assert.Equal(EnumU2.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveEqual1"));
		}

		[Fact]
		public void EnumU2_Equal2()
		{
			Assert.Equal(EnumU2.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveEqual2"));
		}

		[Fact]
		public void EnumU2_Equal3()
		{
			Assert.Equal(EnumU2.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveEqual3"));
		}

		[Fact]
		public void EnumU2_NotEqual1()
		{
			Assert.Equal(EnumU2.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU2_NotEqual2()
		{
			Assert.Equal(EnumU2.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU2_NotEqual3()
		{
			Assert.Equal(EnumU2.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU2_GreaterThan1()
		{
			Assert.Equal(EnumU2.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU2_GreaterThan2()
		{
			Assert.Equal(EnumU2.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU2_GreaterThan3()
		{
			Assert.Equal(EnumU2.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU2_LessThan1()
		{
			Assert.Equal(EnumU2.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU2_LessThan2()
		{
			Assert.Equal(EnumU2.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU2_LessThan3()
		{
			Assert.Equal(EnumU2.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumU2.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumU2.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumU2.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual1()
		{
			Assert.Equal(EnumU2.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual2()
		{
			Assert.Equal(EnumU2.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual3()
		{
			Assert.Equal(EnumU2.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU2.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumU4_Conversion()
		{
			Assert.Equal(EnumU4.PositiveConversion(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveConversion"));
		}

		[Fact]
		public void EnumU4_PlusOne1()
		{
			Assert.Equal(EnumU4.PositivePlusOne1(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePlusOne1"));
		}

		[Fact]
		public void EnumU4_PlusOne2()
		{
			Assert.Equal(EnumU4.PositivePlusOne2(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePlusOne2"));
		}

		[Fact]
		public void EnumU4_MinusOne1()
		{
			Assert.Equal(EnumU4.PositiveMinusOne1(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveMinusOne1"));
		}

		[Fact]
		public void EnumU4_MinusOne2()
		{
			Assert.Equal(EnumU4.PositiveMinusOne2(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveMinusOne2"));
		}

		[Fact]
		public void EnumU4_Shl()
		{
			Assert.Equal(EnumU4.PositiveShl(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveShl"));
		}

		[Fact]
		public void EnumU4_Shr()
		{
			Assert.Equal(EnumU4.PositiveShr(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveShr"));
		}

		[Fact]
		public void EnumU4_Mul2()
		{
			Assert.Equal(EnumU4.PositiveMul2(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveMul2"));
		}

		[Fact]
		public void EnumU4_Div2()
		{
			Assert.Equal(EnumU4.PositiveDiv2(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveDiv2"));
		}

		[Fact]
		public void EnumU4_Rem2()
		{
			Assert.Equal(EnumU4.PositiveRem2(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveRem2"));
		}

		[Fact]
		public void EnumU4_AssignPlusOne()
		{
			Assert.Equal(EnumU4.PositiveAssignPlusOne(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU4_AssignMinusOne()
		{
			Assert.Equal(EnumU4.PositiveAssignMinusOne(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU4_Preincrement()
		{
			Assert.Equal(EnumU4.PositivePreincrement(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePreincrement"));
		}

		[Fact]
		public void EnumU4_Predecrement()
		{
			Assert.Equal(EnumU4.PositivePredecrement(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePredecrement"));
		}

		[Fact]
		public void EnumU4_Postincrement()
		{
			Assert.Equal(EnumU4.PositivePostincrement(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePostincrement"));
		}

		[Fact]
		public void EnumU4_Postdecrement()
		{
			Assert.Equal(EnumU4.PositivePostdecrement(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU4_And()
		{
			Assert.Equal(EnumU4.PositiveAnd(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveAnd"));
		}

		[Fact]
		public void EnumU4_Or()
		{
			Assert.Equal(EnumU4.PositiveOr(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveOr"));
		}

		[Fact]
		public void EnumU4_XOr()
		{
			Assert.Equal(EnumU4.PositiveXOr(), Run<uint>("Mosa.UnitTest.Collection.EnumU4.PositiveXOr"));
		}

		[Fact]
		public void EnumU4_Equal1()
		{
			Assert.Equal(EnumU4.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveEqual1"));
		}

		[Fact]
		public void EnumU4_Equal2()
		{
			Assert.Equal(EnumU4.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveEqual2"));
		}

		[Fact]
		public void EnumU4_Equal3()
		{
			Assert.Equal(EnumU4.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveEqual3"));
		}

		[Fact]
		public void EnumU4_NotEqual1()
		{
			Assert.Equal(EnumU4.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU4_NotEqual2()
		{
			Assert.Equal(EnumU4.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU4_NotEqual3()
		{
			Assert.Equal(EnumU4.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU4_GreaterThan1()
		{
			Assert.Equal(EnumU4.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU4_GreaterThan2()
		{
			Assert.Equal(EnumU4.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU4_GreaterThan3()
		{
			Assert.Equal(EnumU4.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU4_LessThan1()
		{
			Assert.Equal(EnumU4.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU4_LessThan2()
		{
			Assert.Equal(EnumU4.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU4_LessThan3()
		{
			Assert.Equal(EnumU4.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumU4.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumU4.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumU4.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual1()
		{
			Assert.Equal(EnumU4.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual2()
		{
			Assert.Equal(EnumU4.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual3()
		{
			Assert.Equal(EnumU4.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU4.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumU8_Conversion()
		{
			Assert.Equal(EnumU8.PositiveConversion(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveConversion"));
		}

		[Fact]
		public void EnumU8_PlusOne1()
		{
			Assert.Equal(EnumU8.PositivePlusOne1(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePlusOne1"));
		}

		[Fact]
		public void EnumU8_PlusOne2()
		{
			Assert.Equal(EnumU8.PositivePlusOne2(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePlusOne2"));
		}

		[Fact]
		public void EnumU8_MinusOne1()
		{
			Assert.Equal(EnumU8.PositiveMinusOne1(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveMinusOne1"));
		}

		[Fact]
		public void EnumU8_MinusOne2()
		{
			Assert.Equal(EnumU8.PositiveMinusOne2(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveMinusOne2"));
		}

		[Fact]
		public void EnumU8_Shl()
		{
			Assert.Equal(EnumU8.PositiveShl(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveShl"));
		}

		[Fact]
		public void EnumU8_Shr()
		{
			Assert.Equal(EnumU8.PositiveShr(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveShr"));
		}

		[Fact]
		public void EnumU8_Mul2()
		{
			Assert.Equal(EnumU8.PositiveMul2(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveMul2"));
		}

		[Fact]
		public void EnumU8_Div2()
		{
			Assert.Equal(EnumU8.PositiveDiv2(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveDiv2"));
		}

		[Fact]
		public void EnumU8_Rem2()
		{
			Assert.Equal(EnumU8.PositiveRem2(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveRem2"));
		}

		[Fact]
		public void EnumU8_AssignPlusOne()
		{
			Assert.Equal(EnumU8.PositiveAssignPlusOne(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU8_AssignMinusOne()
		{
			Assert.Equal(EnumU8.PositiveAssignMinusOne(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU8_Preincrement()
		{
			Assert.Equal(EnumU8.PositivePreincrement(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePreincrement"));
		}

		[Fact]
		public void EnumU8_Predecrement()
		{
			Assert.Equal(EnumU8.PositivePredecrement(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePredecrement"));
		}

		[Fact]
		public void EnumU8_Postincrement()
		{
			Assert.Equal(EnumU8.PositivePostincrement(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePostincrement"));
		}

		[Fact]
		public void EnumU8_Postdecrement()
		{
			Assert.Equal(EnumU8.PositivePostdecrement(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU8_And()
		{
			Assert.Equal(EnumU8.PositiveAnd(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveAnd"));
		}

		[Fact]
		public void EnumU8_Or()
		{
			Assert.Equal(EnumU8.PositiveOr(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveOr"));
		}

		[Fact]
		public void EnumU8_XOr()
		{
			Assert.Equal(EnumU8.PositiveXOr(), Run<ulong>("Mosa.UnitTest.Collection.EnumU8.PositiveXOr"));
		}

		[Fact]
		public void EnumU8_Equal1()
		{
			Assert.Equal(EnumU8.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveEqual1"));
		}

		[Fact]
		public void EnumU8_Equal2()
		{
			Assert.Equal(EnumU8.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveEqual2"));
		}

		[Fact]
		public void EnumU8_Equal3()
		{
			Assert.Equal(EnumU8.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveEqual3"));
		}

		[Fact]
		public void EnumU8_NotEqual1()
		{
			Assert.Equal(EnumU8.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU8_NotEqual2()
		{
			Assert.Equal(EnumU8.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU8_NotEqual3()
		{
			Assert.Equal(EnumU8.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU8_GreaterThan1()
		{
			Assert.Equal(EnumU8.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU8_GreaterThan2()
		{
			Assert.Equal(EnumU8.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU8_GreaterThan3()
		{
			Assert.Equal(EnumU8.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU8_LessThan1()
		{
			Assert.Equal(EnumU8.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU8_LessThan2()
		{
			Assert.Equal(EnumU8.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU8_LessThan3()
		{
			Assert.Equal(EnumU8.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU8_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumU8.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU8_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumU8.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU8_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumU8.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU8_LessThanOrEqual1()
		{
			Assert.Equal(EnumU8.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU8_LessThanOrEqual2()
		{
			Assert.Equal(EnumU8.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU8_LessThanOrEqual3()
		{
			Assert.Equal(EnumU8.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumU8.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI1_Conversion()
		{
			Assert.Equal(EnumI1.PositiveConversion(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveConversion"));
			Assert.Equal(EnumI1.NegativeConversion(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeConversion"));
		}

		[Fact]
		public void EnumI1_PlusOne1()
		{
			Assert.Equal(EnumI1.PositivePlusOne1(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePlusOne1"));
			Assert.Equal(EnumI1.NegativePlusOne1(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePlusOne1"));
		}

		[Fact]
		public void EnumI1_PlusOne2()
		{
			Assert.Equal(EnumI1.PositivePlusOne2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePlusOne2"));
			Assert.Equal(EnumI1.NegativePlusOne2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePlusOne2"));
		}

		[Fact]
		public void EnumI1_MinusOne1()
		{
			Assert.Equal(EnumI1.PositiveMinusOne1(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveMinusOne1"));
			Assert.Equal(EnumI1.NegativeMinusOne1(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeMinusOne1"));
		}

		[Fact]
		public void EnumI1_MinusOne2()
		{
			Assert.Equal(EnumI1.PositiveMinusOne2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveMinusOne2"));
			Assert.Equal(EnumI1.NegativeMinusOne2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeMinusOne2"));
		}

		[Fact]
		public void EnumI1_Shl()
		{
			Assert.Equal(EnumI1.PositiveShl(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveShl"));
			Assert.Equal(EnumI1.NegativeShl(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeShl"));
		}

		[Fact]
		public void EnumI1_Shr()
		{
			Assert.Equal(EnumI1.PositiveShr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveShr"));
			Assert.Equal(EnumI1.NegativeShr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeShr"));
		}

		[Fact]
		public void EnumI1_Mul2()
		{
			Assert.Equal(EnumI1.PositiveMul2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveMul2"));
			Assert.Equal(EnumI1.NegativeMul2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeMul2"));
		}

		[Fact]
		public void EnumI1_Div2()
		{
			Assert.Equal(EnumI1.PositiveDiv2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveDiv2"));
			Assert.Equal(EnumI1.NegativeDiv2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeDiv2"));
		}

		[Fact]
		public void EnumI1_Rem2()
		{
			Assert.Equal(EnumI1.PositiveRem2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveRem2"));
			Assert.Equal(EnumI1.NegativeRem2(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeRem2"));
		}

		[Fact]
		public void EnumI1_AssignPlusOne()
		{
			Assert.Equal(EnumI1.PositiveAssignPlusOne(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveAssignPlusOne"));
			Assert.Equal(EnumI1.NegativeAssignPlusOne(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI1_AssignMinusOne()
		{
			Assert.Equal(EnumI1.PositiveAssignMinusOne(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveAssignMinusOne"));
			Assert.Equal(EnumI1.NegativeAssignMinusOne(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI1_Preincrement()
		{
			Assert.Equal(EnumI1.PositivePreincrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePreincrement"));
			Assert.Equal(EnumI1.NegativePreincrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePreincrement"));
		}

		[Fact]
		public void EnumI1_Predecrement()
		{
			Assert.Equal(EnumI1.PositivePredecrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePredecrement"));
			Assert.Equal(EnumI1.NegativePredecrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePredecrement"));
		}

		[Fact]
		public void EnumI1_Postincrement()
		{
			Assert.Equal(EnumI1.PositivePostincrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePostincrement"));
			Assert.Equal(EnumI1.NegativePostincrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePostincrement"));
		}

		[Fact]
		public void EnumI1_Postdecrement()
		{
			Assert.Equal(EnumI1.PositivePostdecrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositivePostdecrement"));
			Assert.Equal(EnumI1.NegativePostdecrement(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI1_And()
		{
			Assert.Equal(EnumI1.PositiveAnd(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveAnd"));
			Assert.Equal(EnumI1.NegativeAnd(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeAnd"));
		}

		[Fact]
		public void EnumI1_Or()
		{
			Assert.Equal(EnumI1.PositiveOr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveOr"));
			Assert.Equal(EnumI1.NegativeOr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeOr"));
		}

		[Fact]
		public void EnumI1_XOr()
		{
			Assert.Equal(EnumI1.PositiveXOr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.PositiveXOr"));
			Assert.Equal(EnumI1.NegativeXOr(), Run<sbyte>("Mosa.UnitTest.Collection.EnumI1.NegativeXOr"));
		}

		[Fact]
		public void EnumI1_Equal1()
		{
			Assert.Equal(EnumI1.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveEqual1"));
			Assert.Equal(EnumI1.NegativeEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeEqual1"));
		}

		[Fact]
		public void EnumI1_Equal2()
		{
			Assert.Equal(EnumI1.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveEqual2"));
			Assert.Equal(EnumI1.NegativeEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeEqual2"));
		}

		[Fact]
		public void EnumI1_Equal3()
		{
			Assert.Equal(EnumI1.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveEqual3"));
			Assert.Equal(EnumI1.NegativeEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeEqual3"));
		}

		[Fact]
		public void EnumI1_NotEqual1()
		{
			Assert.Equal(EnumI1.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveNotEqual1"));
			Assert.Equal(EnumI1.NegativeNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI1_NotEqual2()
		{
			Assert.Equal(EnumI1.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveNotEqual2"));
			Assert.Equal(EnumI1.NegativeNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI1_NotEqual3()
		{
			Assert.Equal(EnumI1.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveNotEqual3"));
			Assert.Equal(EnumI1.NegativeNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI1_GreaterThan1()
		{
			Assert.Equal(EnumI1.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThan1"));
			Assert.Equal(EnumI1.NegativeGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI1_GreaterThan2()
		{
			Assert.Equal(EnumI1.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThan2"));
			Assert.Equal(EnumI1.NegativeGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI1_GreaterThan3()
		{
			Assert.Equal(EnumI1.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThan3"));
			Assert.Equal(EnumI1.NegativeGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI1_LessThan1()
		{
			Assert.Equal(EnumI1.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThan1"));
			Assert.Equal(EnumI1.NegativeLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI1_LessThan2()
		{
			Assert.Equal(EnumI1.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThan2"));
			Assert.Equal(EnumI1.NegativeLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI1_LessThan3()
		{
			Assert.Equal(EnumI1.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThan3"));
			Assert.Equal(EnumI1.NegativeLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumI1.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThanOrEqual1"));
			Assert.Equal(EnumI1.NegativeGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumI1.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThanOrEqual2"));
			Assert.Equal(EnumI1.NegativeGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumI1.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveGreaterThanOrEqual3"));
			Assert.Equal(EnumI1.NegativeGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual1()
		{
			Assert.Equal(EnumI1.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThanOrEqual1"));
			Assert.Equal(EnumI1.NegativeLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual2()
		{
			Assert.Equal(EnumI1.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThanOrEqual2"));
			Assert.Equal(EnumI1.NegativeLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual3()
		{
			Assert.Equal(EnumI1.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.PositiveLessThanOrEqual3"));
			Assert.Equal(EnumI1.NegativeLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI1.NegativeLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI2_Conversion()
		{
			Assert.Equal(EnumI2.PositiveConversion(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveConversion"));
			Assert.Equal(EnumI2.NegativeConversion(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeConversion"));
		}

		[Fact]
		public void EnumI2_PlusOne1()
		{
			Assert.Equal(EnumI2.PositivePlusOne1(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePlusOne1"));
			Assert.Equal(EnumI2.NegativePlusOne1(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePlusOne1"));
		}

		[Fact]
		public void EnumI2_PlusOne2()
		{
			Assert.Equal(EnumI2.PositivePlusOne2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePlusOne2"));
			Assert.Equal(EnumI2.NegativePlusOne2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePlusOne2"));
		}

		[Fact]
		public void EnumI2_MinusOne1()
		{
			Assert.Equal(EnumI2.PositiveMinusOne1(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveMinusOne1"));
			Assert.Equal(EnumI2.NegativeMinusOne1(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeMinusOne1"));
		}

		[Fact]
		public void EnumI2_MinusOne2()
		{
			Assert.Equal(EnumI2.PositiveMinusOne2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveMinusOne2"));
			Assert.Equal(EnumI2.NegativeMinusOne2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeMinusOne2"));
		}

		[Fact]
		public void EnumI2_Shl()
		{
			Assert.Equal(EnumI2.PositiveShl(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveShl"));
			Assert.Equal(EnumI2.NegativeShl(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeShl"));
		}

		[Fact]
		public void EnumI2_Shr()
		{
			Assert.Equal(EnumI2.PositiveShr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveShr"));
			Assert.Equal(EnumI2.NegativeShr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeShr"));
		}

		[Fact]
		public void EnumI2_Mul2()
		{
			Assert.Equal(EnumI2.PositiveMul2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveMul2"));
			Assert.Equal(EnumI2.NegativeMul2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeMul2"));
		}

		[Fact]
		public void EnumI2_Div2()
		{
			Assert.Equal(EnumI2.PositiveDiv2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveDiv2"));
			Assert.Equal(EnumI2.NegativeDiv2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeDiv2"));
		}

		[Fact]
		public void EnumI2_Rem2()
		{
			Assert.Equal(EnumI2.PositiveRem2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveRem2"));
			Assert.Equal(EnumI2.NegativeRem2(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeRem2"));
		}

		[Fact]
		public void EnumI2_AssignPlusOne()
		{
			Assert.Equal(EnumI2.PositiveAssignPlusOne(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveAssignPlusOne"));
			Assert.Equal(EnumI2.NegativeAssignPlusOne(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI2_AssignMinusOne()
		{
			Assert.Equal(EnumI2.PositiveAssignMinusOne(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveAssignMinusOne"));
			Assert.Equal(EnumI2.NegativeAssignMinusOne(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI2_Preincrement()
		{
			Assert.Equal(EnumI2.PositivePreincrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePreincrement"));
			Assert.Equal(EnumI2.NegativePreincrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePreincrement"));
		}

		[Fact]
		public void EnumI2_Predecrement()
		{
			Assert.Equal(EnumI2.PositivePredecrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePredecrement"));
			Assert.Equal(EnumI2.NegativePredecrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePredecrement"));
		}

		[Fact]
		public void EnumI2_Postincrement()
		{
			Assert.Equal(EnumI2.PositivePostincrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePostincrement"));
			Assert.Equal(EnumI2.NegativePostincrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePostincrement"));
		}

		[Fact]
		public void EnumI2_Postdecrement()
		{
			Assert.Equal(EnumI2.PositivePostdecrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositivePostdecrement"));
			Assert.Equal(EnumI2.NegativePostdecrement(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI2_And()
		{
			Assert.Equal(EnumI2.PositiveAnd(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveAnd"));
			Assert.Equal(EnumI2.NegativeAnd(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeAnd"));
		}

		[Fact]
		public void EnumI2_Or()
		{
			Assert.Equal(EnumI2.PositiveOr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveOr"));
			Assert.Equal(EnumI2.NegativeOr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeOr"));
		}

		[Fact]
		public void EnumI2_XOr()
		{
			Assert.Equal(EnumI2.PositiveXOr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.PositiveXOr"));
			Assert.Equal(EnumI2.NegativeXOr(), Run<short>("Mosa.UnitTest.Collection.EnumI2.NegativeXOr"));
		}

		[Fact]
		public void EnumI2_Equal1()
		{
			Assert.Equal(EnumI2.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveEqual1"));
			Assert.Equal(EnumI2.NegativeEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeEqual1"));
		}

		[Fact]
		public void EnumI2_Equal2()
		{
			Assert.Equal(EnumI2.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveEqual2"));
			Assert.Equal(EnumI2.NegativeEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeEqual2"));
		}

		[Fact]
		public void EnumI2_Equal3()
		{
			Assert.Equal(EnumI2.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveEqual3"));
			Assert.Equal(EnumI2.NegativeEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeEqual3"));
		}

		[Fact]
		public void EnumI2_NotEqual1()
		{
			Assert.Equal(EnumI2.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveNotEqual1"));
			Assert.Equal(EnumI2.NegativeNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI2_NotEqual2()
		{
			Assert.Equal(EnumI2.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveNotEqual2"));
			Assert.Equal(EnumI2.NegativeNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI2_NotEqual3()
		{
			Assert.Equal(EnumI2.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveNotEqual3"));
			Assert.Equal(EnumI2.NegativeNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI2_GreaterThan1()
		{
			Assert.Equal(EnumI2.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThan1"));
			Assert.Equal(EnumI2.NegativeGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI2_GreaterThan2()
		{
			Assert.Equal(EnumI2.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThan2"));
			Assert.Equal(EnumI2.NegativeGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI2_GreaterThan3()
		{
			Assert.Equal(EnumI2.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThan3"));
			Assert.Equal(EnumI2.NegativeGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI2_LessThan1()
		{
			Assert.Equal(EnumI2.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThan1"));
			Assert.Equal(EnumI2.NegativeLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI2_LessThan2()
		{
			Assert.Equal(EnumI2.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThan2"));
			Assert.Equal(EnumI2.NegativeLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI2_LessThan3()
		{
			Assert.Equal(EnumI2.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThan3"));
			Assert.Equal(EnumI2.NegativeLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumI2.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThanOrEqual1"));
			Assert.Equal(EnumI2.NegativeGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumI2.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThanOrEqual2"));
			Assert.Equal(EnumI2.NegativeGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumI2.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveGreaterThanOrEqual3"));
			Assert.Equal(EnumI2.NegativeGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual1()
		{
			Assert.Equal(EnumI2.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThanOrEqual1"));
			Assert.Equal(EnumI2.NegativeLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual2()
		{
			Assert.Equal(EnumI2.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThanOrEqual2"));
			Assert.Equal(EnumI2.NegativeLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual3()
		{
			Assert.Equal(EnumI2.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.PositiveLessThanOrEqual3"));
			Assert.Equal(EnumI2.NegativeLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI2.NegativeLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI4_Conversion()
		{
			Assert.Equal(EnumI4.PositiveConversion(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveConversion"));
			Assert.Equal(EnumI4.NegativeConversion(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeConversion"));
		}

		[Fact]
		public void EnumI4_PlusOne1()
		{
			Assert.Equal(EnumI4.PositivePlusOne1(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePlusOne1"));
			Assert.Equal(EnumI4.NegativePlusOne1(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePlusOne1"));
		}

		[Fact]
		public void EnumI4_PlusOne2()
		{
			Assert.Equal(EnumI4.PositivePlusOne2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePlusOne2"));
			Assert.Equal(EnumI4.NegativePlusOne2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePlusOne2"));
		}

		[Fact]
		public void EnumI4_MinusOne1()
		{
			Assert.Equal(EnumI4.PositiveMinusOne1(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveMinusOne1"));
			Assert.Equal(EnumI4.NegativeMinusOne1(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeMinusOne1"));
		}

		[Fact]
		public void EnumI4_MinusOne2()
		{
			Assert.Equal(EnumI4.PositiveMinusOne2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveMinusOne2"));
			Assert.Equal(EnumI4.NegativeMinusOne2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeMinusOne2"));
		}

		[Fact]
		public void EnumI4_Shl()
		{
			Assert.Equal(EnumI4.PositiveShl(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveShl"));
			Assert.Equal(EnumI4.NegativeShl(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeShl"));
		}

		[Fact]
		public void EnumI4_Shr()
		{
			Assert.Equal(EnumI4.PositiveShr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveShr"));
			Assert.Equal(EnumI4.NegativeShr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeShr"));
		}

		[Fact]
		public void EnumI4_Mul2()
		{
			Assert.Equal(EnumI4.PositiveMul2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveMul2"));
			Assert.Equal(EnumI4.NegativeMul2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeMul2"));
		}

		[Fact]
		public void EnumI4_Div2()
		{
			Assert.Equal(EnumI4.PositiveDiv2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveDiv2"));
			Assert.Equal(EnumI4.NegativeDiv2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeDiv2"));
		}

		[Fact]
		public void EnumI4_Rem2()
		{
			Assert.Equal(EnumI4.PositiveRem2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveRem2"));
			Assert.Equal(EnumI4.NegativeRem2(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeRem2"));
		}

		[Fact]
		public void EnumI4_AssignPlusOne()
		{
			Assert.Equal(EnumI4.PositiveAssignPlusOne(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveAssignPlusOne"));
			Assert.Equal(EnumI4.NegativeAssignPlusOne(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI4_AssignMinusOne()
		{
			Assert.Equal(EnumI4.PositiveAssignMinusOne(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveAssignMinusOne"));
			Assert.Equal(EnumI4.NegativeAssignMinusOne(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI4_Preincrement()
		{
			Assert.Equal(EnumI4.PositivePreincrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePreincrement"));
			Assert.Equal(EnumI4.NegativePreincrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePreincrement"));
		}

		[Fact]
		public void EnumI4_Predecrement()
		{
			Assert.Equal(EnumI4.PositivePredecrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePredecrement"));
			Assert.Equal(EnumI4.NegativePredecrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePredecrement"));
		}

		[Fact]
		public void EnumI4_Postincrement()
		{
			Assert.Equal(EnumI4.PositivePostincrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePostincrement"));
			Assert.Equal(EnumI4.NegativePostincrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePostincrement"));
		}

		[Fact]
		public void EnumI4_Postdecrement()
		{
			Assert.Equal(EnumI4.PositivePostdecrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositivePostdecrement"));
			Assert.Equal(EnumI4.NegativePostdecrement(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI4_And()
		{
			Assert.Equal(EnumI4.PositiveAnd(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveAnd"));
			Assert.Equal(EnumI4.NegativeAnd(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeAnd"));
		}

		[Fact]
		public void EnumI4_Or()
		{
			Assert.Equal(EnumI4.PositiveOr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveOr"));
			Assert.Equal(EnumI4.NegativeOr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeOr"));
		}

		[Fact]
		public void EnumI4_XOr()
		{
			Assert.Equal(EnumI4.PositiveXOr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.PositiveXOr"));
			Assert.Equal(EnumI4.NegativeXOr(), Run<int>("Mosa.UnitTest.Collection.EnumI4.NegativeXOr"));
		}

		[Fact]
		public void EnumI4_Equal1()
		{
			Assert.Equal(EnumI4.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveEqual1"));
			Assert.Equal(EnumI4.NegativeEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeEqual1"));
		}

		[Fact]
		public void EnumI4_Equal2()
		{
			Assert.Equal(EnumI4.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveEqual2"));
			Assert.Equal(EnumI4.NegativeEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeEqual2"));
		}

		[Fact]
		public void EnumI4_Equal3()
		{
			Assert.Equal(EnumI4.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveEqual3"));
			Assert.Equal(EnumI4.NegativeEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeEqual3"));
		}

		[Fact]
		public void EnumI4_NotEqual1()
		{
			Assert.Equal(EnumI4.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveNotEqual1"));
			Assert.Equal(EnumI4.NegativeNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI4_NotEqual2()
		{
			Assert.Equal(EnumI4.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveNotEqual2"));
			Assert.Equal(EnumI4.NegativeNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI4_NotEqual3()
		{
			Assert.Equal(EnumI4.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveNotEqual3"));
			Assert.Equal(EnumI4.NegativeNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI4_GreaterThan1()
		{
			Assert.Equal(EnumI4.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThan1"));
			Assert.Equal(EnumI4.NegativeGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI4_GreaterThan2()
		{
			Assert.Equal(EnumI4.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThan2"));
			Assert.Equal(EnumI4.NegativeGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI4_GreaterThan3()
		{
			Assert.Equal(EnumI4.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThan3"));
			Assert.Equal(EnumI4.NegativeGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI4_LessThan1()
		{
			Assert.Equal(EnumI4.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThan1"));
			Assert.Equal(EnumI4.NegativeLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI4_LessThan2()
		{
			Assert.Equal(EnumI4.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThan2"));
			Assert.Equal(EnumI4.NegativeLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI4_LessThan3()
		{
			Assert.Equal(EnumI4.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThan3"));
			Assert.Equal(EnumI4.NegativeLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumI4.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThanOrEqual1"));
			Assert.Equal(EnumI4.NegativeGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumI4.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThanOrEqual2"));
			Assert.Equal(EnumI4.NegativeGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumI4.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveGreaterThanOrEqual3"));
			Assert.Equal(EnumI4.NegativeGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual1()
		{
			Assert.Equal(EnumI4.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThanOrEqual1"));
			Assert.Equal(EnumI4.NegativeLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual2()
		{
			Assert.Equal(EnumI4.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThanOrEqual2"));
			Assert.Equal(EnumI4.NegativeLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual3()
		{
			Assert.Equal(EnumI4.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.PositiveLessThanOrEqual3"));
			Assert.Equal(EnumI4.NegativeLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI4.NegativeLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI8_Conversion()
		{
			Assert.Equal(EnumI8.PositiveConversion(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveConversion"));
			Assert.Equal(EnumI8.NegativeConversion(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeConversion"));
		}

		[Fact]
		public void EnumI8_PlusOne1()
		{
			Assert.Equal(EnumI8.PositivePlusOne1(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePlusOne1"));
			Assert.Equal(EnumI8.NegativePlusOne1(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePlusOne1"));
		}

		[Fact]
		public void EnumI8_PlusOne2()
		{
			Assert.Equal(EnumI8.PositivePlusOne2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePlusOne2"));
			Assert.Equal(EnumI8.NegativePlusOne2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePlusOne2"));
		}

		[Fact]
		public void EnumI8_MinusOne1()
		{
			Assert.Equal(EnumI8.PositiveMinusOne1(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveMinusOne1"));
			Assert.Equal(EnumI8.NegativeMinusOne1(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeMinusOne1"));
		}

		[Fact]
		public void EnumI8_MinusOne2()
		{
			Assert.Equal(EnumI8.PositiveMinusOne2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveMinusOne2"));
			Assert.Equal(EnumI8.NegativeMinusOne2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeMinusOne2"));
		}

		[Fact]
		public void EnumI8_Shl()
		{
			Assert.Equal(EnumI8.PositiveShl(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveShl"));
			Assert.Equal(EnumI8.NegativeShl(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeShl"));
		}

		[Fact]
		public void EnumI8_Shr()
		{
			Assert.Equal(EnumI8.PositiveShr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveShr"));
			Assert.Equal(EnumI8.NegativeShr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeShr"));
		}

		[Fact]
		public void EnumI8_Mul2()
		{
			Assert.Equal(EnumI8.PositiveMul2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveMul2"));
			Assert.Equal(EnumI8.NegativeMul2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeMul2"));
		}

		[Fact]
		public void EnumI8_Div2()
		{
			Assert.Equal(EnumI8.PositiveDiv2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveDiv2"));
			Assert.Equal(EnumI8.NegativeDiv2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeDiv2"));
		}

		[Fact]
		public void EnumI8_Rem2()
		{
			Assert.Equal(EnumI8.PositiveRem2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveRem2"));
			Assert.Equal(EnumI8.NegativeRem2(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeRem2"));
		}

		[Fact]
		public void EnumI8_AssignPlusOne()
		{
			Assert.Equal(EnumI8.PositiveAssignPlusOne(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveAssignPlusOne"));
			Assert.Equal(EnumI8.NegativeAssignPlusOne(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI8_AssignMinusOne()
		{
			Assert.Equal(EnumI8.PositiveAssignMinusOne(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveAssignMinusOne"));
			Assert.Equal(EnumI8.NegativeAssignMinusOne(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI8_Preincrement()
		{
			Assert.Equal(EnumI8.PositivePreincrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePreincrement"));
			Assert.Equal(EnumI8.NegativePreincrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePreincrement"));
		}

		[Fact]
		public void EnumI8_Predecrement()
		{
			Assert.Equal(EnumI8.PositivePredecrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePredecrement"));
			Assert.Equal(EnumI8.NegativePredecrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePredecrement"));
		}

		[Fact]
		public void EnumI8_Postincrement()
		{
			Assert.Equal(EnumI8.PositivePostincrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePostincrement"));
			Assert.Equal(EnumI8.NegativePostincrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePostincrement"));
		}

		[Fact]
		public void EnumI8_Postdecrement()
		{
			Assert.Equal(EnumI8.PositivePostdecrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositivePostdecrement"));
			Assert.Equal(EnumI8.NegativePostdecrement(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI8_And()
		{
			Assert.Equal(EnumI8.PositiveAnd(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveAnd"));
			Assert.Equal(EnumI8.NegativeAnd(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeAnd"));
		}

		[Fact]
		public void EnumI8_Or()
		{
			Assert.Equal(EnumI8.PositiveOr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveOr"));
			Assert.Equal(EnumI8.NegativeOr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeOr"));
		}

		[Fact]
		public void EnumI8_XOr()
		{
			Assert.Equal(EnumI8.PositiveXOr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.PositiveXOr"));
			Assert.Equal(EnumI8.NegativeXOr(), Run<long>("Mosa.UnitTest.Collection.EnumI8.NegativeXOr"));
		}

		[Fact]
		public void EnumI8_Equal1()
		{
			Assert.Equal(EnumI8.PositiveEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveEqual1"));
			Assert.Equal(EnumI8.NegativeEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeEqual1"));
		}

		[Fact]
		public void EnumI8_Equal2()
		{
			Assert.Equal(EnumI8.PositiveEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveEqual2"));
			Assert.Equal(EnumI8.NegativeEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeEqual2"));
		}

		[Fact]
		public void EnumI8_Equal3()
		{
			Assert.Equal(EnumI8.PositiveEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveEqual3"));
			Assert.Equal(EnumI8.NegativeEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeEqual3"));
		}

		[Fact]
		public void EnumI8_NotEqual1()
		{
			Assert.Equal(EnumI8.PositiveNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveNotEqual1"));
			Assert.Equal(EnumI8.NegativeNotEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI8_NotEqual2()
		{
			Assert.Equal(EnumI8.PositiveNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveNotEqual2"));
			Assert.Equal(EnumI8.NegativeNotEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI8_NotEqual3()
		{
			Assert.Equal(EnumI8.PositiveNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveNotEqual3"));
			Assert.Equal(EnumI8.NegativeNotEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI8_GreaterThan1()
		{
			Assert.Equal(EnumI8.PositiveGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThan1"));
			Assert.Equal(EnumI8.NegativeGreaterThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI8_GreaterThan2()
		{
			Assert.Equal(EnumI8.PositiveGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThan2"));
			Assert.Equal(EnumI8.NegativeGreaterThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI8_GreaterThan3()
		{
			Assert.Equal(EnumI8.PositiveGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThan3"));
			Assert.Equal(EnumI8.NegativeGreaterThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI8_LessThan1()
		{
			Assert.Equal(EnumI8.PositiveLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThan1"));
			Assert.Equal(EnumI8.NegativeLessThan1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI8_LessThan2()
		{
			Assert.Equal(EnumI8.PositiveLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThan2"));
			Assert.Equal(EnumI8.NegativeLessThan2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI8_LessThan3()
		{
			Assert.Equal(EnumI8.PositiveLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThan3"));
			Assert.Equal(EnumI8.NegativeLessThan3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI8_GreaterThanOrEqual1()
		{
			Assert.Equal(EnumI8.PositiveGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThanOrEqual1"));
			Assert.Equal(EnumI8.NegativeGreaterThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI8_GreaterThanOrEqual2()
		{
			Assert.Equal(EnumI8.PositiveGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThanOrEqual2"));
			Assert.Equal(EnumI8.NegativeGreaterThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI8_GreaterThanOrEqual3()
		{
			Assert.Equal(EnumI8.PositiveGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveGreaterThanOrEqual3"));
			Assert.Equal(EnumI8.NegativeGreaterThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI8_LessThanOrEqual1()
		{
			Assert.Equal(EnumI8.PositiveLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThanOrEqual1"));
			Assert.Equal(EnumI8.NegativeLessThanOrEqual1(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI8_LessThanOrEqual2()
		{
			Assert.Equal(EnumI8.PositiveLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThanOrEqual2"));
			Assert.Equal(EnumI8.NegativeLessThanOrEqual2(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI8_LessThanOrEqual3()
		{
			Assert.Equal(EnumI8.PositiveLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.PositiveLessThanOrEqual3"));
			Assert.Equal(EnumI8.NegativeLessThanOrEqual3(), Run<bool>("Mosa.UnitTest.Collection.EnumI8.NegativeLessThanOrEqual3"));
		}
	}
}
