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
	//[Description(@"Tests support for the basic type System.UInt16")]
	public class UInt16Fixture
	{
		private readonly ArithmeticInstructionTestRunner<int, ushort> arithmeticTests = new ArithmeticInstructionTestRunner<int, ushort>
		{
			ExpectedType = "int",
			FirstType = "ushort",
			SecondType = "ushort"
		};

		private readonly BinaryLogicInstructionTestRunner<int, ushort, ushort> logicTests = new BinaryLogicInstructionTestRunner<int, ushort, ushort>
		{
			ExpectedType = "int",
			FirstType = "ushort",
			SecondType = "ushort",
			ShiftType = "ushort",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<ushort> comparisonTests = new ComparisonInstructionTestRunner<ushort>
		{
			FirstType = "ushort"
		};

		private readonly SZArrayInstructionTestRunner<ushort> arrayTests = new SZArrayInstructionTestRunner<ushort>
		{
			FirstType = "ushort"
		};

		#region Add

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AddU2U2(ushort a, ushort b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void SubU2U2(ushort a, ushort b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void MulU2U2(ushort a, ushort b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void DivU2U2(ushort a, ushort b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException(ushort a, ushort b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void RemU2U2(ushort a, ushort b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException(ushort a, ushort b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "U2_U2BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemU2U2OverflowException(ushort a, ushort b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Ret

		[Test, Factory(typeof(U2), "Samples")]
		public void RetU2(ushort value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AndU2U2(ushort first, ushort second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void OrU2U2(ushort first, ushort second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void XorU2U2(ushort first, ushort second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		//[Test, Factory(typeof(Variations), "U2_U2UpTo16")]
		//public void ShlU2U2(ushort first, ushort second)
		//{
		//    this.logicTests.Shl((first << second), first, second);
		//}

		#endregion // Shl

		#region Shr

		//[Test, Factory(typeof(Variations), "U2_U2UpTo16")]
		//public void ShrU2U2(ushort first, ushort second)
		//{
		//    this.logicTests.Shr((first >> second), first, second);
		//}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CeqU2U2(ushort first, ushort second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgtU2U2(ushort first, ushort second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CltU2U2(ushort first, ushort second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgeU2U2(ushort first, ushort second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CleU2U2(ushort first, ushort second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrU2()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenU2(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void StelemU2(int index, ushort value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemU2(int index, ushort value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemaU2(int index, ushort value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
