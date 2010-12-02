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
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Byte")]
	public partial class ByteFixture
	{
		private readonly ArithmeticInstructionTestRunner<int, byte> arithmeticTests = new ArithmeticInstructionTestRunner<int, byte>
		{
			ExpectedType = "int",
			FirstType = "byte",
			SecondType = "byte",
		};

		private readonly BinaryLogicInstructionTestRunner<int, byte, byte> logicTests = new BinaryLogicInstructionTestRunner<int, byte, byte>
		{
			ExpectedType = "int",
			FirstType = "byte",
			SecondType = "byte",
			ShiftType = "byte",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<byte> comparisonTests = new ComparisonInstructionTestRunner<byte>
		{
			FirstType = "byte"
		};

		private readonly SZArrayInstructionTestRunner<byte> arrayTests = new SZArrayInstructionTestRunner<byte>
		{
			FirstType = "byte"
		};

		#region Add

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void AddU1U1(byte a, byte b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void SubU1U1(byte a, byte b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void MulU1U1(byte a, byte b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Test, Factory(typeof(Numbers), "U1_U1WithoutZero")]
		public void DivU1U1(byte a, byte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		[Test, Factory(typeof(Numbers), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(byte a, byte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Test, Factory(typeof(Numbers), "U1_U1WithoutZero")]
		public void RemU1U1(byte a, byte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		[Test, Factory(typeof(Numbers), "U1_U1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(byte a, byte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}
		
		#endregion // Rem

		#region And

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void AndU1U1(byte first, byte second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void OrU1U1(byte first, byte second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void XorU1U1(byte first, byte second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Neg

		[Test, Factory(typeof(U1), "Samples")]
		public void NegU1(byte first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion // Neg

		#region Ret

		[Test, Factory(typeof(U1), "Samples")]
		public void RetU1(byte value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region Ceq

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void CgtU1U1(byte first, byte second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void CltU1U1(byte first, byte second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void CgeU1U1(byte first, byte second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Test, Factory(typeof(Numbers), "U1_U1")]
		public void CleU1U1(byte first, byte second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test]
		public void Newarr()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Test, Factory(typeof(Numbers), "SmallNumbers")]
		public void Ldlen(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Test, Factory(typeof(Numbers), "ISmall_U1")]
		public void StelemU1(int index, byte value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Test, Factory(typeof(Numbers), "ISmall_U1")]
		public void LdelemU1(int index, byte value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Test, Factory(typeof(Numbers), "ISmall_U1")]
		public void LdelemaU1(int index, byte value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
