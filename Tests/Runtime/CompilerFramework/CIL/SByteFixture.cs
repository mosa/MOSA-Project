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

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.SByte")]
	public class SByteFixture
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

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AddI1I1(sbyte a, sbyte b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void SubI1I1(sbyte a, sbyte b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void MulI1I1(sbyte a, sbyte b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "I1_I1WithoutZero")]
		public void DivI1I1(sbyte a, sbyte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI1I1DivideByZeroException(sbyte a, sbyte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "I1_I1AboveZero")]
		public void RemI1I1(sbyte a, sbyte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI1I1DivideByZeroException(sbyte a, sbyte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "I1_I1BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemI1I1OverflowException(sbyte a, sbyte b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Neg

		[Test, Factory(typeof(I1), "Samples")]
		public void NegI1(sbyte first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(I1), "Samples")]
		public void RetI1(sbyte value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AndI1I1(sbyte first, sbyte second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void OrI1I1(sbyte first, sbyte second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void XorI1I1(sbyte first, sbyte second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Ceq

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgtI1I1(sbyte first, sbyte second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CltI1I1(sbyte first, sbyte second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgeI1I1(sbyte first, sbyte second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CleI1I1(sbyte first, sbyte second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrI1()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenI1(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void StelemI1(int index, sbyte value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemI1(int index, sbyte value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemaI1(int index, sbyte value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
