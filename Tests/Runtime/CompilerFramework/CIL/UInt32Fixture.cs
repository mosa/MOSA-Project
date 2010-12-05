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
	//[Description(@"Tests support for the basic type System.UInt32")]
	public partial class UInt32Fixture
	{
		private readonly ArithmeticInstructionTestRunner<uint, uint> arithmeticTests = new ArithmeticInstructionTestRunner<uint, uint>
		{
			ExpectedType = "uint",
			FirstType = "uint",
			SecondType = "uint"
		};

		private readonly BinaryLogicInstructionTestRunner<uint, uint, int> logicTests = new BinaryLogicInstructionTestRunner<uint, uint, int>
		{
			ExpectedType = "uint",
			FirstType = "uint",
			SecondType = "uint",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<uint> comparisonTests = new ComparisonInstructionTestRunner<uint>
		{
			FirstType = "uint"
		};

		private readonly SZArrayInstructionTestRunner<uint> arrayTests = new SZArrayInstructionTestRunner<uint>
		{
			FirstType = "uint"
		};

		#region Add

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void AddU4U4(uint a, uint b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void SubU4U4(uint a, uint b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void MulU4U4(uint a, uint b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "U4_U4WithoutZero")]
		public void DivU4U4(uint a, uint b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "U4_U4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU4U4DivideByZeroException(uint a, uint b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "U4_U4WithoutZero")]
		public void RemU4U4(uint a, uint b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "U4_U4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU4U4DivideByZeroException(uint a, uint b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "U4_U4BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemU4U4OverflowException(uint a, uint b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Ret

		[Test, Factory(typeof(U4), "Samples")]
		public void RetU4(uint value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void AndU4U4(uint first, uint second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void OrU4U4(uint first, uint second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void XorU4U4(uint first, uint second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		//[Test, Factory(typeof(Variations), "U4_U4UpTo16")]
		//public void ShlU4U4(uint first, uint second)
		//{
		//    this.logicTests.Shl((first << second), first, second);
		//}

		#endregion // Shl

		#region Shr

		//[Test, Factory(typeof(Variations), "U4_U4UpTo16")]
		//public void ShrU4U4(uint first, uint second)
		//{
		//    this.logicTests.Shr((first >> second), first, second);
		//}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CeqU4U4(uint first, uint second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CgtU4U4(uint first, uint second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CltU4U4(uint first, uint second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CgeU4U4(uint first, uint second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CleU4U4(uint first, uint second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrU4()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenU4(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void StelemU4(int index, uint value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void LdelemU4(int index, uint value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void LdelemaU4(int index, uint value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
