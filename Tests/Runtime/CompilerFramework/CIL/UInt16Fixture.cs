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

		private readonly BinaryLogicInstructionTestRunner<int, ushort> logicTests = new BinaryLogicInstructionTestRunner<int, ushort>
		{
			ExpectedTypeName = @"int",
			TypeName = @"ushort",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<ushort> comparisonTests = new ComparisonInstructionTestRunner<ushort>
		{
			TypeName = @"ushort"
		};

        private readonly SZArrayInstructionTestRunner<ushort> arrayTests = new SZArrayInstructionTestRunner<ushort>
        {
            TypeName = @"ushort",
        };

        #region Add

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue, 0)]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0)]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue)]
		[Row(1, ushort.MinValue)]
		[Row(17, ushort.MinValue)]
		[Row(123, ushort.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Add(ushort a, ushort b)
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
		[Row(ushort.MinValue, 0)]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0)]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue)]
		[Row(1, ushort.MinValue)]
		[Row(17, ushort.MinValue)]
		[Row(123, ushort.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void Sub(ushort a, ushort b)
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
		[Row(ushort.MinValue, 0)]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0)]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue)]
		[Row(1, ushort.MinValue)]
		[Row(17, ushort.MinValue)]
		[Row(123, ushort.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Row(ushort.MaxValue, ushort.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Mul(ushort a, ushort b)
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
		[Row(ushort.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Row(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void Div(ushort a, ushort b)
		{
			this.arithmeticTests.Div((a / b), a, b);
		}

		#endregion // Div

		#region Rem

        ////[Row(1, 2)]
        ////[Row(23, 21)]
        ////[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        ////// And reverse
        ////[Row(2, 1)]
        ////[Row(21, 23)]
        ////// (MinValue, X) Cases
        ////[Row(ushort.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(ushort.MinValue, 1)]
        ////[Row(ushort.MinValue, 17)]
        ////[Row(ushort.MinValue, 123)]
        ////// (MaxValue, X) Cases
        ////[Row(ushort.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(ushort.MaxValue, 1)]
        ////[Row(ushort.MaxValue, 17)]
        ////[Row(ushort.MaxValue, 123)]
        ////// (X, MinValue) Cases
        ////[Row(0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        ////// (X, MaxValue) Cases
        ////[Row(0, ushort.MaxValue)]
        [Row(1, ushort.MaxValue)]
        ////[Row(17, ushort.MaxValue)]
        ////[Row(123, ushort.MaxValue)]
        ////// Extremvaluecases
        ////[Row(ushort.MinValue, ushort.MaxValue)]
        ////[Row(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        ////[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void Rem(ushort a, ushort b)
		{
			this.arithmeticTests.Rem((a % b), a, b);
		}

		#endregion // Rem

		#region Neg

		[Row(0)]
		[Row(1)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test]
		public void Neg(ushort first)
		{
			this.arithmeticTests.Neg(-first, first);
		}

		#endregion Neg

		#region Ret

		[Row(0)]
		[Row(1)]
		[Row(128)]
		[Row(ushort.MaxValue)]
		[Row(ushort.MinValue)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
		public void Ret(ushort value)
		{
			this.arithmeticTests.Ret(value);
		}

		#endregion Ret

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

		#region And

		[Row(1, 1)]
		[Row(0, ushort.MaxValue)]
		[Row(1, 0)]
		[Row(ushort.MaxValue, 1)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void And(ushort first, ushort second)
		{
			this.logicTests.And((first & second), first, second);
		}

		#endregion // And

		#region Or

		[Row(0, 1)]
		[Row(0, ushort.MaxValue)]
		[Row(1, 0)]
		[Row(ushort.MaxValue, 0)]
		[Row(0, 128)]
		[Row(128, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Or(ushort first, ushort second)
		{
			this.logicTests.Or((first | second), first, second);
		}

		#endregion // Or

		#region Xor

		[Row(0, 1)]
		[Row(1, ushort.MaxValue)]
		[Row(1, 1)]
		[Row(ushort.MaxValue, 0)]
		[Row(128, 128)]
		[Row(128, 0)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Xor(ushort first, ushort second)
		{
			this.logicTests.Xor((first ^ second), first, second);
		}

		#endregion // Xor

		#region Shl

		[Row(4, 1)]
		[Row(8, 2)]
		[Row(4, 3)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Shl(ushort first, ushort second)
		{
			this.logicTests.Shl((first << second), first, second);
		}

		#endregion // Shl

		#region Shr

		[Row(4, 1)]
		[Row(8, 2)]
		[Row(128, 3)]
		[Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
		public void Shr(ushort first, ushort second)
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
