// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class EnumFixture : TestFixture
	{
		[Fact]
		public void EnumU1_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveConversion"));
		}
		[Fact]
		public void EnumU1_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePlusOne1"));
		}
		[Fact]
		public void EnumU1_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePlusOne2"));
		}
		[Fact]
		public void EnumU1_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveMinusOne1"));
		}
		[Fact]
		public void EnumU1_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveMinusOne2"));
		}
		[Fact]
		public void EnumU1_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveShl"));
		}
		[Fact]
		public void EnumU1_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveShr"));
		}
		[Fact]
		public void EnumU1_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveMul2"));
		}
		[Fact]
		public void EnumU1_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveDiv2"));
		}
		[Fact]
		public void EnumU1_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveRem2"));
		}
		[Fact]
		public void EnumU1_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveAssignPlusOne"));
		}
		[Fact]
		public void EnumU1_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveAssignMinusOne"));
		}
		[Fact]
		public void EnumU1_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePreincrement"));
		}
		[Fact]
		public void EnumU1_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePredecrement"));
		}
		[Fact]
		public void EnumU1_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePostincrement"));
		}
		[Fact]
		public void EnumU1_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositivePostdecrement"));
		}
		[Fact]
		public void EnumU1_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveAnd"));
		}
		[Fact]
		public void EnumU1_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveOr"));
		}
		[Fact]
		public void EnumU1_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveXOr"));
		}
		[Fact]
		public void EnumU1_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveEqual1"));
		}
		[Fact]
		public void EnumU1_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveEqual2"));
		}
		[Fact]
		public void EnumU1_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveEqual3"));
		}
		[Fact]
		public void EnumU1_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveNotEqual1"));
		}
		[Fact]
		public void EnumU1_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveNotEqual2"));
		}
		[Fact]
		public void EnumU1_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveNotEqual3"));
		}
		[Fact]
		public void EnumU1_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThan1"));
		}
		[Fact]
		public void EnumU1_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThan2"));
		}
		[Fact]
		public void EnumU1_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThan3"));
		}
		[Fact]
		public void EnumU1_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThan1"));
		}
		[Fact]
		public void EnumU1_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThan2"));
		}
		[Fact]
		public void EnumU1_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThan3"));
		}
		[Fact]
		public void EnumU1_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumU1_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumU1_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumU1_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThanOrEqual1"));
		}
		[Fact]
		public void EnumU1_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThanOrEqual2"));
		}
		[Fact]
		public void EnumU1_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU1Class.PositiveLessThanOrEqual3"));
		}
		[Fact]
		public void EnumU2_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveConversion"));
		}
		[Fact]
		public void EnumU2_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePlusOne1"));
		}
		[Fact]
		public void EnumU2_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePlusOne2"));
		}
		[Fact]
		public void EnumU2_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveMinusOne1"));
		}
		[Fact]
		public void EnumU2_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveMinusOne2"));
		}
		[Fact]
		public void EnumU2_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveShl"));
		}
		[Fact]
		public void EnumU2_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveShr"));
		}
		[Fact]
		public void EnumU2_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveMul2"));
		}
		[Fact]
		public void EnumU2_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveDiv2"));
		}
		[Fact]
		public void EnumU2_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveRem2"));
		}
		[Fact]
		public void EnumU2_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveAssignPlusOne"));
		}
		[Fact]
		public void EnumU2_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveAssignMinusOne"));
		}
		[Fact]
		public void EnumU2_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePreincrement"));
		}
		[Fact]
		public void EnumU2_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePredecrement"));
		}
		[Fact]
		public void EnumU2_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePostincrement"));
		}
		[Fact]
		public void EnumU2_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositivePostdecrement"));
		}
		[Fact]
		public void EnumU2_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveAnd"));
		}
		[Fact]
		public void EnumU2_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveOr"));
		}
		[Fact]
		public void EnumU2_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveXOr"));
		}
		[Fact]
		public void EnumU2_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveEqual1"));
		}
		[Fact]
		public void EnumU2_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveEqual2"));
		}
		[Fact]
		public void EnumU2_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveEqual3"));
		}
		[Fact]
		public void EnumU2_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveNotEqual1"));
		}
		[Fact]
		public void EnumU2_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveNotEqual2"));
		}
		[Fact]
		public void EnumU2_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveNotEqual3"));
		}
		[Fact]
		public void EnumU2_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThan1"));
		}
		[Fact]
		public void EnumU2_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThan2"));
		}
		[Fact]
		public void EnumU2_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThan3"));
		}
		[Fact]
		public void EnumU2_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThan1"));
		}
		[Fact]
		public void EnumU2_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThan2"));
		}
		[Fact]
		public void EnumU2_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThan3"));
		}
		[Fact]
		public void EnumU2_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumU2_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumU2_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumU2_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThanOrEqual1"));
		}
		[Fact]
		public void EnumU2_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThanOrEqual2"));
		}
		[Fact]
		public void EnumU2_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU2Class.PositiveLessThanOrEqual3"));
		}
		[Fact]
		public void EnumU4_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveConversion"));
		}
		[Fact]
		public void EnumU4_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePlusOne1"));
		}
		[Fact]
		public void EnumU4_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePlusOne2"));
		}
		[Fact]
		public void EnumU4_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveMinusOne1"));
		}
		[Fact]
		public void EnumU4_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveMinusOne2"));
		}
		[Fact]
		public void EnumU4_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveShl"));
		}
		[Fact]
		public void EnumU4_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveShr"));
		}
		[Fact]
		public void EnumU4_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveMul2"));
		}
		[Fact]
		public void EnumU4_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveDiv2"));
		}
		[Fact]
		public void EnumU4_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveRem2"));
		}
		[Fact]
		public void EnumU4_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveAssignPlusOne"));
		}
		[Fact]
		public void EnumU4_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveAssignMinusOne"));
		}
		[Fact]
		public void EnumU4_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePreincrement"));
		}
		[Fact]
		public void EnumU4_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePredecrement"));
		}
		[Fact]
		public void EnumU4_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePostincrement"));
		}
		[Fact]
		public void EnumU4_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositivePostdecrement"));
		}
		[Fact]
		public void EnumU4_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveAnd"));
		}
		[Fact]
		public void EnumU4_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveOr"));
		}
		[Fact]
		public void EnumU4_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveXOr"));
		}
		[Fact]
		public void EnumU4_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveEqual1"));
		}
		[Fact]
		public void EnumU4_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveEqual2"));
		}
		[Fact]
		public void EnumU4_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveEqual3"));
		}
		[Fact]
		public void EnumU4_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveNotEqual1"));
		}
		[Fact]
		public void EnumU4_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveNotEqual2"));
		}
		[Fact]
		public void EnumU4_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveNotEqual3"));
		}
		[Fact]
		public void EnumU4_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThan1"));
		}
		[Fact]
		public void EnumU4_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThan2"));
		}
		[Fact]
		public void EnumU4_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThan3"));
		}
		[Fact]
		public void EnumU4_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThan1"));
		}
		[Fact]
		public void EnumU4_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThan2"));
		}
		[Fact]
		public void EnumU4_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThan3"));
		}
		[Fact]
		public void EnumU4_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumU4_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumU4_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumU4_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThanOrEqual1"));
		}
		[Fact]
		public void EnumU4_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThanOrEqual2"));
		}
		[Fact]
		public void EnumU4_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU4Class.PositiveLessThanOrEqual3"));
		}
		[Fact]
		public void EnumU8_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveConversion"));
		}
		[Fact]
		public void EnumU8_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePlusOne1"));
		}
		[Fact]
		public void EnumU8_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePlusOne2"));
		}
		[Fact]
		public void EnumU8_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveMinusOne1"));
		}
		[Fact]
		public void EnumU8_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveMinusOne2"));
		}
		[Fact]
		public void EnumU8_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveShl"));
		}
		[Fact]
		public void EnumU8_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveShr"));
		}
		[Fact]
		public void EnumU8_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveMul2"));
		}
		[Fact]
		public void EnumU8_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveDiv2"));
		}
		[Fact]
		public void EnumU8_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveRem2"));
		}
		[Fact]
		public void EnumU8_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveAssignPlusOne"));
		}
		[Fact]
		public void EnumU8_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveAssignMinusOne"));
		}
		[Fact]
		public void EnumU8_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePreincrement"));
		}
		[Fact]
		public void EnumU8_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePredecrement"));
		}
		[Fact]
		public void EnumU8_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePostincrement"));
		}
		[Fact]
		public void EnumU8_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositivePostdecrement"));
		}
		[Fact]
		public void EnumU8_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveAnd"));
		}
		[Fact]
		public void EnumU8_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveOr"));
		}
		[Fact]
		public void EnumU8_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveXOr"));
		}
		[Fact]
		public void EnumU8_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveEqual1"));
		}
		[Fact]
		public void EnumU8_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveEqual2"));
		}
		[Fact]
		public void EnumU8_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveEqual3"));
		}
		[Fact]
		public void EnumU8_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveNotEqual1"));
		}
		[Fact]
		public void EnumU8_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveNotEqual2"));
		}
		[Fact]
		public void EnumU8_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveNotEqual3"));
		}
		[Fact]
		public void EnumU8_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThan1"));
		}
		[Fact]
		public void EnumU8_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThan2"));
		}
		[Fact]
		public void EnumU8_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThan3"));
		}
		[Fact]
		public void EnumU8_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThan1"));
		}
		[Fact]
		public void EnumU8_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThan2"));
		}
		[Fact]
		public void EnumU8_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThan3"));
		}
		[Fact]
		public void EnumU8_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumU8_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumU8_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumU8_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThanOrEqual1"));
		}
		[Fact]
		public void EnumU8_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThanOrEqual2"));
		}
		[Fact]
		public void EnumU8_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumU8Class.PositiveLessThanOrEqual3"));
		}
		[Fact]
		public void EnumI1_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeConversion"));
		}
		[Fact]
		public void EnumI1_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePlusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePlusOne1"));
		}
		[Fact]
		public void EnumI1_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePlusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePlusOne2"));
		}
		[Fact]
		public void EnumI1_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveMinusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeMinusOne1"));
		}
		[Fact]
		public void EnumI1_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveMinusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeMinusOne2"));
		}
		[Fact]
		public void EnumI1_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeShl"));
		}
		[Fact]
		public void EnumI1_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeShr"));
		}
		[Fact]
		public void EnumI1_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeMul2"));
		}
		[Fact]
		public void EnumI1_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeDiv2"));
		}
		[Fact]
		public void EnumI1_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveRem2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeRem2"));
		}
		[Fact]
		public void EnumI1_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeAssignPlusOne"));
		}
		[Fact]
		public void EnumI1_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeAssignMinusOne"));
		}
		[Fact]
		public void EnumI1_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePreincrement"));
		}
		[Fact]
		public void EnumI1_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePredecrement"));
		}
		[Fact]
		public void EnumI1_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePostincrement"));
		}
		[Fact]
		public void EnumI1_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativePostdecrement"));
		}
		[Fact]
		public void EnumI1_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeAnd"));
		}
		[Fact]
		public void EnumI1_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeOr"));
		}
		[Fact]
		public void EnumI1_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeXOr"));
		}
		[Fact]
		public void EnumI1_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeEqual1"));
		}
		[Fact]
		public void EnumI1_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeEqual2"));
		}
		[Fact]
		public void EnumI1_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeEqual3"));
		}
		[Fact]
		public void EnumI1_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeNotEqual1"));
		}
		[Fact]
		public void EnumI1_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeNotEqual2"));
		}
		[Fact]
		public void EnumI1_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeNotEqual3"));
		}
		[Fact]
		public void EnumI1_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThan1"));
		}
		[Fact]
		public void EnumI1_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThan2"));
		}
		[Fact]
		public void EnumI1_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThan3"));
		}
		[Fact]
		public void EnumI1_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThan1"));
		}
		[Fact]
		public void EnumI1_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThan2"));
		}
		[Fact]
		public void EnumI1_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThan3"));
		}
		[Fact]
		public void EnumI1_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumI1_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumI1_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumI1_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThanOrEqual1"));
		}
		[Fact]
		public void EnumI1_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThanOrEqual2"));
		}
		[Fact]
		public void EnumI1_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI1Class.NegativeLessThanOrEqual3"));
		}
		[Fact]
		public void EnumI2_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeConversion"));
		}
		[Fact]
		public void EnumI2_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePlusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePlusOne1"));
		}
		[Fact]
		public void EnumI2_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePlusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePlusOne2"));
		}
		[Fact]
		public void EnumI2_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveMinusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeMinusOne1"));
		}
		[Fact]
		public void EnumI2_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveMinusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeMinusOne2"));
		}
		[Fact]
		public void EnumI2_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeShl"));
		}
		[Fact]
		public void EnumI2_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeShr"));
		}
		[Fact]
		public void EnumI2_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeMul2"));
		}
		[Fact]
		public void EnumI2_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeDiv2"));
		}
		[Fact]
		public void EnumI2_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveRem2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeRem2"));
		}
		[Fact]
		public void EnumI2_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeAssignPlusOne"));
		}
		[Fact]
		public void EnumI2_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeAssignMinusOne"));
		}
		[Fact]
		public void EnumI2_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePreincrement"));
		}
		[Fact]
		public void EnumI2_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePredecrement"));
		}
		[Fact]
		public void EnumI2_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePostincrement"));
		}
		[Fact]
		public void EnumI2_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativePostdecrement"));
		}
		[Fact]
		public void EnumI2_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeAnd"));
		}
		[Fact]
		public void EnumI2_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeOr"));
		}
		[Fact]
		public void EnumI2_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeXOr"));
		}
		[Fact]
		public void EnumI2_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeEqual1"));
		}
		[Fact]
		public void EnumI2_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeEqual2"));
		}
		[Fact]
		public void EnumI2_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeEqual3"));
		}
		[Fact]
		public void EnumI2_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeNotEqual1"));
		}
		[Fact]
		public void EnumI2_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeNotEqual2"));
		}
		[Fact]
		public void EnumI2_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeNotEqual3"));
		}
		[Fact]
		public void EnumI2_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThan1"));
		}
		[Fact]
		public void EnumI2_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThan2"));
		}
		[Fact]
		public void EnumI2_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThan3"));
		}
		[Fact]
		public void EnumI2_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThan1"));
		}
		[Fact]
		public void EnumI2_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThan2"));
		}
		[Fact]
		public void EnumI2_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThan3"));
		}
		[Fact]
		public void EnumI2_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumI2_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumI2_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumI2_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThanOrEqual1"));
		}
		[Fact]
		public void EnumI2_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThanOrEqual2"));
		}
		[Fact]
		public void EnumI2_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI2Class.NegativeLessThanOrEqual3"));
		}
		[Fact]
		public void EnumI4_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeConversion"));
		}
		[Fact]
		public void EnumI4_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePlusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePlusOne1"));
		}
		[Fact]
		public void EnumI4_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePlusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePlusOne2"));
		}
		[Fact]
		public void EnumI4_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveMinusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeMinusOne1"));
		}
		[Fact]
		public void EnumI4_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveMinusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeMinusOne2"));
		}
		[Fact]
		public void EnumI4_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeShl"));
		}
		[Fact]
		public void EnumI4_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeShr"));
		}
		[Fact]
		public void EnumI4_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeMul2"));
		}
		[Fact]
		public void EnumI4_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeDiv2"));
		}
		[Fact]
		public void EnumI4_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveRem2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeRem2"));
		}
		[Fact]
		public void EnumI4_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeAssignPlusOne"));
		}
		[Fact]
		public void EnumI4_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeAssignMinusOne"));
		}
		[Fact]
		public void EnumI4_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePreincrement"));
		}
		[Fact]
		public void EnumI4_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePredecrement"));
		}
		[Fact]
		public void EnumI4_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePostincrement"));
		}
		[Fact]
		public void EnumI4_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativePostdecrement"));
		}
		[Fact]
		public void EnumI4_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeAnd"));
		}
		[Fact]
		public void EnumI4_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeOr"));
		}
		[Fact]
		public void EnumI4_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeXOr"));
		}
		[Fact]
		public void EnumI4_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeEqual1"));
		}
		[Fact]
		public void EnumI4_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeEqual2"));
		}
		[Fact]
		public void EnumI4_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeEqual3"));
		}
		[Fact]
		public void EnumI4_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeNotEqual1"));
		}
		[Fact]
		public void EnumI4_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeNotEqual2"));
		}
		[Fact]
		public void EnumI4_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeNotEqual3"));
		}
		[Fact]
		public void EnumI4_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThan1"));
		}
		[Fact]
		public void EnumI4_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThan2"));
		}
		[Fact]
		public void EnumI4_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThan3"));
		}
		[Fact]
		public void EnumI4_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThan1"));
		}
		[Fact]
		public void EnumI4_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThan2"));
		}
		[Fact]
		public void EnumI4_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThan3"));
		}
		[Fact]
		public void EnumI4_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumI4_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumI4_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumI4_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThanOrEqual1"));
		}
		[Fact]
		public void EnumI4_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThanOrEqual2"));
		}
		[Fact]
		public void EnumI4_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI4Class.NegativeLessThanOrEqual3"));
		}
		[Fact]
		public void EnumI8_Conversion()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeConversion"));
		}
		[Fact]
		public void EnumI8_PlusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePlusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePlusOne1"));
		}
		[Fact]
		public void EnumI8_PlusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePlusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePlusOne2"));
		}
		[Fact]
		public void EnumI8_MinusOne1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveMinusOne1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeMinusOne1"));
		}
		[Fact]
		public void EnumI8_MinusOne2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveMinusOne2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeMinusOne2"));
		}
		[Fact]
		public void EnumI8_Shl()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeShl"));
		}
		[Fact]
		public void EnumI8_Shr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeShr"));
		}
		[Fact]
		public void EnumI8_Mul2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeMul2"));
		}
		[Fact]
		public void EnumI8_Div2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeDiv2"));
		}
		[Fact]
		public void EnumI8_Rem2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveRem2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeRem2"));
		}
		[Fact]
		public void EnumI8_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeAssignPlusOne"));
		}
		[Fact]
		public void EnumI8_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeAssignMinusOne"));
		}
		[Fact]
		public void EnumI8_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePreincrement"));
		}
		[Fact]
		public void EnumI8_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePredecrement"));
		}
		[Fact]
		public void EnumI8_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePostincrement"));
		}
		[Fact]
		public void EnumI8_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativePostdecrement"));
		}
		[Fact]
		public void EnumI8_And()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeAnd"));
		}
		[Fact]
		public void EnumI8_Or()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeOr"));
		}
		[Fact]
		public void EnumI8_XOr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeXOr"));
		}
		[Fact]
		public void EnumI8_Equal1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeEqual1"));
		}
		[Fact]
		public void EnumI8_Equal2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeEqual2"));
		}
		[Fact]
		public void EnumI8_Equal3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeEqual3"));
		}
		[Fact]
		public void EnumI8_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeNotEqual1"));
		}
		[Fact]
		public void EnumI8_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeNotEqual2"));
		}
		[Fact]
		public void EnumI8_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeNotEqual3"));
		}
		[Fact]
		public void EnumI8_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThan1"));
		}
		[Fact]
		public void EnumI8_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThan2"));
		}
		[Fact]
		public void EnumI8_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThan3"));
		}
		[Fact]
		public void EnumI8_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThan1"));
		}
		[Fact]
		public void EnumI8_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThan2"));
		}
		[Fact]
		public void EnumI8_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThan3"));
		}
		[Fact]
		public void EnumI8_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThanOrEqual1"));
		}
		[Fact]
		public void EnumI8_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThanOrEqual2"));
		}
		[Fact]
		public void EnumI8_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeGreaterThanOrEqual3"));
		}
		[Fact]
		public void EnumI8_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThanOrEqual1"));
		}
		[Fact]
		public void EnumI8_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThanOrEqual2"));
		}
		[Fact]
		public void EnumI8_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.EnumI8Class.NegativeLessThanOrEqual3"));
		}
	}
}
