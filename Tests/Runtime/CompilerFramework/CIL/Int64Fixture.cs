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
    [Importance(Importance.Critical)]
    [Category(@"Basic types")]
    [Description(@"Tests support for the basic type System.Int64")]
    public class Int64Fixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<long, long> arithmeticTests = new ArithmeticInstructionTestRunner<long, long>
        {
            ExpectedTypeName = @"long",
            TypeName = @"long"
        };

        private readonly BinaryLogicInstructionTestRunner<long, long> logicTests = new BinaryLogicInstructionTestRunner<long, long>
        {
            ExpectedTypeName = @"long",
            TypeName = @"long"
        };

        private readonly ComparisonInstructionTestRunner<long> comparisonTests = new ComparisonInstructionTestRunner<long>
        {
            TypeName = @"long"
        };

        private readonly SZArrayInstructionTestRunner<long> arrayTests = new SZArrayInstructionTestRunner<long>
        {
            TypeName = @"long"
        };

        #region Add

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0)]
        [Row(long.MinValue, -1)]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0)]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        [Row(0x0000000100000000L, 0x0000000100000000L)]
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(long a, long b)
        {
            this.arithmeticTests.Add((long)(a + b), a, b);
        }

        #endregion // Add

        #region Sub

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0)]
        [Row(long.MinValue, -1)]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0)]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(long a, long b)
        {
            this.arithmeticTests.Sub((long)(a - b), a, b);
        }

        #endregion // Sub

        #region Mul

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0)]
        [Row(long.MinValue, -1)]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0)]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(long a, long b)
        {
            this.arithmeticTests.Mul((long)(a * b), a, b);
        }

        #endregion // Mul

        #region Div

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Row(long.MaxValue, long.MinValue)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(long a, long b)
        {
            this.arithmeticTests.Div((long)(a / b), a, b);
        }

        #endregion // Div

        #region Rem

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Row(long.MaxValue, long.MinValue)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Rem(long a, long b)
        {
            this.arithmeticTests.Rem((long)(a % b), a, b);
        }

        #endregion // Rem

        #region Ret

        [Row(0L)]
        [Row(1L)]
        [Row(576460752303423488L)]
        [Row(long.MinValue)]
        [Row(long.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ret(long value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion // Ret

        #region Ceq

        [Row(true, 0L, 0L)]
        [Row(false, 0L, 1L)]
        [Row(true, 1L, 1L)]
        [Row(false, 1L, 0L)]
        [Row(true, long.MinValue, long.MinValue)]
        [Row(true, long.MaxValue, long.MaxValue)]
        [Row(false, long.MinValue, long.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ceq(bool expectedValue, long first, long second)
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

        [Row(0, Int64.MinValue)]
        [Row(0, -1L)]
        [Row(0, 0L)]
        [Row(0, 1L)]
        [Row(0, Int64.MaxValue)]
        [Row(3, Int64.MinValue)]
        [Row(7, -1L)]
        [Row(9, 0L)]
        [Row(6, 1L)]
        [Row(2, Int64.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, long value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, Int64.MinValue)]
        [Row(0, -1L)]
        [Row(0, 0L)]
        [Row(0, 1L)]
        [Row(0, Int64.MaxValue)]
        [Row(3, Int64.MinValue)]
        [Row(7, -1L)]
        [Row(9, 0L)]
        [Row(6, 1L)]
        [Row(2, Int64.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, long value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, Int64.MinValue)]
        [Row(0, -1L)]
        [Row(0, 0L)]
        [Row(0, 1L)]
        [Row(0, Int64.MaxValue)]
        [Row(3, Int64.MinValue)]
        [Row(7, -1L)]
        [Row(9, 0L)]
        [Row(6, 1L)]
        [Row(2, Int64.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, long value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
