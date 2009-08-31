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
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Sub : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a - b);
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
                            return expect == ((" + constLeft + @") - x);
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
                            return expect == (x - (" + constRight + @"));
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
        [TestCase(128, 17)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void SubC(char a, char b)
        {
            CodeSource = CreateTestCode("SubC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "SubC", (char)(a - b), a, b));
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
        public void SubConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("SubConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "SubConstantCRight", (char)(a - b), a));
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
        public void SubConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("SubConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "SubConstantCLeft", (char)(a - b), b));
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
        [Test]
        public void SubI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("SubI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "SubI1", a - b, a, b));
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
        [TestCase(0, 10)]
        [TestCase(0, -10)]
        [TestCase(10, 0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void SubConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "SubConstantI1Right", (a - b), a));
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
        public void SubConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "SubConstantI1Left", (a - b), b));
        }
        #endregion I1
        
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
        [Test]
        public void SubI2(short a, short b)
        {
            CodeSource = CreateTestCode("SubI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "SubI2", (a - b), a, b));
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
        public void SubConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "SubConstantI2Right", (a - b), a));
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
        public void SubConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "SubConstantI2Left", (a - b), b));
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
        [Test]
        public void SubU2(ushort a, ushort b)
        {
            CodeSource = CreateTestCode("SubU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "SubU2", (uint)(a - b), a, b));
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
        public void SubConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("SubConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "SubConstantU2Right", (uint)(a - b), a));
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
        public void SubConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("SubConstantU2Left", "ushort", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "SubConstantU2Left", (uint)(a - b), b));
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
        [Test]
        public void SubI4(int a, int b)
        {
            CodeSource = CreateTestCode("SubI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "SubI4", (a - b), a, b));
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
        public void SubConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "SubConstantI4Right", (a - b), a));
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
        public void SubConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "SubConstantI4Left", (a - b), b));
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
        [Test]
        public void SubI8(long a, long b)
        {
            CodeSource = CreateTestCode("SubI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "SubI8", (a - b), a, b));
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
        public void SubConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "SubConstantI8Right", (a - b), a));
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
        public void SubConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("SubConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "SubConstantI8Left", (a - b), b));
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
        [TestCase(1.2f, 2.1f)]
        [TestCase(23.0f, 21.2578f)]
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
        [TestCase(1.2, float.MinValue)]
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
        [TestCase(1.0f, float.NaN)]
        [TestCase(float.NaN, 1.0f)]
        [TestCase(1.0f, float.PositiveInfinity)]
        [TestCase(float.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, float.NegativeInfinity)]
        [TestCase(float.NegativeInfinity, 1.0f)]
        [Test]
        public void SubR4(float a, float b)
        {
            CodeSource = CreateTestCode("SubR4", "float", "float");
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "SubR4", (a - b), a, b));
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
        [Test]
        public void SubConstantR4Right(float a, float b)
        {
            CodeSource = CreateConstantTestCode("SubConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "SubConstantR4Right", (a - b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        // Obsolete, because of higher precision
        // [TestCase(-17.0002501f, float.MaxValue)]
        [Test]
        public void SubConstantR4Left(float a, float b)
        {
            CodeSource = CreateConstantTestCode("SubConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "SubConstantR4Left", (a - b), b));
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
        public void SubR8(double a, double b)
        {
            CodeSource = CreateTestCode("SubR8", "double", "double");
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "SubR8", (a - b), a, b));
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
        public void SubConstantR8Right(double a, double b)
        {
            CodeSource = CreateConstantTestCode("SubConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "SubConstantR8Right", (a - b), a));
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
        public void SubConstantR8Left(double a, double b)
        {
            CodeSource = CreateConstantTestCode("SubConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "SubConstantR8Left", (a - b), b));
        }
        #endregion
        
    }
}
