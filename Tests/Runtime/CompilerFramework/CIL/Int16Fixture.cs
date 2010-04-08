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
	public class Int16Fixture : RuntimeFixture
	{
		private readonly ArithmeticInstructionTestRunner<int, short> arithmeticTests = new ArithmeticInstructionTestRunner<int, short>
		{
			ExpectedTypeName = @"int",
			TypeName = @"short"
		};

		private readonly BinaryLogicInstructionTestRunner<int, short> logicTests = new BinaryLogicInstructionTestRunner<int, short>
		{
			ExpectedTypeName = @"int",
			TypeName = @"short",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<short> comparisonTests = new ComparisonInstructionTestRunner<short>
		{
			TypeName = @"short"
		};

        private readonly SZArrayInstructionTestRunner<short> arrayTests = new SZArrayInstructionTestRunner<short>
        {
            TypeName = @"short"
        };

		#region Add

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0)]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0)]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Add(short a, short b)
		{
			this.arithmeticTests.Add((a + b), a, b);
		}

		#endregion // Add

		#region Sub

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0)]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0)]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void Sub(short a, short b)
		{
			this.arithmeticTests.Sub((a - b), a, b);
		}

		#endregion // Sub

		#region Mul

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0)]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0)]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Row(short.MaxValue, short.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Mul(short a, short b)
		{
			this.arithmeticTests.Mul((a * b), a, b);
		}

		#endregion // Mul

		#region Div

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Row(short.MaxValue, short.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Div(short a, short b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Row(short.MaxValue, short.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void Rem(short a, short b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		#endregion // Rem

		#region Neg

		[Row(0)]
		[Row(1)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test]
		public void Neg(short first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion Neg

		#region Ret

		[Row(0)]
		[Row(1)]
		[Row(128)]
		[Row(short.MaxValue)]
		[Row(short.MinValue)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
		public void Ret(short value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion Ret

		#region Ceq

		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(true, short.MaxValue, short.MaxValue)]
		[Row(false, 1, short.MinValue)]
		[Row(false, 0, short.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, short.MinValue, 1)]
		[Row(false, short.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
		public void Ceq(bool expectedValue, short first, short second)
		{
			this.comparisonTests.Ceq(expectedValue, first, second);
		}

		#endregion // Ceq

		#region And

		[Row(1, 1)]
		[Row(0, short.MaxValue)]
		[Row(1, 0)]
		[Row(short.MaxValue, 1)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void And(short first, short second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Row(0, 1)]
		[Row(0, short.MaxValue)]
		[Row(1, 0)]
		[Row(short.MaxValue, 0)]
		[Row(0, 128)]
		[Row(128, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Or(short first, short second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Row(0, 1)]
		[Row(1, short.MaxValue)]
		[Row(1, 1)]
		[Row(short.MaxValue, 0)]
		[Row(128, 128)]
		[Row(128, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Xor(short first, short second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Row(4, 1)]
		[Row(8, 2)]
		[Row(4, 3)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Shl(short first, short second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Row(4, 1)]
		[Row(8, 2)]
		[Row(128, 3)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Shr(short first, short second)
		{
			this.logicTests.Shr((first >> second), first, second);
		}

		#endregion // Shr

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

        [Row(0, Int16.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int16.MaxValue)]
        [Row(3, Int16.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, short value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, Int16.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int16.MaxValue)]
        [Row(3, Int16.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, short value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, Int16.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int16.MaxValue)]
        [Row(3, Int16.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int16.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, short value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
