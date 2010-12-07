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

namespace Mosa.Test.Runtime.CompilerFramework.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Int32")]
	public class Int32Fixture
	{
		private readonly ArithmeticInstructionTestRunner<int, int> arithmeticTests = new ArithmeticInstructionTestRunner<int, int>
		{
			ExpectedType = "int",
			FirstType = "int",
			SecondType = "int"
		};

		private readonly BinaryLogicInstructionTestRunner<int, int, int> logicTests = new BinaryLogicInstructionTestRunner<int, int, int>
		{
			ExpectedType = "int",
			FirstType = "int",
			SecondType = "int",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<int> comparisonTests = new ComparisonInstructionTestRunner<int>
		{
			FirstType = "int"
		};

		private readonly SZArrayInstructionTestRunner<int> arrayTests = new SZArrayInstructionTestRunner<int>
		{
			FirstType = "int"
		};


		#region Add

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void AddI4I4(int a, int b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void SubI4I4(int a, int b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void MulI4I4(int a, int b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "I4_I4WithoutZero")]
		public void DivI4I4(int a, int b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "I4_I4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI4I4DivideByZeroException(int a, int b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "I4_I4AboveZero")]
		public void RemI4I4(int a, int b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "I4_I4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a, int b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "I4_I4BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemI4I4OverflowException(int a, int b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Neg

		[Test, Factory(typeof(I4), "Samples")]
		public void NegI4(int first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(I4), "Samples")]
		public void RetI4(int value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void AndI4I4(int first, int second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void OrI4I4(int first, int second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void XorI4I4(int first, int second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Test, Factory(typeof(Variations), "I4_I4UpTo32")]
		public void ShlI4I4(int first, int second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Test, Factory(typeof(Variations), "I4_I4UpTo32")]
		public void ShrI4I4(int first, int second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CeqI4I4(int first, int second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CgtI4I4(int first, int second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CltI4I4(int first, int second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CgeI4I4(int first, int second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CleI4I4(int first, int second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrI4()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenI4(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void StelemI4(int index, int value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void LdelemI4(int index, int value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void LdelemaI4(int index, int value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
