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
	//[Description(@"Tests support for the basic type System.Int64")]
	public class Int64Fixture
	{
		private readonly ArithmeticInstructionTestRunner<long, long> arithmeticTests = new ArithmeticInstructionTestRunner<long, long>
		{
			ExpectedType = "long",
			FirstType = "long",
			SecondType = "long"
		};

		private readonly BinaryLogicInstructionTestRunner<long, long, int> logicTests = new BinaryLogicInstructionTestRunner<long, long, int>
		{
			ExpectedType = "long",
			FirstType = "long",
			SecondType = "long",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<long> comparisonTests = new ComparisonInstructionTestRunner<long>
		{
			FirstType = "long"
		};

		private readonly SZArrayInstructionTestRunner<long> arrayTests = new SZArrayInstructionTestRunner<long>
		{
			FirstType = "long"
		};

		#region Add

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void AddI8I8(long a, long b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void SubI8I8(long a, long b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void MulI8I8(long a, long b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Variations), "I8_I8WithoutZero")]
		public void DivI8I8(long a, long b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Variations), "I8_I8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI8I8DivideByZeroException(long a, long b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Variations), "I8_I8WithoutZero")]
		public void RemI8I8(long a, long b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Variations), "I8_I8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI8I8DivideByZeroException(long a, long b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		//[Test, Factory(typeof(Variations), "I8_I8BelowZero")]
		//[ExpectedException(typeof(OverflowException))]
		//public void RemI8I8OverflowException(long a, long b)
		//{
		//    this.arithmeticTests.Rem((a % b), a, b);
		//}

		#endregion // Rem

		#region Neg

		[Test, Factory(typeof(I8), "Samples")]
		public void NegI8(long first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(I8), "Samples")]
		public void RetI8(long value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void AndI8I8(long first, long second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void OrI8I8(long first, long second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void XorI8I8(long first, long second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		//[Test, Factory(typeof(Variations), "I8_I8UpTo16")]
		//public void ShlI8I8(long first, long second)
		//{
		//    this.logicTests.Shl((first << second), first, second);
		//}

		#endregion // Shl

		#region Shr

		//[Test, Factory(typeof(Variations), "I8_I8UpTo16")]
		//public void ShrI8I8(long first, long second)
		//{
		//    this.logicTests.Shr((first >> second), first, second);
		//}

		#endregion // Shr

		#region Ceq

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CeqI8I8(long first, long second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CgtI8I8(long first, long second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CltI8I8(long first, long second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CgeI8I8(long first, long second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CleI8I8(long first, long second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void NewarrI8()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void LdlenI8(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void StelemI8(int index, long value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void LdelemI8(int index, long value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void LdelemaI8(int index, long value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
