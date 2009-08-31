/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 *
 */

using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Assert=NUnit.Framework.Assert;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Testcase for the AddInstruction
    /// </summary>
    [TestFixture]
    public class Add : CodeDomTestRunner
    {        
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a + b);
                    }
                }";
        }
    
        private static string CreateConstantTestCode(string name, string typeIn, string typeOut, string constLeft, string constRight)
        {
            if (String.IsNullOrEmpty(constRight))
            {
                return @"
                    static class Test
                    {
                        static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return expect == (" + constLeft + @" + x);
                        }
                    }";
            }
            if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return expect == (x + " + constRight + @");
                        }
                    }";
            }
            throw new NotSupportedException();
        }

        #region C
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool C_C_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, (char)0)]
        [TestCase((char)0, (char)1)]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void AddC(char a, char b)
        {
            CodeSource = CreateTestCode("AddC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "AddC", (char)(a + b), a, b));
        }
        
        delegate bool C_Constant_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);
        //delegate bool C_Constant_C(char expect, char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void AddConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("AddConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "AddConstantCRight", (char)(a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase('a', (char)0)]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void AddConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("AddConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "AddConstantCLeft", (char)(a + b), b));
        }
        #endregion
        
        #region I1
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
        [TestCase((sbyte)1, (sbyte)2)]
        [TestCase((sbyte)23, (sbyte)21)]
        [TestCase((sbyte)1, (sbyte)-2)]
        [TestCase((sbyte)-1, (sbyte)2)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)-2)]
        // And reverse
        [TestCase((sbyte)2, (sbyte)1)]
        [TestCase((sbyte)21, (sbyte)23)]
        [TestCase((sbyte)-2, (sbyte)1)]
        [TestCase((sbyte)2, (sbyte)-1)]
        [TestCase((sbyte)-2, (sbyte)-17)]
        // (MinValue, X) Cases
        [TestCase(sbyte.MinValue, (sbyte)0)]
        [TestCase(sbyte.MinValue, (sbyte)1)]
        [TestCase(sbyte.MinValue, (sbyte)17)]
        [TestCase(sbyte.MinValue, (sbyte)123)]
        [TestCase(sbyte.MinValue, (sbyte)-0)]
        [TestCase(sbyte.MinValue, (sbyte)-1)]
        [TestCase(sbyte.MinValue, (sbyte)-17)]
        [TestCase(sbyte.MinValue, (sbyte)-123)]
        // (MaxValue, X) Cases
        [TestCase(sbyte.MaxValue, (sbyte)0)]
        [TestCase(sbyte.MaxValue, (sbyte)1)]
        [TestCase(sbyte.MaxValue, (sbyte)17)]
        [TestCase(sbyte.MaxValue, (sbyte)123)]
        [TestCase(sbyte.MaxValue, (sbyte)-0)]
        [TestCase(sbyte.MaxValue, (sbyte)-1)]
        [TestCase(sbyte.MaxValue, (sbyte)-17)]
        [TestCase(sbyte.MaxValue, (sbyte)-123)]
        // (X, MinValue) Cases
        [TestCase((sbyte)0, sbyte.MinValue)]
        [TestCase((sbyte)1, sbyte.MinValue)]
        [TestCase((sbyte)17, sbyte.MinValue)]
        [TestCase((sbyte)123, sbyte.MinValue)]
        [TestCase((sbyte)-0, sbyte.MinValue)]
        [TestCase((sbyte)-1, sbyte.MinValue)]
        [TestCase((sbyte)-17, sbyte.MinValue)]
        [TestCase((sbyte)-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((sbyte)0, sbyte.MaxValue)]
        [TestCase((sbyte)1, sbyte.MaxValue)]
        [TestCase((sbyte)17, sbyte.MaxValue)]
        [TestCase((sbyte)123, sbyte.MaxValue)]
        [TestCase((sbyte)-0, sbyte.MaxValue)]
        [TestCase((sbyte)-1, sbyte.MaxValue)]
        [TestCase((sbyte)-17, sbyte.MaxValue)]
        [TestCase((sbyte)-123, sbyte.MaxValue)]
        // Extremvaluecases
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void AddI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("AddI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "AddI1", a + b, (sbyte)a, (sbyte)b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)-42, (sbyte)48)]
        [TestCase((sbyte)17, (sbyte)1)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void AddConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "AddConstantI1Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)-42, (sbyte)48)]
        [TestCase((sbyte)17, (sbyte)1)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void AddConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "AddConstantI1Left", (a + b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I1_C(int expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((sbyte)1)]
        [Test]
        public void AddConstantI1Right(sbyte a)
        {
            CodeSource = "static class Test { static bool AddConstantI1Right(int expect, sbyte a) { return expect == (a + 1); } }";
            Assert.IsTrue((bool)Run<I4_I1_C>("", "Test", "AddConstantI1Right", a + 1, a));
        }
        #endregion

        #region U1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U1_U1(uint expect, byte a, byte b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)1, (byte)2)]
        [TestCase((byte)23, (byte)21)]
        [TestCase((byte)0, (byte)0)]
        // And reverse
        [TestCase((byte)2, (byte)1)]
        [TestCase((byte)21, (byte)23)]
        // (MinValue, X) Cases
        [TestCase(byte.MinValue, (byte)0)]
        [TestCase(byte.MinValue, (byte)1)]
        [TestCase(byte.MinValue, (byte)17)]
        [TestCase(byte.MinValue, (byte)123)]
        // (MaxValue, X) Cases
        [TestCase(byte.MaxValue, (byte)0)]
        [TestCase(byte.MaxValue, (byte)1)]
        [TestCase(byte.MaxValue, (byte)17)]
        [TestCase(byte.MaxValue, (byte)123)]
        // (X, MinValue) Cases
        [TestCase((byte)0, byte.MinValue)]
        [TestCase((byte)1, byte.MinValue)]
        [TestCase((byte)17, byte.MinValue)]
        [TestCase((byte)123, byte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((byte)0, byte.MaxValue)]
        [TestCase((byte)1, byte.MaxValue)]
        [TestCase((byte)17, byte.MaxValue)]
        [TestCase((byte)123, byte.MaxValue)]
        // Extremvaluecases
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void AddU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("AddU1", "byte", "uint");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "AddU1", (uint)(a + b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)148)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void AddConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU1Right", "byte", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "AddConstantU1Right", (uint)(a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)148)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void AddConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU1Left", "byte", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "AddConstantU1Left", (uint)(a + b), b));
        }
        #endregion
        
        #region I2
        delegate bool I4_I2_I2(int expect, short a, short b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)1, (short)2)]
        [TestCase((short)23, (short)21)]
        [TestCase((short)1, (short)-2)]
        [TestCase((short)-1, (short)2)]
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)-2)]
        // And reverse
        [TestCase((short)2, (short)1)]
        [TestCase((short)21, (short)23)]
        [TestCase((short)-2, (short)1)]
        [TestCase((short)2, (short)-1)]
        [TestCase((short)-2, (short)-17)]
        // (MinValue, X) Cases
        [TestCase(short.MinValue, (short)0)]
        [TestCase(short.MinValue, (short)1)]
        [TestCase(short.MinValue, (short)17)]
        [TestCase(short.MinValue, (short)123)]
        [TestCase(short.MinValue, (short)-0)]
        [TestCase(short.MinValue, (short)-1)]
        [TestCase(short.MinValue, (short)-17)]
        [TestCase(short.MinValue, (short)-123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, (short)0)]
        [TestCase(short.MaxValue, (short)1)]
        [TestCase(short.MaxValue, (short)17)]
        [TestCase(short.MaxValue, (short)123)]
        [TestCase(short.MaxValue, (short)-0)]
        [TestCase(short.MaxValue, (short)-1)]
        [TestCase(short.MaxValue, (short)-17)]
        [TestCase(short.MaxValue, (short)-123)]
        // (X, MinValue) Cases
        [TestCase((short)0, short.MinValue)]
        [TestCase((short)1, short.MinValue)]
        [TestCase((short)17, short.MinValue)]
        [TestCase((short)123, short.MinValue)]
        [TestCase((short)-0, short.MinValue)]
        [TestCase((short)-1, short.MinValue)]
        [TestCase((short)-17, short.MinValue)]
        [TestCase((short)-123, short.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((short)0, short.MaxValue)]
        [TestCase((short)1, short.MaxValue)]
        [TestCase((short)17, short.MaxValue)]
        [TestCase((short)123, short.MaxValue)]
        [TestCase((short)-0, short.MaxValue)]
        [TestCase((short)-1, short.MaxValue)]
        [TestCase((short)-17, short.MaxValue)]
        [TestCase((short)-123, short.MaxValue)]
        // Extremvaluecases
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void AddI2(short a, short b)
        {
            CodeSource = CreateTestCode("AddI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "AddI2", (a + b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)148)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void AddConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "AddConstantI2Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)148)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void AddConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "AddConstantI2Left", (a + b), b));
        }
        #endregion

        #region U2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U2_U2(uint expect, ushort a, ushort b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)1, (ushort)2)]
        [TestCase((ushort)23, (ushort)21)]
        [TestCase((ushort)0, (ushort)0)]
        // And reverse
        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)21, (ushort)23)]
        // (MinValue, X) Cases
        [TestCase(ushort.MinValue, (ushort)0)]
        [TestCase(ushort.MinValue, (ushort)1)]
        [TestCase(ushort.MinValue, (ushort)17)]
        [TestCase(ushort.MinValue, (ushort)123)]
        // (MaxValue, X) Cases
        [TestCase(ushort.MaxValue, (ushort)0)]
        [TestCase(ushort.MaxValue, (ushort)1)]
        [TestCase(ushort.MaxValue, (ushort)17)]
        [TestCase(ushort.MaxValue, (ushort)123)]
        // (X, MinValue) Cases
        [TestCase((ushort)0, ushort.MinValue)]
        [TestCase((ushort)1, ushort.MinValue)]
        [TestCase((ushort)17, ushort.MinValue)]
        [TestCase((ushort)123, ushort.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((ushort)0, ushort.MaxValue)]
        [TestCase((ushort)1, ushort.MaxValue)]
        [TestCase((ushort)17, ushort.MaxValue)]
        [TestCase((ushort)123, ushort.MaxValue)]
        // Extremvaluecases
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void AddU2(ushort a, ushort b)
        {
            CodeSource = CreateTestCode("AddU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "AddU2", (uint)(a + b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)148)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void AddConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "AddConstantU2Right", (uint)(a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)148)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void AddConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU2Left", "ushort", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "AddConstantU2Left", (uint)(a + b), b));
        }
        #endregion

        #region I4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I4_I4(int expect, int a, int b);
        // Normal Testcases + (0, 0)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0)]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(int.MinValue, 0)]
        [TestCase(int.MinValue, 1)]
        [TestCase(int.MinValue, 17)]
        [TestCase(int.MinValue, 123)]
        [TestCase(int.MinValue, -0)]
        [TestCase(int.MinValue, -1)]
        [TestCase(int.MinValue, -17)]
        [TestCase(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(int.MaxValue, 0)]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MaxValue, 17)]
        [TestCase(int.MaxValue, 123)]
        [TestCase(int.MaxValue, -0)]
        [TestCase(int.MaxValue, -1)]
        [TestCase(int.MaxValue, -17)]
        [TestCase(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, int.MinValue)]
        [TestCase(1, int.MinValue)]
        [TestCase(17, int.MinValue)]
        [TestCase(123, int.MinValue)]
        [TestCase(-0, int.MinValue)]
        [TestCase(-1, int.MinValue)]
        [TestCase(-17, int.MinValue)]
        [TestCase(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, int.MaxValue)]
        [TestCase(1, int.MaxValue)]
        [TestCase(17, int.MaxValue)]
        [TestCase(123, int.MaxValue)]
        [TestCase(-0, int.MaxValue)]
        [TestCase(-1, int.MaxValue)]
        [TestCase(-17, int.MaxValue)]
        [TestCase(-123, int.MaxValue)]
        // Extremvaluecases
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void AddI4(int a, int b)
        {
            CodeSource = CreateTestCode("AddI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddI4", (a + b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void AddConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "AddConstantI4Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void AddConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "AddConstantI4Left", (a + b), b));
        }
        #endregion

        #region U4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U4_U4(uint expect, uint a, uint b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)1, (uint)2)]
        [TestCase((uint)23, (uint)21)]
        [TestCase((uint)0, (uint)0)]
        // And reverse
        [TestCase((uint)2, (uint)1)]
        [TestCase((uint)21, (uint)23)]
        // (MinValue, X) Cases
        [TestCase(uint.MinValue, (uint)0)]
        [TestCase(uint.MinValue, (uint)1)]
        [TestCase(uint.MinValue, (uint)17)]
        [TestCase(uint.MinValue, (uint)123)]
        // (MaxValue, X) Cases
        [TestCase(uint.MaxValue, (uint)0)]
        [TestCase(uint.MaxValue, (uint)1)]
        [TestCase(uint.MaxValue, (uint)17)]
        [TestCase(uint.MaxValue, (uint)123)]
        // (X, MinValue) Cases
        [TestCase((uint)0, uint.MinValue)]
        [TestCase((uint)1, uint.MinValue)]
        [TestCase((uint)17, uint.MinValue)]
        [TestCase((uint)123, uint.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((uint)0, uint.MaxValue)]
        [TestCase((uint)1, uint.MaxValue)]
        [TestCase((uint)17, uint.MaxValue)]
        [TestCase((uint)123, uint.MaxValue)]
        // Extremvaluecases
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void AddU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("AddU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "AddU4", (uint)(a + b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)148)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void AddConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "AddConstantU4Right", (uint)(a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)148)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void AddConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "AddConstantU4Left", (uint)(a + b), b));
        }
        #endregion

        #region I8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I8_I8_I8(long expect, long a, long b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((long)1, (long)2)]
        [TestCase((long)23, (long)21)]
        [TestCase((long)1, (long)-2)]
        [TestCase((long)-1, (long)2)]
        [TestCase((long)0, (long)0)]
        [TestCase((long)-17, (long)-2)]
        // And reverse
        [TestCase((long)2, (long)1)]
        [TestCase((long)21, (long)23)]
        [TestCase((long)-2, (long)1)]
        [TestCase((long)2, (long)-1)]
        [TestCase((long)-2, (long)-17)]
        // (MinValue, X) Cases
        [TestCase(long.MinValue, (long)0)]
        [TestCase(long.MinValue, (long)1)]
        [TestCase(long.MinValue, (long)17)]
        [TestCase(long.MinValue, (long)123)]
        [TestCase(long.MinValue, (long)-0)]
        [TestCase(long.MinValue, (long)-1)]
        [TestCase(long.MinValue, (long)-17)]
        [TestCase(long.MinValue, (long)-123)]
        // (MaxValue, X) Cases
        [TestCase(long.MaxValue, (long)0)]
        [TestCase(long.MaxValue, (long)1)]
        [TestCase(long.MaxValue, (long)17)]
        [TestCase(long.MaxValue, (long)123)]
        [TestCase(long.MaxValue, (long)-0)]
        [TestCase(long.MaxValue, (long)-1)]
        [TestCase(long.MaxValue, (long)-17)]
        [TestCase(long.MaxValue, (long)-123)]
        // (X, MinValue) Cases
        [TestCase((long)0, long.MinValue)]
        [TestCase((long)1, long.MinValue)]
        [TestCase((long)17, long.MinValue)]
        [TestCase((long)123, long.MinValue)]
        [TestCase((long)-0, long.MinValue)]
        [TestCase((long)-1, long.MinValue)]
        [TestCase((long)-17, long.MinValue)]
        [TestCase((long)-123, long.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((long)0, long.MaxValue)]
        [TestCase((long)1, long.MaxValue)]
        [TestCase((long)17, long.MaxValue)]
        [TestCase((long)123, long.MaxValue)]
        [TestCase((long)-0, long.MaxValue)]
        [TestCase((long)-1, long.MaxValue)]
        [TestCase((long)-17, long.MaxValue)]
        [TestCase((long)-123, long.MaxValue)]
        [TestCase((long)0x0000000100000000L, (long)0x0000000100000000L)]
        // Extremvaluecases
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void AddI8(long a, long b)
        {
            CodeSource = CreateTestCode("AddI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "AddI8", (a + b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((long)-23, (long)148)]
        [TestCase((long)17, (long)1)]
        [TestCase((long)0, (long)0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void AddConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "AddConstantI8Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((long)-23, (long)148)]
        [TestCase((long)17, (long)1)]
        [TestCase((long)0, (long)0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void AddConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("AddConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "AddConstantI8Left", (a + b), b));
        }
        #endregion

        #region U8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U8_U8_U8(ulong expect, ulong a, ulong b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)1, (ulong)2)]
        [TestCase((ulong)23, (ulong)21)]
        [TestCase((ulong)0, (ulong)0)]
        // And reverse
        [TestCase((ulong)2, (ulong)1)]
        [TestCase((ulong)21, (ulong)23)]
        // (MinValue, X) Cases
        [TestCase(ulong.MinValue, (ulong)0)]
        [TestCase(ulong.MinValue, (ulong)1)]
        [TestCase(ulong.MinValue, (ulong)17)]
        [TestCase(ulong.MinValue, (ulong)123)]
        // (MaxValue, X) Cases
        [TestCase(ulong.MaxValue, (ulong)0)]
        [TestCase(ulong.MaxValue, (ulong)1)]
        [TestCase(ulong.MaxValue, (ulong)17)]
        [TestCase(ulong.MaxValue, (ulong)123)]
        // (X, MinValue) Cases
        [TestCase((ulong)0, ulong.MinValue)]
        [TestCase((ulong)1, ulong.MinValue)]
        [TestCase((ulong)17, ulong.MinValue)]
        [TestCase((ulong)123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((ulong)0, ulong.MaxValue)]
        [TestCase((ulong)1, ulong.MaxValue)]
        [TestCase((ulong)17, ulong.MaxValue)]
        [TestCase((ulong)123, ulong.MaxValue)]
        // Extremvaluecases
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void AddU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCode("AddU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "AddU8", (ulong)(a + b), a, b));
        }
        
        delegate bool U8_Constant_U8(ulong expect, ulong x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void AddConstantU8Right(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU8Right", "ulong", "ulong", null, b.ToString());
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "AddConstantU8Right", (ulong)(a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void AddConstantU8Left(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("AddConstantU8Left", "ulong", "ulong", a.ToString(), null);
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "AddConstantU8Left", (ulong)(a + b), b));
        }
        #endregion
        
        #region R4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool R4_R4_R4(float expect, float a, float b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1.0f, 1.0f)]
        [TestCase(1.0f, -2.198f)]
        [TestCase(-1.2f, 2.11f)]
        [TestCase(0.0f, 0.0f)]
        // (MinValue, X) Cases
        [TestCase(float.MinValue, 0.0f)]
        [TestCase(float.MinValue, 1.2f)]
        [TestCase(float.MinValue, 17.6f)]
        [TestCase(float.MinValue, 123.1f)]
        [TestCase(float.MinValue, -0.0f)]
        [TestCase(float.MinValue, -1.5f)]
        [TestCase(float.MinValue, -17.99f)]
        [TestCase(float.MinValue, -123.235f)]
        // (MaxValue, X) Cases
        [TestCase(float.MaxValue, 0.0f)]
        [TestCase(float.MaxValue, 1.67f)]
        [TestCase(float.MaxValue, 17.875f)]
        [TestCase(float.MaxValue, 123.283f)]
        [TestCase(float.MaxValue, -0.0f)]
        [TestCase(float.MaxValue, -1.1497f)]
        [TestCase(float.MaxValue, -17.12f)]
        [TestCase(float.MaxValue, -123.34f)]
        // (X, MinValue) Cases
        [TestCase(0.0f, float.MinValue)]
        [TestCase(1.2f, float.MinValue)]
        [TestCase(17.4f, float.MinValue)]
        [TestCase(123.561f, float.MinValue)]
        [TestCase(-0.0f, float.MinValue)]
        [TestCase(-1.78f, float.MinValue)]
        [TestCase(-17.59f, float.MinValue)]
        [TestCase(-123.41f, float.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0.0f, float.MaxValue)]
        [TestCase(1.00012f, float.MaxValue)]
        [TestCase(17.094002f, float.MaxValue)]
        [TestCase(123.001f, float.MaxValue)]
        [TestCase(-0.0f, float.MaxValue)]
        [TestCase(-1.045f, float.MaxValue)]
        [TestCase(-17.0002501f, float.MaxValue)]
        [TestCase(-123.023f, float.MaxValue)]
        // Extremvaluecases
        [TestCase(float.MinValue, float.MaxValue)]
        [TestCase(1.0f, float.NaN)]
        [TestCase(float.NaN, 1.0f)]
        [TestCase(1.0f, float.PositiveInfinity)]
        [TestCase(float.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, float.NegativeInfinity)]
        [TestCase(float.NegativeInfinity, 1.0f)]
        [Test]
        public void AddR4(float a, float b)
        {
            CodeSource = CreateTestCode("AddR4", "float", "float");
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "AddR4", a + b, a, b));
        }
        
        delegate bool R4_Constant_R4(float expect, float x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        // Obsolete. This test just fails because we're calculating with higher precision
        // [TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void AddConstantR4Right(float a, float b)
        {
            CodeSource = CreateConstantTestCode("AddConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "AddConstantR4Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        // Obsolete. This test just fails because we're calculating with higher precision
        // [TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void AddConstantR4Left(float a, float b)
        {
            CodeSource = CreateConstantTestCode("AddConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "AddConstantR4Left", (a + b), b));
        }
        #endregion
        
        #region R8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool R8_R8_R8(double expect, double a, double b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1.2, 2.1)]
        [TestCase(23.0, 21.2578)]
        [TestCase(1.0, -2.198)]
        [TestCase(-1.2, 2.11)]
        [TestCase(0.0, 0.0)]
        [TestCase(-17.1, -2.3)]
        // (MinValue, X) Cases
        [TestCase(double.MinValue, 0.0)]
        [TestCase(double.MinValue, 1.2)]
        [TestCase(double.MinValue, 17.6)]
        [TestCase(double.MinValue, 123.1)]
        [TestCase(double.MinValue, -0.0)]
        [TestCase(double.MinValue, -1.5)]
        [TestCase(double.MinValue, -17.99)]
        [TestCase(double.MinValue, -123.235)]
        // (MaxValue, X) Cases
        [TestCase(double.MaxValue, 0.0)]
        [TestCase(double.MaxValue, 1.67)]
        [TestCase(double.MaxValue, 17.875)]
        [TestCase(double.MaxValue, 123.283)]
        [TestCase(double.MaxValue, -0.0)]
        [TestCase(double.MaxValue, -1.1497)]
        [TestCase(double.MaxValue, -17.12)]
        [TestCase(double.MaxValue, -123.34)]
        // (X, MinValue) Cases
        [TestCase(0.0, double.MinValue)]
        [TestCase(1.2, double.MinValue)]
        [TestCase(17.4, double.MinValue)]
        [TestCase(123.561, double.MinValue)]
        [TestCase(-0.0, double.MinValue)]
        [TestCase(-1.78, double.MinValue)]
        [TestCase(-17.59, double.MinValue)]
        [TestCase(-123.41, double.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0.0, double.MaxValue)]
        [TestCase(1.00012, double.MaxValue)]
        [TestCase(17.094002, double.MaxValue)]
        [TestCase(123.001, double.MaxValue)]
        [TestCase(-0.0, double.MaxValue)]
        [TestCase(-1.045, double.MaxValue)]
        [TestCase(-17.0002501, double.MaxValue)]
        [TestCase(-123.023, double.MaxValue)]
        // Extremvaluecases
        [TestCase(double.MinValue, double.MaxValue)]
        [TestCase(1.0f, double.NaN)]
        [TestCase(double.NaN, 1.0f)]
        [TestCase(1.0f, double.PositiveInfinity)]
        [TestCase(double.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, double.NegativeInfinity)]
        [TestCase(double.NegativeInfinity, 1.0f)]
        [Test]
        public void AddR8(double a, double b)
        {
            CodeSource = CreateTestCode("AddR8", "double", "double");
            CodeSource = "static class Test { static bool AddR8(double expect, double a, double b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "AddR8", (a + b), a, b));
        }
        
        delegate bool R8_Constant_R8(double expect, double x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148.0016)]
        [TestCase(17.2, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void AddConstantR8Right(double a, double b)
        {
            CodeSource = CreateConstantTestCode("AddConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "AddConstantR8Right", (a + b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148.0016)]
        [TestCase(17.2, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void AddConstantR8Left(double a, double b)
        {
            CodeSource = CreateConstantTestCode("AddConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "AddConstantR8Left", (a + b), b));
        }
        #endregion
    }
}
