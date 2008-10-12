/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Tests support for the IL ceq operation with various operands.
    /// </summary>
    [TestFixture]
    public class Clt : CodeDomTestRunner
    {
        private static string s_testCode = @"
            static class Test {
                public static bool Clt(t1 a, t2 b) {
                    return (a < b);
                }
            }
        ";

        delegate bool B_I1_I1(sbyte a, sbyte b);
        delegate bool B_I2_I2(short a, short b);
        delegate bool B_I4_I4(int a, int b);
        delegate bool B_I8_I8(long a, long b);
        delegate bool B_U1_U1(byte a, byte b);
        delegate bool B_U2_U2(ushort a, ushort b);
        delegate bool B_U4_U4(uint a, uint b);
        delegate bool B_U8_U8(ulong a, ulong b);
        delegate bool B_R4_R4(float a, float b);
        delegate bool B_R8_R8(double a, double b);

        /// <summary>
        /// Tests support for the ceq IL operation for I1 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(1, 2)]
        [Row(SByte.MinValue, SByte.MinValue + 10)]
        [Row(SByte.MaxValue, SByte.MaxValue)]
        [Row(0, SByte.MinValue)]
        [Row(0, SByte.MaxValue)]
        [Row(0, 1)]
        [Row(SByte.MinValue, 0)]
        [Row(SByte.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltI1(sbyte a, sbyte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "sbyte").Replace("t2", "sbyte");
            bool res = (bool)Run<B_I1_I1>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for I4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(Int16.MinValue, Int16.MinValue + 10)]
        [Row(Int16.MaxValue, Int16.MaxValue)]
        [Row(0, Int16.MinValue)]
        [Row(0, Int16.MaxValue)]
        [Row(0, 1)]
        [Row(Int16.MinValue, 0)]
        [Row(Int16.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltI2(short a, short b)
        {
            this.CodeSource = s_testCode.Replace("t1", "short").Replace("t2", "short");
            bool res = (bool)Run<B_I2_I2>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for I4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(Int32.MinValue, Int32.MinValue  + 10)]
        [Row(Int32.MaxValue, Int32.MaxValue)]
        [Row(0, Int32.MinValue)]
        [Row(0, Int32.MaxValue)]
        [Row(0, 1)]
        [Row(Int32.MinValue, 0)]
        [Row(Int32.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltI4(int a, int b)
        {
            this.CodeSource = s_testCode.Replace("t1", "int").Replace("t2", "int");
            bool res = (bool)Run<B_I4_I4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for I8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0L, 0L)]
        [Row(1L, 1L)]
        [Row(Int64.MinValue, Int64.MinValue + 10)]
        [Row(Int64.MaxValue, Int64.MaxValue)]
        [Row(0L, Int64.MinValue)]
        [Row(0L, Int64.MaxValue)]
        [Row(0L, 1L)]
        [Row(Int64.MinValue, 0L)]
        [Row(Int64.MaxValue, 0L)]
        [Row(1L, 0L)]
        [Row(-1L, -2L)]
        [Row(-2L, -1L)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltI8(long a, long b)
        {
            this.CodeSource = s_testCode.Replace("t1", "long").Replace("t2", "long");
            bool res = (bool)Run<B_I8_I8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for U1 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(Byte.MinValue, Byte.MinValue + 10)]
        [Row(Byte.MaxValue, Byte.MaxValue)]
        [Row(1, Byte.MinValue)]
        [Row(0, Byte.MaxValue)]
        [Row(0, 1)]
        [Row(Byte.MinValue, 1)]
        [Row(Byte.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltU1(byte a, byte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "byte").Replace("t2", "byte");
            bool res = (bool)Run<B_U1_U1>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for U2 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(UInt16.MinValue, UInt16.MinValue + 10)]
        [Row(UInt16.MaxValue, UInt16.MaxValue)]
        [Row(1, UInt16.MinValue)]
        [Row(0, UInt16.MaxValue)]
        [Row(0, 1)]
        [Row(UInt16.MinValue, 2)]
        [Row(UInt16.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltU2(ushort a, ushort b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ushort").Replace("t2", "ushort");
            bool res = (bool)Run<B_U2_U2>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for U4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(UInt32.MinValue, UInt32.MinValue + 10)]
        [Row(UInt32.MaxValue, UInt32.MaxValue)]
        [Row(1, UInt32.MinValue)]
        [Row(0, UInt32.MaxValue)]
        [Row(0, 1)]
        [Row(UInt32.MinValue, 1)]
        [Row(UInt32.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltU4(uint a, uint b)
        {
            this.CodeSource = s_testCode.Replace("t1", "uint").Replace("t2", "uint");
            bool res = (bool)Run<B_U4_U4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for U4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0, 0)]
        [Row(1, 1)]
        [Row(UInt64.MinValue, UInt64.MinValue + 10)]
        [Row(UInt64.MaxValue, UInt64.MaxValue)]
        [Row(1, UInt64.MinValue)]
        [Row(0, UInt64.MaxValue)]
        [Row(0, 1)]
        [Row(UInt64.MinValue, 1)]
        [Row(UInt64.MaxValue, 0)]
        [Row(1, 0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltU8(ulong a, ulong b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ulong").Replace("t2", "ulong");
            bool res = (bool)Run<B_U8_U8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for R4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0.0f, 0.0f)]
        [Row(1.0f, 1.0f)]
        [Row(Single.MinValue, Single.MinValue + 10)]
        [Row(Single.MaxValue, Single.MaxValue)]
        [Row(0.0f, Single.MinValue)]
        [Row(0.0f, Single.MaxValue)]
        [Row(0.0f, 1.0f)]
        [Row(Single.MinValue, 0.0f)]
        [Row(Single.MaxValue, 0.0f)]
        [Row(1.0f, 0.0f)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltR4(float a, float b)
        {
            this.CodeSource = s_testCode.Replace("t1", "float").Replace("t2", "float");
            bool res = (bool)Run<B_R4_R4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }

        /// <summary>
        /// Tests support for the ceq IL operation for R8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [Row(0.0, 0.5)]
        [Row(1.0, 2.0)]
        [Row(Double.MinValue, Double.MinValue + 1)]
        [Row(Double.MaxValue - 1, Double.MaxValue)]
        [Row(0.0, Double.MinValue)]
        [Row(0.0, Double.MaxValue)]
        [Row(0.0, 1.0)]
        [Row(Double.MinValue, 0.0)]
        [Row(Double.MaxValue, 0.0)]
        [Row(1.0, 0.0)]
        [Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
        public void CltR8(double a, double b)
        {
            this.CodeSource = s_testCode.Replace("t1", "double").Replace("t2", "double");
            bool res = (bool)Run<B_R8_R8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
    }
}
