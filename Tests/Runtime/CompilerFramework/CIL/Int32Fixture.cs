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
    [Description(@"Tests support for the basic type System.Int32")]
    public class Int32Fixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<int, int> arithmeticTests = new ArithmeticInstructionTestRunner<int, int>
        {
            ExpectedTypeName = @"int",
            TypeName = @"int"
        };

		private readonly BinaryLogicInstructionTestRunner<int, int, int> logicTests = new BinaryLogicInstructionTestRunner<int, int, int>
		{
			ExpectedTypeName = @"int",
			TypeName = @"int"
		};

        private readonly ComparisonInstructionTestRunner<int> comparisonTests = new ComparisonInstructionTestRunner<int>
        {
            TypeName = @"int"
        };

        private readonly SZArrayInstructionTestRunner<int> arrayTests = new SZArrayInstructionTestRunner<int>
        {
            TypeName = @"int"
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
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0)]
        [Row(int.MinValue, -1)]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0)]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(int a, int b)
        {
            this.arithmeticTests.Add((a + b), a, b);
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
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0)]
        [Row(int.MinValue, -1)]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0)]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(int a, int b)
        {
            this.arithmeticTests.Sub((a - b), a, b);
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
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0)]
        [Row(int.MinValue, -1)]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0)]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(int a, int b)
        {
            this.arithmeticTests.Mul((a * b), a, b);
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
        [Row(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(int a, int b)
        {
            this.arithmeticTests.Div((a / b), a, b);
        }

        #endregion // Div

        #region Rem

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
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
        [Row(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Rem(int a, int b)
        {
            this.arithmeticTests.Rem((a % b), a, b);
        }

        #endregion // Rem

        #region Ret

        [Row(0)]
        [Row(1)]
        [Row(134217728)]
        [Row(int.MinValue)]
        [Row(int.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ret(int value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion // Ret

        #region Ceq

        [Row(true, 0, 0)]
        [Row(false, 0, 1)]
        [Row(true, 1, 1)]
        [Row(false, 1, 0)]
        [Row(true, int.MinValue, int.MinValue)]
        [Row(true, int.MaxValue, int.MaxValue)]
        [Row(false, int.MinValue, int.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ceq(bool expectedValue, int first, int second)
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

        [Row(0, Int32.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int32.MaxValue)]
        [Row(3, Int32.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, int value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, Int32.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int32.MaxValue)]
        [Row(3, Int32.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, int value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, Int32.MinValue)]
        [Row(0, -1)]
        [Row(0, 0)]
        [Row(0, 1)]
        [Row(0, Int32.MaxValue)]
        [Row(3, Int32.MinValue)]
        [Row(7, -1)]
        [Row(9, 0)]
        [Row(6, 1)]
        [Row(2, Int32.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, int value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
