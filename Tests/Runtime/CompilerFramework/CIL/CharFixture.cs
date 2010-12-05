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

using Test.Mosa.Runtime.CompilerFramework.Numbers;

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Char")]
	public class CharFixture
	{
		private readonly ArithmeticInstructionTestRunner<int, char> arithmeticTests = new ArithmeticInstructionTestRunner<int, char>
		{
			ExpectedType = "int",
			FirstType = "char",
			SecondType = "char",
		};

		private readonly BinaryLogicInstructionTestRunner<int, char, char> logicTests = new BinaryLogicInstructionTestRunner<int, char, char>
		{
			ExpectedType = "int",
			FirstType = "char",
			SecondType = "char",
			ShiftType = "char",
			IncludeComp = false,
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<char> comparisonTests = new ComparisonInstructionTestRunner<char>
		{
			FirstType = "char"
		};

		private readonly SZArrayInstructionTestRunner<char> arrayTests = new SZArrayInstructionTestRunner<char>
		{
			FirstType = "char"
		};


		#region Add

		[Test, Factory(typeof(Variations), "C_C")]
		public void AddCC(char a, char b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "C_C")]
		public void SubCC(char a, char b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "C_C")]
		public void MulCC(char a, char b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "C_CWithoutZero")]
		public void DivCC(char a, char b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "C_CZero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivCCDivideByZeroException(char a, char b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "C_CWithoutZero")]
		public void RemCC(char a, char b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "C_CZero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemCCDivideByZeroException(char a, char b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "C_CBelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemCCOverflowException(char a, char b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Neg

		[Test, Factory(typeof(C), "Samples")]
		public void NegC(char first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(C), "Samples")]
		public void RetC(char value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "C_C")]
		public void AndCC(char first, char second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "C_C")]
		public void OrCC(char first, char second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "C_C")]
		public void XorCC(char first, char second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Test, Factory(typeof(Variations), "C_CUpTo16")]
		public void ShlCC(char first, char second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Test, Factory(typeof(Variations), "C_CUpTo16")]
		public void ShrCC(char first, char second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "C_C")]
		public void CeqCC(char first, char second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "C_C")]
		public void CgtCC(char first, char second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "C_C")]
		public void CltCC(char first, char second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "C_C")]
		public void CgeCC(char first, char second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "C_C")]
		public void CleCC(char first, char second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrC()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenC(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void StelemC(int index, char value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void LdelemC(int index, char value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void LdelemaC(int index, char value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
