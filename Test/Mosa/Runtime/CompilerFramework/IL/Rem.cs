/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:kintaro@think-in-co.de>)
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
    public class Rem : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a % b);
                    }
                }";
        }

        private static string CreateTestCodeWithReturn(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return (a % b);
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
                            return expect == (" + constLeft + @" % x);
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
                            return expect == (x % " + constRight + @");
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
        [TestCase((char)0, (char)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((char)17, (char)128)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void RemC(char a, char b)
        {
            CodeSource = CreateTestCode("RemC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "RemC", (char)(a % b), a, b));
        }
        
        delegate bool C_Constant_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void RemConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("RemConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "RemConstantCRight", (char)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase('a', (char)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void RemConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("RemConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "RemConstantCLeft", (char)(a % b), b));
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
        delegate bool I4_I1_I1(sbyte expect, sbyte a, sbyte b);
        delegate int I4_I1_I1_Return(sbyte expect, sbyte a, sbyte b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)1, (sbyte)2)]
        [TestCase((sbyte)23, (sbyte)21)]
        [TestCase((sbyte)1, (sbyte)-2)]
        [TestCase((sbyte)-1, (sbyte)2)]
        [TestCase((sbyte)0, (sbyte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((sbyte)-17, (sbyte)-2)]
        // And reverse
        [TestCase((sbyte)2, (sbyte)1)]
        [TestCase((sbyte)21, (sbyte)23)]
        [TestCase((sbyte)-2, (sbyte)1)]
        [TestCase((sbyte)2, (sbyte)-1)]
        [TestCase((sbyte)-2, (sbyte)-17)]
        // (MinValue, X) Cases
        [TestCase(sbyte.MinValue, (sbyte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, (sbyte)1)]
        [TestCase(sbyte.MinValue, (sbyte)17)]
        [TestCase(sbyte.MinValue, (sbyte)123)]
        [TestCase(sbyte.MinValue, (sbyte)-0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, (sbyte)-1)]
        [TestCase(sbyte.MinValue, (sbyte)-17)]
        [TestCase(sbyte.MinValue, (sbyte)-123)]
        // (MaxValue, X) Cases
        [TestCase(sbyte.MaxValue, (sbyte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MaxValue, (sbyte)1)]
        [TestCase(sbyte.MaxValue, (sbyte)17)]
        [TestCase(sbyte.MaxValue, (sbyte)123)]
        [TestCase(sbyte.MaxValue, (sbyte)-0, ExpectedException = typeof(DivideByZeroException))]
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
        public void RemI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCodeWithReturn("RemI1", "sbyte", "int");
            Assert.AreEqual(a % b, Run<I4_I1_I1_Return>("", "Test", "RemI1", (sbyte)(a % b), a, b));
        }
        
        delegate bool I1_Constant_I1(sbyte expect, sbyte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)23, (sbyte)21)]
        [TestCase((sbyte)2, (sbyte)-17)]
        [TestCase((sbyte)0, (sbyte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void RemConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI1Right", "sbyte", "sbyte", null, b.ToString());
            Assert.IsTrue((bool)Run<I1_Constant_I1>("", "Test", "RemConstantI1Right", (sbyte)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)23, (sbyte)21)]
        [TestCase((sbyte)2, (sbyte)-17)]
        [TestCase((sbyte)0, (sbyte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void RemConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI1Left", "sbyte", "sbyte", a.ToString(), null);
            Assert.IsTrue((bool)Run<I1_Constant_I1>("", "Test", "RemConstantI1Left", (sbyte)(a % b), b));
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
        delegate bool U4_U1_U1(byte expect, byte a, byte b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)1, (byte)2)]
        [TestCase((byte)23, (byte)21)]
        [TestCase((byte)0, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase((byte)2, (byte)1)]
        [TestCase((byte)21, (byte)23)]
        // (MinValue, X) Cases
        [TestCase(byte.MinValue, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, (byte)1)]
        [TestCase(byte.MinValue, (byte)17)]
        [TestCase(byte.MinValue, (byte)123)]
        // (MaxValue, X) Cases
        [TestCase(byte.MaxValue, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MaxValue, (byte)1)]
        [TestCase(byte.MaxValue, (byte)17)]
        [TestCase(byte.MaxValue, (byte)123)]
        // (X, MinValue) Cases
        [TestCase((byte)0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((byte)1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((byte)17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((byte)123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase((byte)0, byte.MaxValue)]
        [TestCase((byte)1, byte.MaxValue)]
        [TestCase((byte)17, byte.MaxValue)]
        [TestCase((byte)123, byte.MaxValue)]
        // Extremvaluecases
        [TestCase(byte.MinValue, byte.MaxValue)]
        [TestCase(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((byte)1, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("RemU1", "byte", "byte");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "RemU1", (byte)(a % b), a, b));
        }
        
        delegate bool U1_Constant_U1(byte expect, byte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)21)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void RemConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU1Right", "byte", "byte", null, b.ToString());
            Assert.IsTrue((bool)Run<U1_Constant_U1>("", "Test", "RemConstantU1Right", (byte)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)21)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void RemConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU1Left", "byte", "byte", a.ToString(), null);
            Assert.IsTrue((bool)Run<U1_Constant_U1>("", "Test", "RemConstantU1Left", (byte)(a % b), b));
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
        delegate bool I4_I2_I2(short expect, short a, short b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)1, (short)2)]
        [TestCase((short)23, (short)21)]
        [TestCase((short)1, (short)-2)]
        [TestCase((short)-1, (short)2)]
        [TestCase((short)0, (short)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((short)-17, (short)-2)]
        // And reverse
        [TestCase((short)2, (short)1)]
        [TestCase((short)21, (short)23)]
        [TestCase((short)-2, (short)1)]
        [TestCase((short)2, (short)-1)]
        [TestCase((short)-2, (short)-17)]
        // (MinValue, X) Cases
        [TestCase(short.MinValue, (short)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, (short)1)]
        [TestCase(short.MinValue, (short)17)]
        [TestCase(short.MinValue, (short)123)]
        [TestCase(short.MinValue, (short)-0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, (short)-1)]
        [TestCase(short.MinValue, (short)-17)]
        [TestCase(short.MinValue, (short)-123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, (short)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MaxValue, (short)1)]
        [TestCase(short.MaxValue, (short)17)]
        [TestCase(short.MaxValue, (short)123)]
        [TestCase(short.MaxValue, (short)-0, ExpectedException = typeof(DivideByZeroException))]
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
        public void RemI2(short a, short b)
        {
            CodeSource = CreateTestCode("RemI2", "short", "short");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "RemI2", (short)(a % b), a, b));
        }
        
        delegate bool I2_Constant_I2(short expect, short x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)21)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void RemConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI2Right", "short", "short", null, b.ToString());
            Assert.IsTrue((bool)Run<I2_Constant_I2>("", "Test", "RemConstantI2Right", (short)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)21)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void RemConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI2Left", "short", "short", a.ToString(), null);
            Assert.IsTrue((bool)Run<I2_Constant_I2>("", "Test", "RemConstantI2Left", (short)(a % b), b));
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
        delegate bool U4_U2_U2(ushort expect, ushort a, ushort b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)1, (ushort)2)]
        [TestCase((ushort)23, (ushort)21)]
        [TestCase((ushort)0, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)21, (ushort)23)]
        // (MinValue, X) Cases
        [TestCase(ushort.MinValue, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, (ushort)1)]
        [TestCase(ushort.MinValue, (ushort)17)]
        [TestCase(ushort.MinValue, (ushort)123)]
        // (MaxValue, X) Cases
        [TestCase(ushort.MaxValue, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MaxValue, (ushort)1)]
        [TestCase(ushort.MaxValue, (ushort)17)]
        [TestCase(ushort.MaxValue, (ushort)123)]
        // (X, MinValue) Cases
        [TestCase((ushort)0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ushort)1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ushort)17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ushort)123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase((ushort)0, ushort.MaxValue)]
        [TestCase((ushort)1, ushort.MaxValue)]
        [TestCase((ushort)17, ushort.MaxValue)]
        [TestCase((ushort)123, ushort.MaxValue)]
        // Extremvaluecases
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [TestCase(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ushort)1, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemU2(ushort a, ushort b)
        {
            CodeSource = CreateTestCode("RemU2", "ushort", "ushort");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "RemU2", (ushort)(a % b), a, b));
        }
        
        delegate bool U2_Constant_U2(ushort expect, ushort x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)21)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void RemConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU2Right", "ushort", "ushort", null, b.ToString());
            Assert.IsTrue((bool)Run<U2_Constant_U2>("", "Test", "RemConstantU2Right", (ushort)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)21)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void RemConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU2Left", "ushort", "ushort", a.ToString(), null);
            Assert.IsTrue((bool)Run<U2_Constant_U2>("", "Test", "RemConstantU2Left", (ushort)(a % b), b));
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
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, 1)]
        [TestCase(int.MinValue, 17)]
        [TestCase(int.MinValue, 123)]
        [TestCase(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [TestCase(int.MinValue, -17)]
        [TestCase(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MaxValue, 17)]
        [TestCase(int.MaxValue, 123)]
        [TestCase(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemI4(int a, int b)
        {
            CodeSource = CreateTestCode("RemI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "RemI4", (int)(a % b), a, b));
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
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void RemConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "RemConstantI4Right", (a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void RemConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "RemConstantI4Left", (a % b), b));
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
        [TestCase((uint)0, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase((uint)2, (uint)1)]
        [TestCase((uint)21, (uint)23)]
        // (MinValue, X) Cases
        [TestCase(uint.MinValue, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MinValue, (uint)1)]
        [TestCase(uint.MinValue, (uint)17)]
        [TestCase(uint.MinValue, (uint)123)]
        // (MaxValue, X) Cases
        [TestCase(uint.MaxValue, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MaxValue, (uint)1)]
        [TestCase(uint.MaxValue, (uint)17)]
        [TestCase(uint.MaxValue, (uint)123)]
        // (X, MinValue) Cases
        [TestCase((uint)0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((uint)1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((uint)17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((uint)123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase((uint)0, uint.MaxValue)]
        [TestCase((uint)1, uint.MaxValue)]
        [TestCase((uint)17, uint.MaxValue)]
        [TestCase((uint)123, uint.MaxValue)]
        // Extremvaluecases
        [TestCase(uint.MinValue, uint.MaxValue)]
        [TestCase(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((uint)1, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("RemU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "RemU4", (uint)(a % b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)21)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void RemConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "RemConstantU4Right", (uint)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)21)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void RemConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "RemConstantU4Left", (uint)(a % b), b));
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
        delegate long I8_I8_I8_Return(long expect, long a, long b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, 1)]
        [TestCase(long.MinValue, 17)]
        [TestCase(long.MinValue, 123)]
        [TestCase(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [TestCase(long.MinValue, -17)]
        [TestCase(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MaxValue, 1)]
        [TestCase(long.MaxValue, 17)]
        [TestCase(long.MaxValue, 123)]
        [TestCase(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemI8(long a, long b)
        {
            CodeSource = CreateTestCodeWithReturn("RemI8", "long", "long");
            Assert.AreEqual((long)(a % b), (long)Run<I8_I8_I8_Return>("", "Test", "RemI8", (long)(a % b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void RemConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "RemConstantI8Right", (a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void RemConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("RemConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "RemConstantI8Left", (a % b), b));
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
        [TestCase((ulong)0, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase((ulong)2, (ulong)1)]
        [TestCase((ulong)21, (ulong)23)]
        // (MinValue, X) Cases
        [TestCase(ulong.MinValue, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MinValue, (ulong)1)]
        [TestCase(ulong.MinValue, (ulong)17)]
        [TestCase(ulong.MinValue, (ulong)123)]
        // (MaxValue, X) Cases
        [TestCase(ulong.MaxValue, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MaxValue, (ulong)1)]
        [TestCase(ulong.MaxValue, (ulong)17)]
        [TestCase(ulong.MaxValue, (ulong)123)]
        // (X, MinValue) Cases
        [TestCase((ulong)0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ulong)1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ulong)17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ulong)123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase((ulong)0, ulong.MaxValue)]
        [TestCase((ulong)1, ulong.MaxValue)]
        [TestCase((ulong)17, ulong.MaxValue)]
        [TestCase((ulong)123, ulong.MaxValue)]
        // Extremvaluecases
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [TestCase(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase((ulong)1, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void RemU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCode("RemU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "RemU8", (ulong)(a % b), a, b));
        }
        
        delegate bool U8_Constant_U8(ulong expect, ulong x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void RemConstantU8Right(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU8Right", "ulong", "ulong", null, b.ToString());
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "RemConstantU8Right", (ulong)(a % b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void RemConstantU8Left(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("RemConstantU8Left", "ulong", "ulong", a.ToString(), null);
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "RemConstantU8Left", (ulong)(a % b), b));
        }
        #endregion
    }
}
