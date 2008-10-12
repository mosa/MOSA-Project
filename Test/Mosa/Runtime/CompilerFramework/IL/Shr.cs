/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Shr : MosaCompilerTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(1, 2)]
        [Row(23, 3)]
        // And reverse
        [Row(2, 0)]
        [Row(21, -1)]
        // (MinValue, X) Cases
        [Row(sbyte.MinValue, 0)]
        [Row(sbyte.MinValue, 1)]
        [Row(sbyte.MinValue, 17)]
        [Row(sbyte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(sbyte.MaxValue, 0)]
        [Row(sbyte.MaxValue, 1)]
        [Row(sbyte.MaxValue, 17)]
        [Row(sbyte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, sbyte.MinValue)]
        [Row(1, sbyte.MinValue)]
        [Row(17, sbyte.MinValue)]
        [Row(123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, sbyte.MaxValue)]
        [Row(1, sbyte.MaxValue)]
        [Row(17, sbyte.MaxValue)]
        [Row(123, sbyte.MaxValue)]
        // Extremvaluecases
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Row(sbyte.MaxValue, sbyte.MinValue)]
        [Row(unchecked((sbyte)0x80), 8)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool ShrI1(int expect, sbyte a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "ShrI1", a >> b, a, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I2_I2(int expect, short a, short b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(1, 2)]
        [Row(23, 3)]
        // And reverse
        [Row(2, 0)]
        [Row(21, -1)]
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
        [Row(unchecked((short)0x8000), 16)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI2(short a, short b)
        {
            CodeSource = "static class Test { static bool ShrI2(int expect, short a, byte b) { return expect == (a >> b); } }";
            int v = (a >> b);
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "ShrI2", v, a, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I4_I4(int expect, int a, int b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(1, 2)]
        [Row(23, 3)]
        // And reverse
        [Row(2, 0)]
        [Row(21, -1)]
        // (MinValue, X) Cases
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Row(unchecked((int)0x80000000), 32)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI4(int a, int b)
        {
            CodeSource = "static class Test { static bool ShrI4(int expect, int a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "ShrI4", (a >> b), a, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I8_I8_I4(long expect, long a, int b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(1, 2)]
        [Row(23, 3)]
        // And reverse
        [Row(2, 0)]
        [Row(21, -1)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        // Extremvaluecases
        [Row(long.MinValue, int.MaxValue)]
        [Row(long.MaxValue, int.MinValue)]
        [Row(unchecked((long)0x8000000000000000), 64)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI8(long a, int b)
        {
            CodeSource = "static class Test { static bool ShrI8(long expect, long a, int b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I8_I8_I4>("", "Test", "ShrI8", (a >> b), a, b));
        }
    }
}
