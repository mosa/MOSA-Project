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

using Test.Mosa.Runtime.CompilerFramework.Permutation;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Basic types")]
	[Description(@"Tests support for the basic type System.SByte")]
	public partial class SByteFixture
	{
		private readonly ArithmeticInstructionTestRunner<int, sbyte> arithmeticTests = new ArithmeticInstructionTestRunner<int, sbyte>
		{
			ExpectedType = "int",
			FirstType = "sbyte",
			SecondType = "sbyte"
		};

		private readonly BinaryLogicInstructionTestRunner<int, sbyte, sbyte> logicTests = new BinaryLogicInstructionTestRunner<int, sbyte, sbyte>
		{
			ExpectedType = "int",
			FirstType = "sbyte",
			SecondType = "sbyte",
			ShiftType = "sbyte",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<sbyte> comparisonTests = new ComparisonInstructionTestRunner<sbyte>
		{
			FirstType = "sbyte"
		};

		private readonly SZArrayInstructionTestRunner<sbyte> arrayTests = new SZArrayInstructionTestRunner<sbyte>
		{
			FirstType = "sbyte"
		};

		#region Add

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void AddSbyteSbyte(sbyte a, sbyte b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void SubSbyteSbyte(sbyte a, sbyte b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void MulSbyteSbyte(sbyte a, sbyte b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Numbers), "I1_I1WithoutZero")]
		public void DivSbyteSbyte(sbyte a, sbyte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Numbers), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivSbyteSbyteDivideByZeroException(sbyte a, sbyte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Numbers), "I1_I1AboveZero")]
		public void RemSbyteSbyte(sbyte a, sbyte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Numbers), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemSbyteSbyteDivideByZeroException(sbyte a, sbyte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Numbers), "I1_I1BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemSbyteSbyteOverflowException(sbyte a, sbyte b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region And

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void AndSbyteSbyte(sbyte first, sbyte second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void OrSbyteSbyte(sbyte first, sbyte second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void XorSbyteSbyte(sbyte first, sbyte second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Neg

		[Test, Factory(typeof(I1), "Samples")]
		public void NegSbyte(sbyte first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(I1), "Samples")]
		public void RetSbyte(sbyte value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region Ceq

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void CgtSbyteSbyte(sbyte first, sbyte second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void CltSbyteSbyte(sbyte first, sbyte second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void CgeSbyteSbyte(sbyte first, sbyte second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Numbers), "I1_I1")]
		public void CleSbyteSbyte(sbyte first, sbyte second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrSbyte()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Numbers), "SmallNumbers")]
		public void LdlenSbyte(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Numbers), "ISmall_I1")]
		public void StelemSbyte(int index, sbyte value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Numbers), "ISmall_I1")]
		public void LdelemSbyte(int index, sbyte value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Numbers), "ISmall_I1")]
		public void LdelemaSbyte(int index, sbyte value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
