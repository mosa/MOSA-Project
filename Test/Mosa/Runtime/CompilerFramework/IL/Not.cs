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
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + @" a)
                    {
                        return expect == (~a);
                    }
                }";
        }
        
        #region I1
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
            CodeSource = CreateTestCode("NotI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1>("", "Test", "NotI1", (sbyte)~a, (sbyte)a));
        }
        #endregion

        #region U1
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
            CodeSource = CreateTestCode("NotU1", "byte", "byte");
            Assert.IsTrue((bool)Run<U1_U1>("", "Test", "NotU1", (byte)~(byte)a, a));
        }
        #endregion

        #region I2
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
            CodeSource = CreateTestCode("NotI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2>("", "Test", "NotI2", (short)~a, (short)a));
        }
        #endregion
        
        #region U2
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
            CodeSource = CreateTestCode("NotU2", "ushort", "ushort");
            Assert.IsTrue((bool)Run<U2_U2>("", "Test", "NotU2", (ushort)~(ushort)a, (ushort)a));
        }
        #endregion
        
        #region I4
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
            CodeSource = CreateTestCode("NotI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4>("", "Test", "NotI4", (int)~a, (int)a));
        }
        #endregion

        #region U4
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
            CodeSource = CreateTestCode("NotU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4>("", "Test", "NotU4", ~(uint)a, a));
        }
        #endregion

        #region I8
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
            CodeSource = CreateTestCode("NotI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8>("", "Test", "NotI8", (long)~a, (long)a));
        }
        #endregion

        #region U8
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
            CodeSource = CreateTestCode("NotU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8>("", "Test", "NotU8", ~(ulong)a, a));
        }
        #endregion
    }
}
