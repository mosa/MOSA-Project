/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
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
    /// 
    /// </summary>
    [TestFixture]
    public class Not : CodeDomTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I1(sbyte expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(sbyte.MinValue)]
        [Row(sbyte.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotI1(sbyte a)
        {
            CodeSource = "static class Test { static bool NotI1(int expect, sbyte a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<I4_I1>("", "Test", "NotI1", (sbyte)~a, (sbyte)a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U1_U1(byte expect, byte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(byte.MinValue)]
        [Row(byte.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotU1(byte a)
        {
            CodeSource = "static class Test { static bool NotU1(byte expect, byte a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<U1_U1>("", "Test", "NotU1", (byte)~(byte)a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I2(short expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(short.MinValue)]
        [Row(short.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotI2(short a)
        {
            CodeSource = "static class Test { static bool NotI2(int expect, short a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<I4_I2>("", "Test", "NotI2", (short)~a, (short)a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U2_U2(ushort expect, ushort a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(ushort.MinValue)]
        [Row(ushort.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotU2(ushort a)
        {
            CodeSource = "static class Test { static bool NotU2(ushort expect, ushort a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<U2_U2>("", "Test", "NotU2", ~(ushort)a, (ushort)a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I4(int expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(int.MinValue)]
        [Row(int.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotI4(int a)
        {
            CodeSource = "static class Test { static bool NotI4(int expect, int a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<I4_I4>("", "Test", "NotI4", (int)~a, (int)a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U4_U4(uint expect, uint a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(uint.MinValue)]
        [Row(uint.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotU4(uint a)
        {
            CodeSource = "static class Test { static bool NotU4(uint expect, uint a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<U4_U4>("", "Test", "NotU4", ~(uint)a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I8_I8(long expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(sbyte.MinValue)]
        [Row(sbyte.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotI8(long a)
        {
            CodeSource = "static class Test { static bool NotI8(long expect, long a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<I8_I8>("", "Test", "NotI8", (long)~a, (long)a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U8_U8(ulong expect, ulong a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(ulong.MinValue)]
        [Row(ulong.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void NotU8(ulong a)
        {
            CodeSource = "static class Test { static bool NotU8(ulong expect, ulong a) { return expect == (~a); } }";
            Assert.IsTrue((bool)Run<U8_U8>("", "Test", "NotU8", ~(ulong)a, a));
        }
    }
}
