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
	//[Description(@"Tests support for the basic type System.UInt64")]
	public class UInt64Fixture
	{
		private readonly ArithmeticInstructionTestRunner<ulong, ulong> arithmeticTests = new ArithmeticInstructionTestRunner<ulong, ulong>
		{
			ExpectedType = "ulong",
			FirstType = "ulong",
			SecondType = "ulong",
			IncludeNeg = false
		};

		private readonly BinaryLogicInstructionTestRunner<ulong, ulong, int> logicTests = new BinaryLogicInstructionTestRunner<ulong, ulong, int>
		{
			ExpectedType = "ulong",
			FirstType = "ulong",
			SecondType = "ulong",
			ShiftType = "int",
			IncludeNot = false,
		};

		private readonly ComparisonInstructionTestRunner<ulong> comparisonTests = new ComparisonInstructionTestRunner<ulong>
		{
			FirstType = "ulong"
		};

		private readonly SZArrayInstructionTestRunner<ulong> arrayTests = new SZArrayInstructionTestRunner<ulong>
		{
			FirstType = "ulong"
		};

		#region Add

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void AddU8U8(ulong a, ulong b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void SubU8U8(ulong a, ulong b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void MulU8U8(ulong a, ulong b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "U8_U8WithoutZero")]
		public void DivU8U8(ulong a, ulong b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "U8_U8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU8U8DivideByZeroException(ulong a, ulong b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "U8_U8WithoutZero")]
		public void RemU8U8(ulong a, ulong b)
		{
			//[Row(UInt64.MaxValue - 1, UInt64.MaxValue)] // Crashes test runner
			if ((a == UInt64.MaxValue - 1) && (b == UInt64.MaxValue))
				return;

			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "U8_U8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU8U8DivideByZeroException(ulong a, ulong b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "U8_U8BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemU8U8OverflowException(ulong a, ulong b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Ret

		[Test, Factory(typeof(U8), "Samples")]
		public void RetU8(ulong value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void AndU8U8(ulong first, ulong second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void OrU8U8(ulong first, ulong second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void XorU8U8(ulong first, ulong second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		//[Test, Factory(typeof(Variations), "U8_U8UpTo16")]
		//public void ShlU8U8(ulong first, ulong second)
		//{
		//    this.logicTests.Shl((first << second), first, second);
		//}

		#endregion // Shl

		#region Shr

		//[Test, Factory(typeof(Variations), "U8_U8UpTo16")]
		//public void ShrU8U8(ulong first, ulong second)
		//{
		//    this.logicTests.Shr((first >> second), first, second);
		//}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CeqU8U8(ulong first, ulong second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CgtU8U8(ulong first, ulong second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CltU8U8(ulong first, ulong second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CgeU8U8(ulong first, ulong second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "U8_U8")]
		public void CleU8U8(ulong first, ulong second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrU8()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenU8(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void StelemU8(int index, ulong value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void LdelemU8(int index, ulong value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_U8")]
		public void LdelemaU8(int index, ulong value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
