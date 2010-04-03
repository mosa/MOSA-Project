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
    [Description(@"Tests support for the basic type System.Double")]
    public class DoubleFixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<double, double> arithmeticTests = new ArithmeticInstructionTestRunner<double, double>
        {
            ExpectedTypeName = @"double",
            TypeName = @"double",
            IncludeNeg = false
        };

        private readonly ComparisonInstructionTestRunner<double> comparisonTests = new ComparisonInstructionTestRunner<double>
        {
            TypeName = @"double"
        };

        private readonly SZArrayInstructionTestRunner<double> arrayTests = new SZArrayInstructionTestRunner<double>
        {
            TypeName = @"double"
        };

        #region Add

        [Row(1.2, 2.1)]
        [Row(23.0, 21.2578)]
        [Row(1.0, -2.198)]
        [Row(-1.2, 2.11)]
        [Row(0.0, 0.0)]
        [Row(-17.1, -2.3)]
        // (MinValue, X) Cases
        [Row(double.MinValue, 0.0)]
        [Row(double.MinValue, 1.2)]
        [Row(double.MinValue, 17.6)]
        [Row(double.MinValue, 123.1)]
        [Row(double.MinValue, -0.0)]
        [Row(double.MinValue, -1.5)]
        [Row(double.MinValue, -17.99)]
        [Row(double.MinValue, -123.235)]
        // (MaxValue, X) Cases
        [Row(double.MaxValue, 0.0)]
        [Row(double.MaxValue, 1.67)]
        [Row(double.MaxValue, 17.875)]
        [Row(double.MaxValue, 123.283)]
        [Row(double.MaxValue, -0.0)]
        [Row(double.MaxValue, -1.1497)]
        [Row(double.MaxValue, -17.12)]
        [Row(double.MaxValue, -123.34)]
        // (X, MinValue) Cases
        [Row(0.0, double.MinValue)]
        [Row(1.2, double.MinValue)]
        [Row(17.4, double.MinValue)]
        [Row(123.561, double.MinValue)]
        [Row(-0.0, double.MinValue)]
        [Row(-1.78, double.MinValue)]
        [Row(-17.59, double.MinValue)]
        [Row(-123.41, double.MinValue)]
        // (X, MaxValue) Cases
        [Row(0.0, double.MaxValue)]
        [Row(1.00012, double.MaxValue)]
        [Row(17.094002, double.MaxValue)]
        [Row(123.001, double.MaxValue)]
        [Row(-0.0, double.MaxValue)]
        [Row(-1.045, double.MaxValue)]
        [Row(-17.0002501, double.MaxValue)]
        [Row(-123.023, double.MaxValue)]
        // Extremvaluecases
        [Row(double.MinValue, double.MaxValue)]
        [Row(1.0f, double.NaN)]
        [Row(double.NaN, 1.0f)]
        [Row(1.0f, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0f)]
        [Row(1.0f, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0f)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Add(double a, double b)
        {
            this.arithmeticTests.Add((double)(a + b), a, b);
        }

        #endregion // Add

        #region Sub

        [Row(1.2, 2.1)]
        [Row(23.0, 21.2578)]
        [Row(1.0, -2.198)]
        [Row(-1.2, 2.11)]
        [Row(0.0, 0.0)]
        [Row(-17.1, -2.3)]
        // (MinValue, X) Cases
        [Row(double.MinValue, 0.0)]
        [Row(double.MinValue, 1.2)]
        [Row(double.MinValue, 17.6)]
        [Row(double.MinValue, 123.1)]
        [Row(double.MinValue, -0.0)]
        [Row(double.MinValue, -1.5)]
        [Row(double.MinValue, -17.99)]
        [Row(double.MinValue, -123.235)]
        // (MaxValue, X) Cases
        [Row(double.MaxValue, 0.0)]
        [Row(double.MaxValue, 1.67)]
        [Row(double.MaxValue, 17.875)]
        [Row(double.MaxValue, 123.283)]
        [Row(double.MaxValue, -0.0)]
        [Row(double.MaxValue, -1.1497)]
        [Row(double.MaxValue, -17.12)]
        [Row(double.MaxValue, -123.34)]
        // (X, MinValue) Cases
        [Row(0.0, double.MinValue)]
        [Row(1.2, double.MinValue)]
        [Row(17.4, double.MinValue)]
        [Row(123.561, double.MinValue)]
        [Row(-0.0, double.MinValue)]
        [Row(-1.78, double.MinValue)]
        [Row(-17.59, double.MinValue)]
        [Row(-123.41, double.MinValue)]
        // (X, MaxValue) Cases
        [Row(0.0, double.MaxValue)]
        [Row(1.00012, double.MaxValue)]
        [Row(17.094002, double.MaxValue)]
        [Row(123.001, double.MaxValue)]
        [Row(-0.0, double.MaxValue)]
        [Row(-1.045, double.MaxValue)]
        [Row(-17.0002501, double.MaxValue)]
        [Row(-123.023, double.MaxValue)]
        // Extremvaluecases
        [Row(double.MinValue, double.MaxValue)]
        [Row(1.0f, double.NaN)]
        [Row(double.NaN, 1.0f)]
        [Row(1.0f, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0f)]
        [Row(1.0f, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0f)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void Sub(double a, double b)
        {
            this.arithmeticTests.Sub((double)(a - b), a, b);
        }

        #endregion // Sub

        #region Mul

        [Row(1.0, 2.0)]
        [Row(2.0, 0.0)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Mul(double a, double b)
        {
            this.arithmeticTests.Mul((double)(a * b), a, b);
        }

        #endregion // Mul

        #region Div

        [Row(1.0, 2.0)]
        [Row(2.0, 1.0)]
        [Row(1.0, 2.5)]
        [Row(1.7, 2.3)]
        [Row(2.0, -1.0)]
        [Row(1.0, -2.5)]
        [Row(1.7, -2.3)]
        [Row(-2.0, 1.0)]
        [Row(-1.0, 2.5)]
        [Row(-1.7, 2.3)]
        [Row(-2.0, -1.0)]
        [Row(-1.0, -2.5)]
        [Row(-1.7, -2.3)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Row(1.0, 0.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void Div(double a, double b)
        {
            this.arithmeticTests.Div((double)(a / b), a, b);
        }

        #endregion // Div

        #region Rem

        [Row(3.14, 2.1)]
        [Row(3.0, 2.0)]
        [Row(10.0, 6.0)]
        [Row(-10.0, 6.0)]
        [Row(-10.0, -6.0)]
        [Row(10.0, -6.0)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Rem(double a, double b)
        {
            this.arithmeticTests.Rem((double)(a % b), a, b);
        }

        #endregion // Rem

        #region Ret

        [Row(0)]
        [Row(1)]
        [Row(double.MinValue)]
        [Row(double.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        [Ignore(@"MOSA puts floating point results in XMM#0, where stdcall expects them in FP0 causing this test to fail.")]
        public void Ret(double value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion // Ret

        #region Ceq

        [Row(true, 0, 0)]
        [Row(false, 0, 1)]
        [Row(true, 1, 1)]
        [Row(false, 1, 0)]
        [Row(true, double.MinValue, double.MinValue)]
        [Row(true, double.MaxValue, double.MaxValue)]
        [Row(false, double.MinValue, double.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ceq(bool expectedValue, double first, double second)
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

        [Row(0, Double.MinValue)]
        [Row(0, -1.1)]
        [Row(0, 0.0)]
        [Row(0, 1.7)]
        [Row(0, Double.MaxValue)]
        [Row(3, Double.MinValue)]
        [Row(7, -1.9)]
        [Row(9, 0.0)]
        [Row(6, 1.3)]
        [Row(2, Double.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, double value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, Double.MinValue)]
        [Row(0, -1.1)]
        [Row(0, 0.0)]
        [Row(0, 1.7)]
        [Row(0, Double.MaxValue)]
        [Row(3, Double.MinValue)]
        [Row(7, -1.9)]
        [Row(9, 0.0)]
        [Row(6, 1.3)]
        [Row(2, Double.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, double value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, Double.MinValue)]
        [Row(0, -1.1)]
        [Row(0, 0.0)]
        [Row(0, 1.7)]
        [Row(0, Double.MaxValue)]
        [Row(3, Double.MinValue)]
        [Row(7, -1.9)]
        [Row(9, 0.0)]
        [Row(6, 1.3)]
        [Row(2, Double.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, double value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
