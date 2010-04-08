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
    [Description(@"Tests support for the basic type System.UInt64")]
    public class UInt64Fixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<ulong, ulong> arithmeticTests = new ArithmeticInstructionTestRunner<ulong, ulong>
        {
            ExpectedTypeName = @"ulong",
            TypeName = @"ulong",
            IncludeNeg = false
        };

        private readonly BinaryLogicInstructionTestRunner<ulong, ulong> logicTests = new BinaryLogicInstructionTestRunner<ulong, ulong>
        {
            ExpectedTypeName = @"ulong",
            TypeName = @"ulong"
        };

        private readonly ComparisonInstructionTestRunner<ulong> comparisonTests = new ComparisonInstructionTestRunner<ulong>
        {
            TypeName = @"ulong"
        };

        private readonly SZArrayInstructionTestRunner<ulong> arrayTests = new SZArrayInstructionTestRunner<ulong>
        {
            TypeName = @"ulong"
        };

        #region Add

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(ulong.MinValue, 0)]
        [Row(ulong.MinValue, 1)]
        [Row(ulong.MinValue, 17)]
        [Row(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0)]
        [Row(ulong.MaxValue, 1)]
        [Row(ulong.MaxValue, 17)]
        [Row(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ulong.MinValue)]
        [Row(1, ulong.MinValue)]
        [Row(17, ulong.MinValue)]
        [Row(123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, ulong.MaxValue)]
        [Row(1, ulong.MaxValue)]
        [Row(17, ulong.MaxValue)]
        [Row(123, ulong.MaxValue)]
        // Extremvaluecases
        [Row(ulong.MinValue, ulong.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(ulong a, ulong b)
        {
            this.arithmeticTests.Add((ulong)(a + b), a, b);
        }

        #endregion // Add

        #region Sub

        [Row(1UL, 2UL)]
        [Row(23UL, 21UL)]
        [Row(0UL, 0UL)]
        // And reverse
        [Row(2UL, 1UL)]
        [Row(21UL, 23UL)]
        // (MinValue, X) Cases
        [Row(ulong.MinValue, 0UL)]
        [Row(ulong.MinValue, 1UL)]
        [Row(ulong.MinValue, 17UL)]
        [Row(ulong.MinValue, 123UL)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0UL)]
        [Row(ulong.MaxValue, 1UL)]
        [Row(ulong.MaxValue, 17UL)]
        [Row(ulong.MaxValue, 123UL)]
        // (X, MinValue) Cases
        [Row(0UL, ulong.MinValue)]
        [Row(1UL, ulong.MinValue)]
        [Row(17UL, ulong.MinValue)]
        [Row(123UL, ulong.MinValue)]
        // (X, MaxValue) Cases
        [Row(0UL, ulong.MaxValue)]
        [Row(1UL, ulong.MaxValue)]
        [Row(17UL, ulong.MaxValue)]
        [Row(123UL, ulong.MaxValue)]
        // Extremvaluecases
        [Row(ulong.MinValue, ulong.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(ulong a, ulong b)
        {
            this.arithmeticTests.Sub((ulong)(a - b), a, b);
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
        [Row(ulong.MinValue, 0)]
        [Row(ulong.MinValue, 1)]
        [Row(ulong.MinValue, 17)]
        [Row(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0)]
        [Row(ulong.MaxValue, 1)]
        [Row(ulong.MaxValue, 17)]
        [Row(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ulong.MinValue)]
        [Row(1, ulong.MinValue)]
        [Row(17, ulong.MinValue)]
        [Row(123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, ulong.MaxValue)]
        [Row(1, ulong.MaxValue)]
        [Row(17, ulong.MaxValue)]
        [Row(123, ulong.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(ulong a, ulong b)
        {
            this.arithmeticTests.Mul((ulong)(a * b), a, b);
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
        [Row(ulong.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(ulong.MinValue, 1)]
        [Row(ulong.MinValue, 17)]
        [Row(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(ulong.MaxValue, 1)]
        [Row(ulong.MaxValue, 17)]
        [Row(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, ulong.MaxValue)]
        [Row(1, ulong.MaxValue)]
        [Row(17, ulong.MaxValue)]
        [Row(123, ulong.MaxValue)]
        // Extremvaluecases
        [Row(ulong.MinValue, ulong.MaxValue)]
        [Row(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(ulong a, ulong b)
        {
            this.arithmeticTests.Div((ulong)(a / b), a, b);
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
        [Row(ulong.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(ulong.MinValue, 1)]
        [Row(ulong.MinValue, 17)]
        [Row(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(ulong.MaxValue, 1)]
        [Row(ulong.MaxValue, 17)]
        [Row(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, ulong.MaxValue)]
        [Row(1, ulong.MaxValue)]
        [Row(17, ulong.MaxValue)]
        [Row(123, ulong.MaxValue)]
        // Extremvaluecases
        [Row(ulong.MinValue, ulong.MaxValue)]
        [Row(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Rem(ulong a, ulong b)
        {
            this.arithmeticTests.Rem((ulong)(a % b), a, b);
        }

        #endregion // Rem

        #region Ret

        [Row(0UL)]
        [Row(1UL)]
        [Row(9223372036854775808UL)]
        [Row(ulong.MinValue)]
        [Row(ulong.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ret(ulong value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion // Ret

        #region Ceq

        [Row(true, 0UL, 0UL)]
        [Row(false, 0UL, 1UL)]
        [Row(true, 1UL, 1UL)]
        [Row(false, 1UL, 0UL)]
        [Row(true, ulong.MinValue, ulong.MinValue)]
        [Row(true, ulong.MaxValue, ulong.MaxValue)]
        [Row(false, ulong.MinValue, ulong.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ceq(bool expectedValue, ulong first, ulong second)
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

        [Row(0, UInt64.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt64.MaxValue)]
        [Row(3, UInt64.MinValue)]
        [Row(6, 1)]
        [Row(2, Int32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, ulong value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, UInt64.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt64.MaxValue)]
        [Row(3, UInt64.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt64.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, ulong value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, UInt64.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt64.MaxValue)]
        [Row(3, UInt64.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt64.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, ulong value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
