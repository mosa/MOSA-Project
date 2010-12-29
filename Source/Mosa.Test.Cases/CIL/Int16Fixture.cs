/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Int16")]
	public class Int16Fixture
	{
		private readonly ArithmeticInstructionTestRunner<int, short> arithmeticTests = new ArithmeticInstructionTestRunner<int, short>
		{
			ExpectedType = "int",
			FirstType = @"short",
			SecondType = @"short"
		};

		private readonly BinaryLogicInstructionTestRunner<int, short, short> logicTests = new BinaryLogicInstructionTestRunner<int, short, short>
		{
			ExpectedType = "int",
			FirstType = @"short",
			SecondType = @"short",
			ShiftType = @"short",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<short> comparisonTests = new ComparisonInstructionTestRunner<short>
		{
			FirstType = @"short"
		};

		private readonly SZArrayInstructionTestRunner<short> arrayTests = new SZArrayInstructionTestRunner<short>
		{
			FirstType = @"short"
		};

		#region Add

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AddI2I2(short a, short b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void SubI2I2(short a, short b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void MulI2I2(short a, short b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "I2_I2WithoutZero")]
		public void DivI2I2(short a, short b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI2I2DivideByZeroException(short a, short b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "I2_I2AboveZero")]
		public void RemI2I2(short a, short b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI2I2DivideByZeroException(short a, short b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "I2_I2BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemI2I2OverflowException(short a, short b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Neg

		[Test, Factory(typeof(I2), "Samples")]
		public void NegI2(short first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(I2), "Samples")]
		public void RetI2(short value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AndI2I2(short first, short second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void OrI2I2(short first, short second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void XorI2I2(short first, short second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Test, Factory(typeof(Variations), "I2_I2UpTo16")]
		public void ShlI2I2(short first, short second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Test, Factory(typeof(Variations), "I2_I2UpTo16")]
		public void ShrI2I2(short first, short second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CeqI2I2(short first, short second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgtI2I2(short first, short second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CltI2I2(short first, short second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgeI2I2(short first, short second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CleI2I2(short first, short second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrI2()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenI2(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_I2")]
		public void StelemI2(int index, short value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_I2")]
		public void LdelemI2(int index, short value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_I2")]
		public void LdelemaI2(int index, short value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
