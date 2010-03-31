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
    public class UInt32Fixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<uint, uint> arithmeticTests = new ArithmeticInstructionTestRunner<uint, uint>
        {
            ExpectedTypeName = @"uint",
            TypeName = @"uint"
        };

        private readonly BinaryLogicInstructionTestRunner<uint, uint> logicTests = new BinaryLogicInstructionTestRunner<uint, uint>
        {
            ExpectedTypeName = @"uint",
            TypeName = @"uint"
        };

        private readonly ComparisonInstructionTestRunner<uint> comparisonTests = new ComparisonInstructionTestRunner<uint>
        {
            TypeName = @"uint"
        };

        private readonly SZArrayInstructionTestRunner<uint> arrayTests = new SZArrayInstructionTestRunner<uint>
        {
            TypeName = @"uint"
        };

        #region Add

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(uint.MinValue, 0)]
        [Row(uint.MinValue, 1)]
        [Row(uint.MinValue, 17)]
        [Row(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0)]
        [Row(uint.MaxValue, 1)]
        [Row(uint.MaxValue, 17)]
        [Row(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, uint.MinValue)]
        [Row(1, uint.MinValue)]
        [Row(17, uint.MinValue)]
        [Row(123, uint.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, uint.MaxValue)]
        [Row(1, uint.MaxValue)]
        [Row(17, uint.MaxValue)]
        [Row(123, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(uint a, uint b)
        {
            this.arithmeticTests.Add((uint)(a + b), a, b);
        }

        #endregion // Add

        #region Sub

        [Row(1U, 2U)]
        [Row(23U, 21U)]
        [Row(0U, 0U)]
        // And reverse
        [Row(2U, 1U)]
        [Row(21U, 23U)]
        // (MinValue, X) Cases
        [Row(uint.MinValue, 0U)]
        [Row(uint.MinValue, 1U)]
        [Row(uint.MinValue, 17U)]
        [Row(uint.MinValue, 123U)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0U)]
        [Row(uint.MaxValue, 1U)]
        [Row(uint.MaxValue, 17U)]
        [Row(uint.MaxValue, 123U)]
        // (X, MinValue) Cases
        [Row(0U, uint.MinValue)]
        [Row(1U, uint.MinValue)]
        [Row(17U, uint.MinValue)]
        [Row(123U, uint.MinValue)]
        // (X, MaxValue) Cases
        [Row(0U, uint.MaxValue)]
        [Row(1U, uint.MaxValue)]
        [Row(17U, uint.MaxValue)]
        [Row(123U, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(uint a, uint b)
        {
            this.arithmeticTests.Sub((uint)(a - b), a, b);
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
        [Row(uint.MinValue, 0)]
        [Row(uint.MinValue, 1)]
        [Row(uint.MinValue, 17)]
        [Row(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0)]
        [Row(uint.MaxValue, 1)]
        [Row(uint.MaxValue, 17)]
        [Row(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, uint.MinValue)]
        [Row(1, uint.MinValue)]
        [Row(17, uint.MinValue)]
        [Row(123, uint.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, uint.MaxValue)]
        [Row(1, uint.MaxValue)]
        [Row(17, uint.MaxValue)]
        [Row(123, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Row(uint.MaxValue, uint.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(uint a, uint b)
        {
            this.arithmeticTests.Mul((uint)(a * b), a, b);
        }

        #endregion // MU

        #region Div

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(uint.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(uint.MinValue, 1)]
        [Row(uint.MinValue, 17)]
        [Row(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(uint.MaxValue, 1)]
        [Row(uint.MaxValue, 17)]
        [Row(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, uint.MaxValue)]
        [Row(1, uint.MaxValue)]
        [Row(17, uint.MaxValue)]
        [Row(123, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Row(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(uint a, uint b)
        {
            this.arithmeticTests.Div((uint)(a / b), a, b);
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
        [Row(uint.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(uint.MinValue, 1)]
        [Row(uint.MinValue, 17)]
        [Row(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(uint.MaxValue, 1)]
        [Row(uint.MaxValue, 17)]
        [Row(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, uint.MaxValue)]
        [Row(1, uint.MaxValue)]
        [Row(17, uint.MaxValue)]
        [Row(123, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Row(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Rem(uint a, uint b)
        {
            this.arithmeticTests.Rem((uint)(a % b), a, b);
        }

        #endregion // Rem

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

        [Row(0, UInt32.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt32.MaxValue)]
        [Row(3, UInt32.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, uint value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, UInt32.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt32.MaxValue)]
        [Row(3, UInt32.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, uint value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, UInt32.MinValue)]
        [Row(0, 1)]
        [Row(0, UInt32.MaxValue)]
        [Row(3, UInt32.MinValue)]
        [Row(6, 1)]
        [Row(2, UInt32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, uint value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
