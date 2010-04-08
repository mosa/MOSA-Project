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
    public class ByteFixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<int, byte> arithmeticTests = new ArithmeticInstructionTestRunner<int, byte>
        {
            ExpectedTypeName = @"int",
            TypeName = @"byte"
        };

        private readonly BinaryLogicInstructionTestRunner<int, byte> logicTests = new BinaryLogicInstructionTestRunner<int, byte>
        {
            ExpectedTypeName = @"int",
            TypeName = @"byte",
            IncludeNot = false
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

        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(byte.MinValue, 0)]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0)]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue)]
        [Row(1, byte.MinValue)]
        [Row(17, byte.MinValue)]
        [Row(123, byte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(byte a, byte b)
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
        [Row(byte.MinValue, 0)]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0)]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue)]
        [Row(1, byte.MinValue)]
        [Row(17, byte.MinValue)]
        [Row(123, byte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(byte a, byte b)
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
        [Row(byte.MinValue, 0)]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0)]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue)]
        [Row(1, byte.MinValue)]
        [Row(17, byte.MinValue)]
        [Row(123, byte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Row(byte.MaxValue, byte.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(byte a, byte b)
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
        [Row(byte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Row(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(byte a, byte b)
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
        [Row(byte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Row(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Rem(byte a, byte b)
        {
            this.arithmeticTests.Rem((a % b), a, b);
        }

        #endregion // Rem

        #region Neg

        [Row(0)]
        [Row(1)]
        [Row(byte.MinValue)]
        [Row(byte.MaxValue)]
        [Test]
        public void Neg(byte first)
        {
            this.arithmeticTests.Neg(-first, first);
        }

        #endregion Neg

        #region Ret

        [Row(0)]
        [Row(1)]
        [Row(128)]
        [Row(Byte.MaxValue)]
        [Row(Byte.MinValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
        public void Ret(byte value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion Ret

        #region Ceq

        [Row(true, 0, 0)]
        [Row(true, 1, 1)]
        [Row(true, Byte.MinValue, Byte.MinValue)]
        [Row(true, Byte.MaxValue, Byte.MaxValue)]
        [Row(false, 1, Byte.MinValue)]
        [Row(false, 0, Byte.MaxValue)]
        [Row(false, 0, 1)]
        [Row(false, Byte.MinValue, 1)]
        [Row(false, Byte.MaxValue, 0)]
        [Row(false, 1, 0)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de"), Importance(Importance.Critical)]
        public void Ceq(bool expectedValue, byte first, byte second)
        {
            this.comparisonTests.Ceq(expectedValue, first, second);
        }

        #endregion // Ceq

        #region And

        [Row(1, 1)]
        [Row(0, byte.MaxValue)]
        [Row(1, 0)]
        [Row(byte.MaxValue, 1)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void And(byte first, byte second)
        {
            this.logicTests.And((first & second), first, second);
        }

        #endregion // And

        #region Or

        [Row(0, 1)]
        [Row(0, byte.MaxValue)]
        [Row(1, 0)]
        [Row(byte.MaxValue, 0)]
        [Row(0, 128)]
        [Row(128, 0)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Or(byte first, byte second)
        {
            this.logicTests.Or((first | second), first, second);
        }

        #endregion // Or

        #region Xor

        [Row(0, 1)]
        [Row(1, byte.MaxValue)]
        [Row(1, 1)]
        [Row(byte.MaxValue, 0)]
        [Row(128, 128)]
        [Row(128, 0)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Xor(byte first, byte second)
        {
            this.logicTests.Xor((first ^ second), first, second);
        }

        #endregion // Xor

        #region Shl

        [Row(4, 1)]
        [Row(8, 2)]
        [Row(4, 3)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Shl(byte first, byte second)
        {
            this.logicTests.Shl((first << second), first, second);
        }

        #endregion // Shl

        #region Shr

        [Row(4, 1)]
        [Row(8, 2)]
        [Row(128, 3)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Shr(byte first, byte second)
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
