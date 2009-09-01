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
using System.Runtime.InteropServices;
using NUnit.Framework;

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
        
        #region C
        
        delegate bool I4_C(int expect, [MarshalAs(UnmanagedType.U2)]char a);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((char)0)]
        [TestCase((char)1)]
        [TestCase((char)2)]
        [TestCase((char)5)]
        [TestCase('a')]
        [TestCase('Z')]
        [TestCase((char)100)]
        [TestCase(char.MaxValue)]
        [Test]
        public void NotC(char a)
        {
            CodeSource = CreateTestCode("NotC", "char", "int");
            Assert.IsTrue((bool)Run<I4_C>("", "Test", "NotC", (int)~a, a));
        }
        #endregion
        
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
        [TestCase((sbyte)0)]
        [TestCase((sbyte)1)]
        [TestCase((sbyte)2)]
        [TestCase((sbyte)5)]
        [TestCase((sbyte)10)]
        [TestCase((sbyte)11)]
        [TestCase((sbyte)100)]
        [TestCase((sbyte)-0)]
        [TestCase((sbyte)-1)]
        [TestCase((sbyte)-2)]
        [TestCase((sbyte)-5)]
        [TestCase((sbyte)-10)]
        [TestCase((sbyte)-11)]
        [TestCase((sbyte)-100)]
        [TestCase(sbyte.MinValue)]
        [TestCase(sbyte.MaxValue)]
        [Test]
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
        [TestCase((byte)0)]
        [TestCase((byte)1)]
        [TestCase((byte)2)]
        [TestCase((byte)5)]
        [TestCase((byte)10)]
        [TestCase((byte)11)]
        [TestCase((byte)100)]
        [TestCase(byte.MinValue)]
        [TestCase(byte.MaxValue)]
        [Test]
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
        [TestCase((short)0)]
        [TestCase((short)1)]
        [TestCase((short)2)]
        [TestCase((short)5)]
        [TestCase((short)10)]
        [TestCase((short)11)]
        [TestCase((short)100)]
        [TestCase((short)-0)]
        [TestCase((short)-1)]
        [TestCase((short)-2)]
        [TestCase((short)-5)]
        [TestCase((short)-10)]
        [TestCase((short)-11)]
        [TestCase((short)-100)]
        [TestCase(short.MinValue)]
        [TestCase(short.MaxValue)]
        [Test]
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
        [TestCase((ushort)0)]
        [TestCase((ushort)1)]
        [TestCase((ushort)2)]
        [TestCase((ushort)5)]
        [TestCase((ushort)10)]
        [TestCase((ushort)11)]
        [TestCase((ushort)100)]
        [TestCase(ushort.MinValue)]
        [TestCase(ushort.MaxValue)]
        [Test]
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
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(100)]
        [TestCase(-0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(-10)]
        [TestCase(-11)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [Test]
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
        [TestCase((uint)0)]
        [TestCase((uint)1)]
        [TestCase((uint)2)]
        [TestCase((uint)5)]
        [TestCase((uint)10)]
        [TestCase((uint)11)]
        [TestCase((uint)100)]
        [TestCase(uint.MinValue)]
        [TestCase(uint.MaxValue)]
        [Test]
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
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(100)]
        [TestCase(-0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(-10)]
        [TestCase(-11)]
        [TestCase(-100)]
        [TestCase(sbyte.MinValue)]
        [TestCase(sbyte.MaxValue)]
        [Test]
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
        [TestCase((ulong)0)]
        [TestCase((ulong)1)]
        [TestCase((ulong)2)]
        [TestCase((ulong)5)]
        [TestCase((ulong)10)]
        [TestCase((ulong)11)]
        [TestCase((ulong)100)]
        [TestCase(ulong.MinValue)]
        [TestCase(ulong.MaxValue)]
        [Test]
        public void NotU8(ulong a)
        {
            CodeSource = CreateTestCode("NotU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8>("", "Test", "NotU8", ~(ulong)a, a));
        }
        #endregion
    }
}
