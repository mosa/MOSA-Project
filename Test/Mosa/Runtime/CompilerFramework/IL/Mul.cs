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
    public class Mul : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a * b);
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
                            return expect == (" + constLeft + @" * x);
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
                            return expect == (x * " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
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
        [TestCase(0, 0)]
        [TestCase(17, 128)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void MulC(char a, char b)
        {
            CodeSource = CreateTestCode("MulC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "MulC", (char)(a * b), a, b));
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
        public void MulConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("MulConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "MulConstantCRight", (char)(a * b), a));
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
        public void MulConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("MulConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "MulConstantCLeft", (char)(a * b), b));
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
        // (MaxValue, X) Cases
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
        public void MulI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("MulI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "MulI1", a * b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(2, -17)]
        [TestCase(0, 0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void MulConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "MulConstantI1Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(2, -17)]
        [TestCase(0, 0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void MulConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "MulConstantI1Left", (a * b), b));
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
        [TestCase(0, 0)]
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
        public void MulU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("MulU1", "byte", "uint");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "MulU1", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void MulConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU1Right", "byte", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "MulConstantU1Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void MulConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU1Left", "byte", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "MulConstantU1Left", (uint)(a * b), b));
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
        public void MulI2(short a, short b)
        {
            CodeSource = CreateTestCode("MulI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "MulI2", (a * b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void MulConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "MulConstantI2Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void MulConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "MulConstantI2Left", (a * b), b));
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
        [TestCase(0, 0)]
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
        public void MulU2(ushort a, ushort b)
        {
            CodeSource = CreateTestCode("MulU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "MulU2", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void MulConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "MulConstantU2Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(23, 148)] 
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void MulConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU2Left", "ushort", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "MulConstantU2Left", (uint)(a * b), b));
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
        public void MulI4(int a, int b)
        {
            CodeSource = CreateTestCode("MulI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "MulI4", (a * b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)] 
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void MulConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "MulConstantI4Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void MulConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "MulConstantI4Left", (a * b), b));
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
        [TestCase(0, 0)]
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
        public void MulU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("MulU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "MulU4", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void MulConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "MulConstantU4Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(23, 148)] 
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void MulConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "MulConstantU4Left", (uint)(a * b), b));
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
        [Test]
        public void MulI8(long a, long b)
        {
            CodeSource = CreateTestCode("MulI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "MulI8", (a * b), a, b));
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
        [TestCase(-123, long.MaxValue)]
        [Test]
        public void MulConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "MulConstantI8Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(-123, long.MaxValue)]
        [Test]
        public void MulConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("MulConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "MulConstantI8Left", (a * b), b));
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
        [TestCase(0, 0)]
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
        [Test]
        public void MulU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCode("MulU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "MulU8", (ulong)(a * b), a, b));
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
        [TestCase(1, ulong.MaxValue)]
        [Test]
        public void MulConstantU8Right(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU8Right", "ulong", "ulong", null, b.ToString());
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "MulConstantU8Right", (ulong)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(1, ulong.MaxValue)]
        [Test]
        public void MulConstantU8Left(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("MulConstantU8Left", "ulong", "ulong", a.ToString(), null);
            // left side constant
            CodeSource = "static class Test { static bool MulConstantU8Left(ulong expect, ulong b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "MulConstantU8Left", (ulong)(a * b), b));
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
        [TestCase(1.0f, 2.0f)]
        [TestCase(2.0f, 0.0f)]
        [TestCase(1.0f, float.NaN)]
        [TestCase(float.NaN, 1.0f)]
        [TestCase(1.0f, float.PositiveInfinity)]
        [TestCase(float.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, float.NegativeInfinity)]
        [TestCase(float.NegativeInfinity, 1.0f)]
        [Test]
        public void MulR4(float a, float b)
        {
            CodeSource = CreateTestCode("MulR4", "float", "float");
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "MulR4", (a * b), a, b));
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
        [TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void MulConstantR4Right(float a, float b)
        {
            CodeSource = CreateConstantTestCode("MulConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "MulConstantR4Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        [TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void MulConstantR4Left(float a, float b)
        {
            CodeSource = CreateConstantTestCode("MulConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "MulConstantR4Left", (a * b), b));
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
        [TestCase(1.0, 2.0)]
        [TestCase(2.0, 0.0)]
        [TestCase(1.0, double.NaN)]
        [TestCase(double.NaN, 1.0)]
        [TestCase(1.0, double.PositiveInfinity)]
        [TestCase(double.PositiveInfinity, 1.0)]
        [TestCase(1.0, double.NegativeInfinity)]
        [TestCase(double.NegativeInfinity, 1.0)]
        [Test]
        public void MulR8(double a, double b)
        {
            CodeSource = CreateTestCode("MulR8", "double", "double");
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "MulR8", (a * b), a, b));
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
        public void MulConstantR8Right(double a, double b)
        {
            CodeSource = CreateConstantTestCode("MulConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "MulConstantR8Right", (a * b), a));
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
        public void MulConstantR8Left(double a, double b)
        {
            CodeSource = CreateConstantTestCode("MulConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "MulConstantR8Left", (a * b), b));
        }
        #endregion
    }
}
