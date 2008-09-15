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
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConvI1 : MosaCompilerTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I1(sbyte expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_I1(sbyte a)
        {
            CodeSource = "static class Test { static bool ConvI1_I1(sbyte expect, sbyte a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I1>("", "Test", "ConvI1_I1", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I2(sbyte expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_I2(short a)
        {
            CodeSource = "static class Test { static bool ConvI1_I2(sbyte expect, short a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I2>("", "Test", "ConvI1_I2", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I4(sbyte expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_I4(int a)
        {
            CodeSource = "static class Test { static bool ConvI1_I4(sbyte expect, int a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I4>("", "Test", "ConvI1_I4", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I8(sbyte expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_I8(long a)
        {
            CodeSource = "static class Test { static bool ConvI1_I8(sbyte expect, long a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I4>("", "Test", "ConvI1_I8", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_R4(sbyte expect, float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_R4(float a)
        {
            CodeSource = "static class Test { static bool ConvI1_R4(sbyte expect, float a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I4>("", "Test", "ConvI1_R4", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_R8(sbyte expect, double a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI1_R8(double a)
        {
            CodeSource = "static class Test { static bool ConvI1_R8(sbyte expect, double a) { return expect == (sbyte)a; } }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I4>("", "Test", "ConvI1_R8", ((sbyte)a), a));
        }
    }
}
