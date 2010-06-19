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
    [Description(@"Tests support for the basic type System.Char")]
    public class CharFixture : RuntimeFixture
    {
        private readonly ArithmeticInstructionTestRunner<int, char> arithmeticTests = new ArithmeticInstructionTestRunner<int, char>
        {
            ExpectedTypeName = @"int",
            TypeName = @"char"
        };

		private readonly BinaryLogicInstructionTestRunner<char, char, char> logicTests = new BinaryLogicInstructionTestRunner<char, char, char>
		{
			ExpectedTypeName = @"int",
			TypeName = @"char",
			ShiftTypeName = @"char",
			IncludeComp = false
		};

        private readonly ComparisonInstructionTestRunner<char> comparisonTests = new ComparisonInstructionTestRunner<char>
        {
            TypeName = @"char"
        };

        private readonly SZArrayInstructionTestRunner<char> arrayTests = new SZArrayInstructionTestRunner<char>
        {
            TypeName = @"char"
        };

        #region Add

        [Row(0, 0)]
        [Row(0, 1)]
        [Row('-', '.')]
        [Row('a', 'Z')]
        [Row(char.MinValue, char.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void Add(char a, char b)
        {
            this.arithmeticTests.Add((char)(a + b), a, b);
        }

        #endregion // Add

        #region Sub

        [Row(0, 0)]
        [Row(128, 17)]
        [Row('a', 'Z')]
        [Row(char.MinValue, char.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void Sub(char a, char b)
        {
            int expected = a - b;
            this.arithmeticTests.Sub(expected, a, b);
        }

        #endregion // Sub

        #region Mul

        [Row(0, 0)]
        [Row(17, 128)]
        [Row('a', 'Z')]
        [Row(char.MinValue, char.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void Mul(char a, char b)
        {
            this.arithmeticTests.Mul((char)(a * b), a, b);
        }

        #endregion // Mul

        #region Div

        [Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, 128)]
        [Row('a', 'Z')]
        [Row(char.MinValue, char.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void Div(char a, char b)
        {
            this.arithmeticTests.Div((char)(a / b), a, b);
        }

        #endregion // Div

        #region Rem

        [Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(17, 128)]
        [Row('a', 'Z')]
        [Row(char.MinValue, char.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void Rem(char a, char b)
        {
            this.arithmeticTests.Rem((char)(a % b), a, b);
        }

        #endregion // Rem

        #region Ret

        [Row(0)]
        [Row(1)]
        [Row('-')]
        [Row('.')]
        [Row('a')]
        [Row('Z')]
        [Row(char.MinValue)]
        [Row(char.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ret(char value)
        {
            this.arithmeticTests.Ret(value);
        }

        #endregion // Ret

        #region Ceq

        [Row(true, 0, 0)]
        [Row(false, 0, 1)]
        [Row(true, 1, 1)]
        [Row(false, 1, 0)]
        [Row(true, char.MinValue, char.MinValue)]
        [Row(true, char.MaxValue, char.MaxValue)]
        [Row(false, char.MinValue, char.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ceq(bool expectedValue, char first, char second)
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

        [Row(0, Char.MinValue)]
        [Row(0, 1)]
        [Row(0, Char.MaxValue)]
        [Row(3, Char.MinValue)]
        [Row(6, 1)]
        [Row(2, Char.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Stelem(int index, char value)
        {
            this.arrayTests.Stelem(index, value);
        }

        #endregion // Stelem

        #region Ldelem

        [Row(0, Char.MinValue)]
        [Row(0, 1)]
        [Row(0, Char.MaxValue)]
        [Row(3, Char.MinValue)]
        [Row(6, 1)]
        [Row(2, Char.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelem(int index, char value)
        {
            this.arrayTests.Ldelem(index, value);
        }

        #endregion // Ldelem

        #region Ldelema

        [Row(0, Char.MinValue)]
        [Row(0, 1)]
        [Row(0, Char.MaxValue)]
        [Row(3, Char.MinValue)]
        [Row(6, 1)]
        [Row(2, Char.MaxValue)]
        [Test, Author(@"Michael Fröhlich, sharpos@michaelruck.de")]
        public void Ldelema(int index, char value)
        {
            this.arrayTests.Ldelema(index, value);
        }

        #endregion // Ldelema
    }
}
