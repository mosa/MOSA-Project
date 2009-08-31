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
    public class Or : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a | b);
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
                            return expect == (" + constLeft + @" | x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return expect == (x | " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
        #region B
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool B_B_B(bool expect, bool a, bool b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrB(bool a, bool b)
        {
            CodeSource = CreateTestCode("OrB", "bool", "bool");
            Assert.IsTrue((bool)Run<B_B_B>("", "Test", "OrB", (a | b), a, b));
        }
        
        delegate bool B_Constant_B(bool expect, bool x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrConstantBRight(bool a, bool b)
        {
            CodeSource = CreateConstantTestCode("OrConstantBRight", "bool", "bool", null, b.ToString().ToLower());
            Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "OrConstantBRight", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrConstantBLeft(bool a, bool b)
        {
            CodeSource = CreateConstantTestCode("OrConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
            Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "OrConstantBLeft", (a | b), b));
        }
        #endregion
        
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
        [TestCase(0, 0)]
        [TestCase(17, 128)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void OrC(char a, char b)
        {
            CodeSource = CreateTestCode("OrC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "OrC", (char)(a | b), a, b));
        }
        
        delegate bool C_Constant_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void OrConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("OrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "OrConstantCRight", (char)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase('a', 0)]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void OrConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("OrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "OrConstantCLeft", (char)(a | b), b));
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
        [TestCase(sbyte.MinValue, 0)]
        [TestCase(sbyte.MinValue, 1)]
        [TestCase(sbyte.MinValue, 17)]
        [TestCase(sbyte.MinValue, 123)]
        [TestCase(sbyte.MinValue, -0)]
        [TestCase(sbyte.MinValue, -1)]
        [TestCase(sbyte.MinValue, -17)]
        [TestCase(sbyte.MinValue, -123)]
        // (MaxValueee, X) Cases
        [TestCase(sbyte.MaxValue, 0)]
        [TestCase(sbyte.MaxValue, 1)]
        [TestCase(sbyte.MaxValue, 17)]
        [TestCase(sbyte.MaxValue, 123)]
        [TestCase(sbyte.MaxValue, -0)]
        [TestCase(sbyte.MaxValue, -1)]
        [TestCase(sbyte.MaxValue, -17)]
        [TestCase(sbyte.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, sbyte.MinValue)]
        [TestCase(1, sbyte.MinValue)]
        [TestCase(17, sbyte.MinValue)]
        [TestCase(123, sbyte.MinValue)]
        [TestCase(-0, sbyte.MinValue)]
        [TestCase(-1, sbyte.MinValue)]
        [TestCase(-17, sbyte.MinValue)]
        [TestCase(-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, sbyte.MaxValue)]
        [TestCase(1, sbyte.MaxValue)]
        [TestCase(17, sbyte.MaxValue)]
        [TestCase(123, sbyte.MaxValue)]
        [TestCase(-0, sbyte.MaxValue)]
        [TestCase(-1, sbyte.MaxValue)]
        [TestCase(-17, sbyte.MaxValue)]
        [TestCase(-123, sbyte.MaxValue)]
        // Extremvaluecases
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [TestCase(sbyte.MaxValue, sbyte.MinValue)]
        [Test]
        public void OrI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("OrI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "OrI1", a | b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-42, 48)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void OrConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "OrConstantI1Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-42, 48)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void OrConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "OrConstantI1Left", (a | b), b));
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
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(byte.MinValue, 0)]
        [TestCase(byte.MinValue, 1)]
        [TestCase(byte.MinValue, 17)]
        [TestCase(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(byte.MaxValue, 0)]
        [TestCase(byte.MaxValue, 1)]
        [TestCase(byte.MaxValue, 17)]
        [TestCase(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, byte.MinValue)]
        [TestCase(1, byte.MinValue)]
        [TestCase(17, byte.MinValue)]
        [TestCase(123, byte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, byte.MaxValue)]
        [TestCase(1, byte.MaxValue)]
        [TestCase(17, byte.MaxValue)]
        [TestCase(123, byte.MaxValue)]
        // Extremvaluecases
        [TestCase(byte.MinValue, byte.MaxValue)]
        [TestCase(byte.MaxValue, byte.MinValue)]
        [Test]
        public void OrU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("OrU1", "byte", "uint");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "OrU1", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void OrConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU1Right", "byte", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "OrConstantU1Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void OrConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU1Left", "byte", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "OrConstantU1Left", (uint)(a | b), b));
        }
        #endregion

        #region I2
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
        [TestCase(short.MinValue, 0)]
        [TestCase(short.MinValue, 1)]
        [TestCase(short.MinValue, 17)]
        [TestCase(short.MinValue, 123)]
        [TestCase(short.MinValue, -0)]
        [TestCase(short.MinValue, -1)]
        [TestCase(short.MinValue, -17)]
        [TestCase(short.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, 0)]
        [TestCase(short.MaxValue, 1)]
        [TestCase(short.MaxValue, 17)]
        [TestCase(short.MaxValue, 123)]
        [TestCase(short.MaxValue, -0)]
        [TestCase(short.MaxValue, -1)]
        [TestCase(short.MaxValue, -17)]
        [TestCase(short.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, short.MinValue)]
        [TestCase(1, short.MinValue)]
        [TestCase(17, short.MinValue)]
        [TestCase(123, short.MinValue)]
        [TestCase(-0, short.MinValue)]
        [TestCase(-1, short.MinValue)]
        [TestCase(-17, short.MinValue)]
        [TestCase(-123, short.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, short.MaxValue)]
        [TestCase(1, short.MaxValue)]
        [TestCase(17, short.MaxValue)]
        [TestCase(123, short.MaxValue)]
        [TestCase(-0, short.MaxValue)]
        [TestCase(-1, short.MaxValue)]
        [TestCase(-17, short.MaxValue)]
        [TestCase(-123, short.MaxValue)]
        // Extremvaluecases
        [TestCase(short.MinValue, short.MaxValue)]
        [TestCase(short.MaxValue, short.MinValue)]
        [Test]
        public void OrI2(short a, short b)
        {
            short e = (short)(a | b);
            CodeSource = CreateTestCode("OrI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "OrI2", (a | b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void OrConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "OrConstantI2Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void OrConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "OrConstantI2Left", (a | b), b));
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
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(ushort.MinValue, 0)]
        [TestCase(ushort.MinValue, 1)]
        [TestCase(ushort.MinValue, 17)]
        [TestCase(ushort.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(ushort.MaxValue, 0)]
        [TestCase(ushort.MaxValue, 1)]
        [TestCase(ushort.MaxValue, 17)]
        [TestCase(ushort.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, ushort.MinValue)]
        [TestCase(1, ushort.MinValue)]
        [TestCase(17, ushort.MinValue)]
        [TestCase(123, ushort.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, ushort.MaxValue)]
        [TestCase(1, ushort.MaxValue)]
        [TestCase(17, ushort.MaxValue)]
        [TestCase(123, ushort.MaxValue)]
        // Extremvaluecases
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [TestCase(ushort.MaxValue, ushort.MinValue)]
        [Test]
        public void OrU2(ushort a, ushort b)
        {
            ushort e = (ushort)(a | b);
            CodeSource = CreateTestCode("OrU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "OrU2", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void OrConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "OrConstantU2Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void OrConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU2Left", "ushort", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "OrConstantU2Left", (uint)(a | b), b));
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
        [TestCase(int.MaxValue, int.MinValue)]
        [Test]
        public void OrI4(int a, int b)
        {
            CodeSource = CreateTestCode("OrI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "OrI4", (a | b), a, b));
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
        public void OrConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "OrConstantI4Right", (a | b), a));
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
        public void OrConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "OrConstantI4Left", (a | b), b));
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
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(uint.MinValue, 0)]
        [TestCase(uint.MinValue, 1)]
        [TestCase(uint.MinValue, 17)]
        [TestCase(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(uint.MaxValue, 0)]
        [TestCase(uint.MaxValue, 1)]
        [TestCase(uint.MaxValue, 17)]
        [TestCase(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, uint.MinValue)]
        [TestCase(1, uint.MinValue)]
        [TestCase(17, uint.MinValue)]
        [TestCase(123, uint.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, uint.MaxValue)]
        [TestCase(1, uint.MaxValue)]
        [TestCase(17, uint.MaxValue)]
        [TestCase(123, uint.MaxValue)]
        // Extremvaluecases
        [TestCase(uint.MinValue, uint.MaxValue)]
        [TestCase(uint.MaxValue, uint.MinValue)]
        [Test]
        public void OrU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("OrU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "OrU4", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void OrConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "OrConstantU4Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void OrConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "OrConstantU4Left", (uint)(a | b), b));
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
        [TestCase(long.MinValue, 0)]
        [TestCase(long.MinValue, 1)]
        [TestCase(long.MinValue, 17)]
        [TestCase(long.MinValue, 123)]
        [TestCase(long.MinValue, -0)]
        [TestCase(long.MinValue, -1)]
        [TestCase(long.MinValue, -17)]
        [TestCase(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(long.MaxValue, 0)]
        [TestCase(long.MaxValue, 1)]
        [TestCase(long.MaxValue, 17)]
        [TestCase(long.MaxValue, 123)]
        [TestCase(long.MaxValue, -0)]
        [TestCase(long.MaxValue, -1)]
        [TestCase(long.MaxValue, -17)]
        [TestCase(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, long.MinValue)]
        [TestCase(1, long.MinValue)]
        [TestCase(17, long.MinValue)]
        [TestCase(123, long.MinValue)]
        [TestCase(-0, long.MinValue)]
        [TestCase(-1, long.MinValue)]
        [TestCase(-17, long.MinValue)]
        [TestCase(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, long.MaxValue)]
        [TestCase(1, long.MaxValue)]
        [TestCase(17, long.MaxValue)]
        [TestCase(123, long.MaxValue)]
        [TestCase(-0, long.MaxValue)]
        [TestCase(-1, long.MaxValue)]
        [TestCase(-17, long.MaxValue)]
        [TestCase(-123, long.MaxValue)]
        // Extremvaluecases
        [TestCase(long.MinValue, long.MaxValue)]
        [TestCase(long.MaxValue, long.MinValue)]
        [Test]
        public void OrI8(long a, long b)
        {
            CodeSource = CreateTestCode("OrI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "OrI8", (a | b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void OrConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "OrConstantI8Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void OrConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "OrConstantI8Left", (a | b), b));
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
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(ulong.MinValue, 0)]
        [TestCase(ulong.MinValue, 1)]
        [TestCase(ulong.MinValue, 17)]
        [TestCase(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(ulong.MaxValue, 0)]
        [TestCase(ulong.MaxValue, 1)]
        [TestCase(ulong.MaxValue, 17)]
        [TestCase(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, ulong.MinValue)]
        [TestCase(1, ulong.MinValue)]
        [TestCase(17, ulong.MinValue)]
        [TestCase(123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, ulong.MaxValue)]
        [TestCase(1, ulong.MaxValue)]
        [TestCase(17, ulong.MaxValue)]
        [TestCase(123, ulong.MaxValue)]
        // Extremvaluecases
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [TestCase(ulong.MaxValue, ulong.MinValue)]
        [Test]
        public void OrU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCode("OrU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "OrU8", (ulong)(a | b), a, b));
        }
        
        delegate bool U8_Constant_U8(ulong expect, ulong x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void OrConstantU8Right(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU8Right", "ulong", "ulong", null, b.ToString());
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "OrConstantU8Right", (ulong)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void OrConstantU8Left(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU8Left", "ulong", "ulong", a.ToString(), null);
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "OrConstantU8Left", (ulong)(a | b), b));
        }
        #endregion
    }
}
