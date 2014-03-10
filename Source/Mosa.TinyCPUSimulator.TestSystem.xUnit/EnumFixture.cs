/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Andrey Turkin <andrey.turkin@gmail.com>
 *
 */

 



using Xunit;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class EnumFixture : TestFixture
	{

		[Fact]
		public void EnumU1_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveConversion"));
		}

		[Fact]
		public void EnumU1_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePlusOne_1"));
		}

		[Fact]
		public void EnumU1_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePlusOne_2"));
		}

		[Fact]
		public void EnumU1_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveMinusOne_1"));
		}

		[Fact]
		public void EnumU1_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveMinusOne_2"));
		}

		[Fact]
		public void EnumU1_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveShl"));
		}

		[Fact]
		public void EnumU1_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveShr"));
		}

		[Fact]
		public void EnumU1_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveMul2"));
		}

		[Fact]
		public void EnumU1_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveDiv2"));
		}

		[Fact]
		public void EnumU1_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU1_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU1_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePreincrement"));
		}

		[Fact]
		public void EnumU1_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePredecrement"));
		}

		[Fact]
		public void EnumU1_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePostincrement"));
		}

		[Fact]
		public void EnumU1_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU1_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveAnd"));
		}

		[Fact]
		public void EnumU1_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveOr"));
		}

		[Fact]
		public void EnumU1_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveXOr"));
		}

		[Fact]
		public void EnumU1_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveEqual1"));
		}

		[Fact]
		public void EnumU1_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveEqual2"));
		}

		[Fact]
		public void EnumU1_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveEqual3"));
		}

		[Fact]
		public void EnumU1_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU1_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU1_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU1_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU1_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU1_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU1_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU1_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU1_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU1_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU1_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU1Class.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumU2_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveConversion"));
		}

		[Fact]
		public void EnumU2_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePlusOne_1"));
		}

		[Fact]
		public void EnumU2_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePlusOne_2"));
		}

		[Fact]
		public void EnumU2_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveMinusOne_1"));
		}

		[Fact]
		public void EnumU2_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveMinusOne_2"));
		}

		[Fact]
		public void EnumU2_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveShl"));
		}

		[Fact]
		public void EnumU2_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveShr"));
		}

		[Fact]
		public void EnumU2_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveMul2"));
		}

		[Fact]
		public void EnumU2_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveDiv2"));
		}

		[Fact]
		public void EnumU2_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU2_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU2_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePreincrement"));
		}

		[Fact]
		public void EnumU2_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePredecrement"));
		}

		[Fact]
		public void EnumU2_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePostincrement"));
		}

		[Fact]
		public void EnumU2_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU2_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveAnd"));
		}

		[Fact]
		public void EnumU2_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveOr"));
		}

		[Fact]
		public void EnumU2_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveXOr"));
		}

		[Fact]
		public void EnumU2_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveEqual1"));
		}

		[Fact]
		public void EnumU2_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveEqual2"));
		}

		[Fact]
		public void EnumU2_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveEqual3"));
		}

		[Fact]
		public void EnumU2_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU2_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU2_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU2_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU2_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU2_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU2_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU2_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU2_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU2_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU2_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU2Class.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumU4_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveConversion"));
		}

		[Fact]
		public void EnumU4_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePlusOne_1"));
		}

		[Fact]
		public void EnumU4_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePlusOne_2"));
		}

		[Fact]
		public void EnumU4_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveMinusOne_1"));
		}

		[Fact]
		public void EnumU4_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveMinusOne_2"));
		}

		[Fact]
		public void EnumU4_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveShl"));
		}

		[Fact]
		public void EnumU4_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveShr"));
		}

		[Fact]
		public void EnumU4_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveMul2"));
		}

		[Fact]
		public void EnumU4_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveDiv2"));
		}

		[Fact]
		public void EnumU4_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveAssignPlusOne"));
		}

		[Fact]
		public void EnumU4_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveAssignMinusOne"));
		}

		[Fact]
		public void EnumU4_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePreincrement"));
		}

		[Fact]
		public void EnumU4_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePredecrement"));
		}

		[Fact]
		public void EnumU4_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePostincrement"));
		}

		[Fact]
		public void EnumU4_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositivePostdecrement"));
		}

		[Fact]
		public void EnumU4_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveAnd"));
		}

		[Fact]
		public void EnumU4_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveOr"));
		}

		[Fact]
		public void EnumU4_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveXOr"));
		}

		[Fact]
		public void EnumU4_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveEqual1"));
		}

		[Fact]
		public void EnumU4_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveEqual2"));
		}

		[Fact]
		public void EnumU4_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveEqual3"));
		}

		[Fact]
		public void EnumU4_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveNotEqual1"));
		}

		[Fact]
		public void EnumU4_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveNotEqual2"));
		}

		[Fact]
		public void EnumU4_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveNotEqual3"));
		}

		[Fact]
		public void EnumU4_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThan1"));
		}

		[Fact]
		public void EnumU4_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThan2"));
		}

		[Fact]
		public void EnumU4_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThan3"));
		}

		[Fact]
		public void EnumU4_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThan1"));
		}

		[Fact]
		public void EnumU4_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThan2"));
		}

		[Fact]
		public void EnumU4_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThan3"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumU4_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThanOrEqual1"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThanOrEqual2"));
		}

		[Fact]
		public void EnumU4_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumU4Class.PositiveLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI1_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeConversion"));
		}

		[Fact]
		public void EnumI1_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePlusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePlusOne_1"));
		}

		[Fact]
		public void EnumI1_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePlusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePlusOne_2"));
		}

		[Fact]
		public void EnumI1_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveMinusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeMinusOne_1"));
		}

		[Fact]
		public void EnumI1_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveMinusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeMinusOne_2"));
		}

		[Fact]
		public void EnumI1_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeShl"));
		}

		[Fact]
		public void EnumI1_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeShr"));
		}

		[Fact]
		public void EnumI1_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeMul2"));
		}

		[Fact]
		public void EnumI1_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeDiv2"));
		}

		[Fact]
		public void EnumI1_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI1_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI1_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePreincrement"));
		}

		[Fact]
		public void EnumI1_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePredecrement"));
		}

		[Fact]
		public void EnumI1_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePostincrement"));
		}

		[Fact]
		public void EnumI1_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI1_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeAnd"));
		}

		[Fact]
		public void EnumI1_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeOr"));
		}

		[Fact]
		public void EnumI1_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeXOr"));
		}

		[Fact]
		public void EnumI1_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeEqual1"));
		}

		[Fact]
		public void EnumI1_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeEqual2"));
		}

		[Fact]
		public void EnumI1_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeEqual3"));
		}

		[Fact]
		public void EnumI1_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI1_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI1_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI1_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI1_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI1_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI1_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI1_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI1_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI1_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI1_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI1Class.NegativeLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI2_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeConversion"));
		}

		[Fact]
		public void EnumI2_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePlusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePlusOne_1"));
		}

		[Fact]
		public void EnumI2_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePlusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePlusOne_2"));
		}

		[Fact]
		public void EnumI2_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveMinusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeMinusOne_1"));
		}

		[Fact]
		public void EnumI2_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveMinusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeMinusOne_2"));
		}

		[Fact]
		public void EnumI2_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeShl"));
		}

		[Fact]
		public void EnumI2_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeShr"));
		}

		[Fact]
		public void EnumI2_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeMul2"));
		}

		[Fact]
		public void EnumI2_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeDiv2"));
		}

		[Fact]
		public void EnumI2_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI2_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI2_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePreincrement"));
		}

		[Fact]
		public void EnumI2_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePredecrement"));
		}

		[Fact]
		public void EnumI2_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePostincrement"));
		}

		[Fact]
		public void EnumI2_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI2_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeAnd"));
		}

		[Fact]
		public void EnumI2_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeOr"));
		}

		[Fact]
		public void EnumI2_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeXOr"));
		}

		[Fact]
		public void EnumI2_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeEqual1"));
		}

		[Fact]
		public void EnumI2_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeEqual2"));
		}

		[Fact]
		public void EnumI2_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeEqual3"));
		}

		[Fact]
		public void EnumI2_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI2_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI2_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI2_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI2_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI2_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI2_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI2_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI2_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI2_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI2_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI2Class.NegativeLessThanOrEqual3"));
		}

		[Fact]
		public void EnumI4_Conversion()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveConversion"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeConversion"));
		}

		[Fact]
		public void EnumI4_PlusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePlusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePlusOne_1"));
		}

		[Fact]
		public void EnumI4_PlusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePlusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePlusOne_2"));
		}

		[Fact]
		public void EnumI4_MinusOne_1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveMinusOne_1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeMinusOne_1"));
		}

		[Fact]
		public void EnumI4_MinusOne_2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveMinusOne_2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeMinusOne_2"));
		}

		[Fact]
		public void EnumI4_Shl()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveShl"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeShl"));
		}

		[Fact]
		public void EnumI4_Shr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveShr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeShr"));
		}

		[Fact]
		public void EnumI4_Mul2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveMul2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeMul2"));
		}

		[Fact]
		public void EnumI4_Div2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveDiv2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeDiv2"));
		}

		[Fact]
		public void EnumI4_AssignPlusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveAssignPlusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeAssignPlusOne"));
		}

		[Fact]
		public void EnumI4_AssignMinusOne()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveAssignMinusOne"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeAssignMinusOne"));
		}

		[Fact]
		public void EnumI4_Preincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePreincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePreincrement"));
		}

		[Fact]
		public void EnumI4_Predecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePredecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePredecrement"));
		}

		[Fact]
		public void EnumI4_Postincrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePostincrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePostincrement"));
		}

		[Fact]
		public void EnumI4_Postdecrement()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositivePostdecrement"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativePostdecrement"));
		}

		[Fact]
		public void EnumI4_And()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveAnd"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeAnd"));
		}

		[Fact]
		public void EnumI4_Or()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeOr"));
		}

		[Fact]
		public void EnumI4_XOr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveXOr"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeXOr"));
		}

		[Fact]
		public void EnumI4_Equal1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeEqual1"));
		}

		[Fact]
		public void EnumI4_Equal2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeEqual2"));
		}

		[Fact]
		public void EnumI4_Equal3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeEqual3"));
		}

		[Fact]
		public void EnumI4_NotEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveNotEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeNotEqual1"));
		}

		[Fact]
		public void EnumI4_NotEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveNotEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeNotEqual2"));
		}

		[Fact]
		public void EnumI4_NotEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveNotEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeNotEqual3"));
		}

		[Fact]
		public void EnumI4_GreaterThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThan1"));
		}

		[Fact]
		public void EnumI4_GreaterThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThan2"));
		}

		[Fact]
		public void EnumI4_GreaterThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThan3"));
		}

		[Fact]
		public void EnumI4_LessThan1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThan1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThan1"));
		}

		[Fact]
		public void EnumI4_LessThan2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThan2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThan2"));
		}

		[Fact]
		public void EnumI4_LessThan3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThan3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThan3"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThanOrEqual1"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThanOrEqual2"));
		}

		[Fact]
		public void EnumI4_GreaterThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveGreaterThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeGreaterThanOrEqual3"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThanOrEqual1"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThanOrEqual1"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThanOrEqual2"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThanOrEqual2"));
		}

		[Fact]
		public void EnumI4_LessThanOrEqual3()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.PositiveLessThanOrEqual3"));
			Assert.True(Run<bool>("Mosa.Test.Collection.TestEnumI4Class.NegativeLessThanOrEqual3"));
		}
	}
}
