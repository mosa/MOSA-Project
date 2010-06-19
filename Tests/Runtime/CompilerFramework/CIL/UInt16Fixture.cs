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
	public class UInt16Fixture : RuntimeFixture
	{
        private readonly ArithmeticInstructionTestRunner<int, ushort> arithmeticTests = new ArithmeticInstructionTestRunner<int, ushort>
        {
            ExpectedTypeName = @"int",
            TypeName = @"ushort"
        };

		private readonly BinaryLogicInstructionTestRunner<int, ushort, ushort> logicTests = new BinaryLogicInstructionTestRunner<int, ushort, ushort>
		{
			ExpectedTypeName = @"int",
			ShiftTypeName = @"ushort",
			TypeName = @"ushort",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<ushort> comparisonTests = new ComparisonInstructionTestRunner<ushort>
		{
			TypeName = @"ushort"
		};

        private readonly SZArrayInstructionTestRunner<ushort> arrayTests = new SZArrayInstructionTestRunner<ushort>
        {
            TypeName = @"ushort"
        };

		#region Add

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Add(ushort a, ushort b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Sub(ushort a, ushort b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Mul(ushort a, ushort b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		//[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		//[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		//[Row(2, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		//[Row(UInt16.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		//[Row(UInt16.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		//[Row(17, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		//[Row(123, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Div(ushort a, ushort b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		//[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		//[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		//[Row(2, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		//[Row(UInt16.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		//[Row(UInt16.MaxValue - 1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		//[Row(17, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		//[Row(123, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Rem(ushort a, ushort b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		#endregion // Rem

		#region Ret

		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1)]
		[Row(17)]
		[Row(123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Ret(ushort value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion // Ret

		#region And

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void And(ushort first, ushort second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Or(ushort first, ushort second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Row(0, 0)]
		[Row(0, 1)]
		[Row(0, 2)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, UInt16.MaxValue - 1)]
		[Row(0, 17)]
		[Row(0, 123)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, UInt16.MaxValue)]
		[Row(1, UInt16.MaxValue - 1)]
		[Row(1, 17)]
		[Row(1, 123)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, UInt16.MaxValue)]
		[Row(2, UInt16.MaxValue - 1)]
		[Row(2, 17)]
		[Row(2, 123)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(UInt16.MaxValue, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue, 17)]
		[Row(UInt16.MaxValue, 123)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue)]
		[Row(UInt16.MaxValue - 1, UInt16.MaxValue - 1)]
		[Row(UInt16.MaxValue - 1, 17)]
		[Row(UInt16.MaxValue - 1, 123)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, UInt16.MaxValue)]
		[Row(17, UInt16.MaxValue - 1)]
		[Row(17, 17)]
		[Row(17, 123)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, UInt16.MaxValue)]
		[Row(123, UInt16.MaxValue - 1)]
		[Row(123, 17)]
		[Row(123, 123)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Xor(ushort first, ushort second)
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
		[Row(0, 8)]
		[Row(0, 9)]
		[Row(0, 10)]
		[Row(0, 11)]
		[Row(0, 12)]
		[Row(0, 13)]
		[Row(0, 14)]
		[Row(0, 15)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(1, 8)]
		[Row(1, 9)]
		[Row(1, 10)]
		[Row(1, 11)]
		[Row(1, 12)]
		[Row(1, 13)]
		[Row(1, 14)]
		[Row(1, 15)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, 3)]
		[Row(2, 4)]
		[Row(2, 5)]
		[Row(2, 6)]
		[Row(2, 7)]
		[Row(2, 8)]
		[Row(2, 9)]
		[Row(2, 10)]
		[Row(2, 11)]
		[Row(2, 12)]
		[Row(2, 13)]
		[Row(2, 14)]
		[Row(2, 15)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, 3)]
		[Row(UInt16.MaxValue, 4)]
		[Row(UInt16.MaxValue, 5)]
		[Row(UInt16.MaxValue, 6)]
		[Row(UInt16.MaxValue, 7)]
		[Row(UInt16.MaxValue, 8)]
		[Row(UInt16.MaxValue, 9)]
		[Row(UInt16.MaxValue, 10)]
		[Row(UInt16.MaxValue, 11)]
		[Row(UInt16.MaxValue, 12)]
		[Row(UInt16.MaxValue, 13)]
		[Row(UInt16.MaxValue, 14)]
		[Row(UInt16.MaxValue, 15)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, 3)]
		[Row(UInt16.MaxValue - 1, 4)]
		[Row(UInt16.MaxValue - 1, 5)]
		[Row(UInt16.MaxValue - 1, 6)]
		[Row(UInt16.MaxValue - 1, 7)]
		[Row(UInt16.MaxValue - 1, 8)]
		[Row(UInt16.MaxValue - 1, 9)]
		[Row(UInt16.MaxValue - 1, 10)]
		[Row(UInt16.MaxValue - 1, 11)]
		[Row(UInt16.MaxValue - 1, 12)]
		[Row(UInt16.MaxValue - 1, 13)]
		[Row(UInt16.MaxValue - 1, 14)]
		[Row(UInt16.MaxValue - 1, 15)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, 3)]
		[Row(17, 4)]
		[Row(17, 5)]
		[Row(17, 6)]
		[Row(17, 7)]
		[Row(17, 8)]
		[Row(17, 9)]
		[Row(17, 10)]
		[Row(17, 11)]
		[Row(17, 12)]
		[Row(17, 13)]
		[Row(17, 14)]
		[Row(17, 15)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, 3)]
		[Row(123, 4)]
		[Row(123, 5)]
		[Row(123, 6)]
		[Row(123, 7)]
		[Row(123, 8)]
		[Row(123, 9)]
		[Row(123, 10)]
		[Row(123, 11)]
		[Row(123, 12)]
		[Row(123, 13)]
		[Row(123, 14)]
		[Row(123, 15)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Shl(ushort first, ushort second)
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
		[Row(0, 8)]
		[Row(0, 9)]
		[Row(0, 10)]
		[Row(0, 11)]
		[Row(0, 12)]
		[Row(0, 13)]
		[Row(0, 14)]
		[Row(0, 15)]
		[Row(1, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(1, 3)]
		[Row(1, 4)]
		[Row(1, 5)]
		[Row(1, 6)]
		[Row(1, 7)]
		[Row(1, 8)]
		[Row(1, 9)]
		[Row(1, 10)]
		[Row(1, 11)]
		[Row(1, 12)]
		[Row(1, 13)]
		[Row(1, 14)]
		[Row(1, 15)]
		[Row(2, 0)]
		[Row(2, 1)]
		[Row(2, 2)]
		[Row(2, 3)]
		[Row(2, 4)]
		[Row(2, 5)]
		[Row(2, 6)]
		[Row(2, 7)]
		[Row(2, 8)]
		[Row(2, 9)]
		[Row(2, 10)]
		[Row(2, 11)]
		[Row(2, 12)]
		[Row(2, 13)]
		[Row(2, 14)]
		[Row(2, 15)]
		[Row(UInt16.MaxValue, 0)]
		[Row(UInt16.MaxValue, 1)]
		[Row(UInt16.MaxValue, 2)]
		[Row(UInt16.MaxValue, 3)]
		[Row(UInt16.MaxValue, 4)]
		[Row(UInt16.MaxValue, 5)]
		[Row(UInt16.MaxValue, 6)]
		[Row(UInt16.MaxValue, 7)]
		[Row(UInt16.MaxValue, 8)]
		[Row(UInt16.MaxValue, 9)]
		[Row(UInt16.MaxValue, 10)]
		[Row(UInt16.MaxValue, 11)]
		[Row(UInt16.MaxValue, 12)]
		[Row(UInt16.MaxValue, 13)]
		[Row(UInt16.MaxValue, 14)]
		[Row(UInt16.MaxValue, 15)]
		[Row(UInt16.MaxValue - 1, 0)]
		[Row(UInt16.MaxValue - 1, 1)]
		[Row(UInt16.MaxValue - 1, 2)]
		[Row(UInt16.MaxValue - 1, 3)]
		[Row(UInt16.MaxValue - 1, 4)]
		[Row(UInt16.MaxValue - 1, 5)]
		[Row(UInt16.MaxValue - 1, 6)]
		[Row(UInt16.MaxValue - 1, 7)]
		[Row(UInt16.MaxValue - 1, 8)]
		[Row(UInt16.MaxValue - 1, 9)]
		[Row(UInt16.MaxValue - 1, 10)]
		[Row(UInt16.MaxValue - 1, 11)]
		[Row(UInt16.MaxValue - 1, 12)]
		[Row(UInt16.MaxValue - 1, 13)]
		[Row(UInt16.MaxValue - 1, 14)]
		[Row(UInt16.MaxValue - 1, 15)]
		[Row(17, 0)]
		[Row(17, 1)]
		[Row(17, 2)]
		[Row(17, 3)]
		[Row(17, 4)]
		[Row(17, 5)]
		[Row(17, 6)]
		[Row(17, 7)]
		[Row(17, 8)]
		[Row(17, 9)]
		[Row(17, 10)]
		[Row(17, 11)]
		[Row(17, 12)]
		[Row(17, 13)]
		[Row(17, 14)]
		[Row(17, 15)]
		[Row(123, 0)]
		[Row(123, 1)]
		[Row(123, 2)]
		[Row(123, 3)]
		[Row(123, 4)]
		[Row(123, 5)]
		[Row(123, 6)]
		[Row(123, 7)]
		[Row(123, 8)]
		[Row(123, 9)]
		[Row(123, 10)]
		[Row(123, 11)]
		[Row(123, 12)]
		[Row(123, 13)]
		[Row(123, 14)]
		[Row(123, 15)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void Shr(ushort first, ushort second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

		#region Ceq

		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, ushort.MinValue, ushort.MinValue)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, 1, ushort.MinValue)]
		[Row(false, 0, ushort.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, ushort.MinValue, 1)]
		[Row(false, ushort.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
		public void Ceq(bool expectedValue, ushort first, ushort second)
		{
			this.comparisonTests.Ceq(expectedValue, first, second);
		}

		#endregion // Ceq

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

        [Row(0, UInt16.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt16.MaxValue)]
        [Row(3, UInt16.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, ushort value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, UInt16.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt16.MaxValue)]
        [Row(3, UInt16.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, ushort value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, UInt16.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt16.MaxValue)]
        [Row(3, UInt16.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, ushort value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
