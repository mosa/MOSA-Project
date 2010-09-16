/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

using System;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	[TestFixture]
	public class ByteFixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, byte> arithmeticTests = new ArithmeticInstructionTestRunner<int, byte>
		{
			ExpectedTypeName = @"int",
			TypeName = @"byte"
		};

		private readonly BinaryLogicInstructionTestRunner<int, byte, byte> logicTests = new BinaryLogicInstructionTestRunner<int, byte, byte>
		{
			ExpectedTypeName = @"int",
			TypeName = @"byte",
			ShiftTypeName = @"byte",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<byte> comparisonTests = new ComparisonInstructionTestRunner<byte>
		{
			TypeName = @"byte"
		};

		private readonly SZArrayInstructionTestRunner<byte> arrayTests = new SZArrayInstructionTestRunner<byte>
		{
			TypeName = @"byte"
		};

		#region Add

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Add(byte a, byte b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Sub(byte a, byte b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Mul(byte a, byte b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Div(byte a, byte b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Rem(byte a, byte b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		#endregion // Rem

		#region Ret

		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(byte.MaxValue)]
		[Row(byte.MaxValue - 1)]
		[Row(17)]
		[Row(123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Ret(byte value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void And(byte first, byte second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Or(byte first, byte second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Xor(byte first, byte second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, 3)]
		[Row(0, 4)]
		[Row(0, 5)]
		[Row(0, 6)]
		[Row(0, 7)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, 3)]
		[Row(2, 4)]
		[Row(2, 5)]
		[Row(2, 6)]
		[Row(2, 7)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, 3)]
		[Row(byte.MaxValue, 4)]
		[Row(byte.MaxValue, 5)]
		[Row(byte.MaxValue, 6)]
		[Row(byte.MaxValue, 7)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, 3)]
		[Row(byte.MaxValue - 1, 4)]
		[Row(byte.MaxValue - 1, 5)]
		[Row(byte.MaxValue - 1, 6)]
		[Row(byte.MaxValue - 1, 7)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, 3)]
		[Row(17, 4)]
		[Row(17, 5)]
		[Row(17, 6)]
		[Row(17, 7)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, 3)]
		[Row(123, 4)]
		[Row(123, 5)]
		[Row(123, 6)]
		[Row(123, 7)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Shl(byte first, byte second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, 3)]
		[Row(0, 4)]
		[Row(0, 5)]
		[Row(0, 6)]
		[Row(0, 7)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, 3)]
		[Row(2, 4)]
		[Row(2, 5)]
		[Row(2, 6)]
		[Row(2, 7)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, 3)]
		[Row(byte.MaxValue, 4)]
		[Row(byte.MaxValue, 5)]
		[Row(byte.MaxValue, 6)]
		[Row(byte.MaxValue, 7)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, 3)]
		[Row(byte.MaxValue - 1, 4)]
		[Row(byte.MaxValue - 1, 5)]
		[Row(byte.MaxValue - 1, 6)]
		[Row(byte.MaxValue - 1, 7)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, 3)]
		[Row(17, 4)]
		[Row(17, 5)]
		[Row(17, 6)]
		[Row(17, 7)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, 3)]
		[Row(123, 4)]
		[Row(123, 5)]
		[Row(123, 6)]
		[Row(123, 7)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Shr(byte first, byte second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

		#region Ceq

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Ceq(byte first, byte second)
		{
			this.comparisonTests.Ceq((first == second), first, second);
		}

		#endregion // Ceq

		#region Cgt

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Cgt(byte first, byte second)
		{
			this.comparisonTests.Cgt((first > second), first, second);
		}

		#endregion // Cgt

		#region Clt

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Clt(byte first, byte second)
		{
			this.comparisonTests.Clt((first < second), first, second);
		}

		#endregion // Clt

		#region Cge

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Cge(byte first, byte second)
		{
			this.comparisonTests.Cge((first >= second), first, second);
		}

		#endregion // Cge

		#region Cle

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, byte.MaxValue)]
		[Row(0, byte.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, byte.MaxValue)]
		[Row(1, byte.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, byte.MaxValue)]
		[Row(2, byte.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 2)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MaxValue - 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		[Row(byte.MaxValue - 1, 0)]
		[Row(byte.MaxValue - 1, 1)]
		[Row(byte.MaxValue - 1, 2)]
		[Row(byte.MaxValue - 1, byte.MaxValue)]
		[Row(byte.MaxValue - 1, byte.MaxValue - 1)]
		[Row(byte.MaxValue - 1, 17)]
		[Row(byte.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, byte.MaxValue)]
		[Row(17, byte.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, byte.MaxValue)]
		[Row(123, byte.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Cle(byte first, byte second)
		{
			this.comparisonTests.Cle((first <= second), first, second);
		}

		#endregion // Cle

		#region Newarr

		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Newarr()
		{
			this.arrayTests.Newarr();
		}

		#endregion // Newarr

		#region Ldlen

		[Row(0)]
		[Row(1)]
		[Row(10)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Ldlen(int length)
		{
			this.arrayTests.Ldlen(length);
		}

		#endregion // Ldlen

		#region Stelem

		[Row(0, Byte.MinValue)]
		[Row(0, 1)]
		[Row(0, Byte.MaxValue)]
		[Row(3, Byte.MinValue)]
		[Row(6, 1)]
		[Row(2, Byte.MaxValue)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Stelem(int index, byte value)
		{
			this.arrayTests.Stelem(index, value);
		}

		#endregion // Stelem

		#region Ldelem

		[Row(0, Byte.MinValue)]
		[Row(0, 1)]
		[Row(0, Byte.MaxValue)]
		[Row(3, Byte.MinValue)]
		[Row(6, 1)]
		[Row(2, Byte.MaxValue)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Ldelem(int index, byte value)
		{
			this.arrayTests.Ldelem(index, value);
		}

		#endregion // Ldelem

		#region Ldelema

		[Row(0, Byte.MinValue)]
		[Row(0, 1)]
		[Row(0, Byte.MaxValue)]
		[Row(3, Byte.MinValue)]
		[Row(6, 1)]
		[Row(2, Byte.MaxValue)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Ldelema(int index, byte value)
		{
			this.arrayTests.Ldelema(index, value);
		}

		#endregion // Ldelema
	}
}
