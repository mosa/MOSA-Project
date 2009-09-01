/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConvI8 : CodeDomTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI8_I1(long expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((sbyte)0), TestCase((sbyte)1), TestCase((sbyte)2), TestCase(sbyte.MinValue), TestCase(sbyte.MaxValue)]
        [Test]
        public void ConvI8_I1(sbyte a)
        {
            CodeSource = "static class Test { static bool ConvI8_I1(long expect, sbyte a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I1>("", "Test", "ConvI8_I1", ((long)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI8_I2(long expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((short)0), TestCase((short)1), TestCase((short)2), TestCase(short.MinValue), TestCase(short.MaxValue)]
        [Test]
        public void ConvI8_I2(short a)
        {
            CodeSource = "static class Test { static bool ConvI8_I2(long expect, short a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I2>("", "Test", "ConvI8_I2", ((long)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI8_I4(long expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(int.MinValue), TestCase(int.MaxValue)]
        [Test]
        public void ConvI8_I4(int a)
        {
            CodeSource = "static class Test { static bool ConvI8_I4(long expect, int a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I4>("", "Test", "ConvI8_I4", ((long)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI8_I8(long expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(long.MinValue), TestCase(long.MaxValue)]
        [Test]
        public void ConvI8_I8(long a)
        {
            CodeSource = "static class Test { static bool ConvI8_I8(long expect, long a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I8>("", "Test", "ConvI8_I8", ((long)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI4_R4(int expect, float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(long.MinValue), TestCase(long.MaxValue)]
        [Test]
        public void ConvI8_R4(float a)
        {
            CodeSource = "static class Test { static bool ConvI1_R4(int expect, float a) { return expect == ((int)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI4_R4>("", "Test", "ConvI1_R4", ((int)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI4_R8(int expect, double a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(long.MinValue), TestCase(long.MaxValue)]
        [Test]
        public void ConvI8_R8(double a)
        {
            CodeSource = "static class Test { static bool ConvI1_R8(int expect, double a) { return expect == ((int)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI4_R8>("", "Test", "ConvI1_R8", ((int)a), a));
        }
    }
}
