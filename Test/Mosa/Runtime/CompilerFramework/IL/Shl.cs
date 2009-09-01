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
    public class Shl : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return CreateTestCode(name, typeIn, typeIn, typeOut);
        }
        
        private static string CreateTestCode(string name, string typeInA, string typeInB, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
                    {
                        return expect == (a << b);
                    }
                }";
        }

        private static string CreateTestCodeWithReturn(string name, string typeInA, string typeInB, string typeOut)
        {
            return @"
                static class Test
                {
                    static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
                    {
                        return (a << b);
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
                            return expect == (" + constLeft + @" << x);
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
                            return expect == (x << " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static string CreateConstantTestCodeWithReturn(string name, string typeIn, string typeOut, string constLeft, string constRight)
        {
            if (String.IsNullOrEmpty(constRight))
            {
                return @"
                    static class Test
                    {
                        static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return (" + constLeft + @" << x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return (x << " + constRight + @");
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
        delegate bool C_C_C(int expect, [MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, (char)0)]
        [TestCase((char)17, (char)128)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void ShlC(char a, char b)
        {
            CodeSource = CreateTestCode("AddC", "char", "int");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "AddC", (a << b), a, b));
        }
        
        delegate bool C_Constant_C(int expect, char x);
        delegate int C_Constant_C_Return([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void ShlConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCodeWithReturn("ShlConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
            Assert.AreEqual(a << b, (int)Run<C_Constant_C_Return>("", "Test", "ShlConstantCRight", (char)(a << b), a));
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
        public void ShlConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCodeWithReturn("ShlConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
            Assert.AreEqual(a << b, (int)Run<C_Constant_C_Return>("", "Test", "ShlConstantCLeft", (char)(a << b), b));
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
        // And reverse
        [TestCase((sbyte)2, (sbyte)1)]
        [TestCase((sbyte)21, (sbyte)23)]
        // (MinValue, X) Cases
        [TestCase(sbyte.MinValue, (sbyte)0)]
        [TestCase(sbyte.MinValue, (sbyte)1)]
        [TestCase(sbyte.MinValue, (sbyte)17)]
        [TestCase(sbyte.MinValue, (sbyte)123)]
        // (MaxValue, X) Cases
        [TestCase(sbyte.MaxValue, (sbyte)0)]
        [TestCase(sbyte.MaxValue, (sbyte)1)]
        [TestCase(sbyte.MaxValue, (sbyte)17)]
        [TestCase(sbyte.MaxValue, (sbyte)123)]
        // (X, MinValue) Cases
        [TestCase((sbyte)0, sbyte.MinValue)]
        [TestCase((sbyte)1, sbyte.MinValue)]
        [TestCase((sbyte)17, sbyte.MinValue)]
        [TestCase((sbyte)123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((sbyte)0, sbyte.MaxValue)]
        [TestCase((sbyte)1, sbyte.MaxValue)]
        [TestCase((sbyte)17, sbyte.MaxValue)]
        [TestCase((sbyte)123, sbyte.MaxValue)]
        // Extremvaluecases
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [TestCase(sbyte.MaxValue, sbyte.MinValue)]
        [Test]
        public void ShlI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("ShlI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "ShlI1", a << b, a, b));
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
        public void ShlConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "ShlConstantI1Right", (a << b), a));
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
        public void ShlConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "ShlConstantI1Left", (a << b), b));
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
        [TestCase((short)1, (short)2)]
        [TestCase((short)23, (short)21)]
        // And reverse
        [TestCase((short)2, (short)1)]
        [TestCase((short)21, (short)23)]
        // (MinValue, X) Cases
        [TestCase(short.MinValue, (short)0)]
        [TestCase(short.MinValue, (short)1)]
        [TestCase(short.MinValue, (short)17)]
        [TestCase(short.MinValue, (short)123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, (short)0)]
        [TestCase(short.MaxValue, (short)1)]
        [TestCase(short.MaxValue, (short)17)]
        [TestCase(short.MaxValue, (short)123)]
        // (X, MinValue) Cases
        [TestCase((short)0, short.MinValue)]
        [TestCase((short)1, short.MinValue)]
        [TestCase((short)17, short.MinValue)]
        [TestCase((short)123, short.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((short)0, short.MaxValue)]
        [TestCase((short)1, short.MaxValue)]
        [TestCase((short)17, short.MaxValue)]
        [TestCase((short)123, short.MaxValue)]
        // Extremvaluecases
        [TestCase(short.MinValue, short.MaxValue)]
        [TestCase(short.MaxValue, short.MinValue)]
        [Test]
        public void ShlI2(short a, short b)
        {
            CodeSource = CreateTestCode("ShlI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "ShlI2", (a << b), a, b));
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
        public void ShlConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "ShlConstantI2Right", (a << b), a));
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
        public void ShlConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "ShlConstantI2Left", (a << b), b));
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
        [TestCase((int)1, (int)2)]
        [TestCase((int)23, (int)21)]
        // And reverse
        [TestCase((int)2, (int)1)]
        [TestCase((int)21, (int)23)]
        // (MinValue, X) Cases
        [TestCase(int.MinValue, (int)0)]
        [TestCase(int.MinValue, (int)1)]
        [TestCase(int.MinValue, (int)17)]
        [TestCase(int.MinValue, (int)123)]
        // (MaxValue, X) Cases
        [TestCase(int.MaxValue, (int)0)]
        [TestCase(int.MaxValue, (int)1)]
        [TestCase(int.MaxValue, (int)17)]
        [TestCase(int.MaxValue, (int)123)]
        // (X, MinValue) Cases
        [TestCase((int)0, int.MinValue)]
        [TestCase((int)1, int.MinValue)]
        [TestCase((int)17, int.MinValue)]
        [TestCase((int)123, int.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((int)0, int.MaxValue)]
        [TestCase((int)1, int.MaxValue)]
        [TestCase((int)17, int.MaxValue)]
        [TestCase((int)123, int.MaxValue)]
        // Extremvaluecases
        [TestCase(int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, int.MinValue)]
        [Test]
        public void ShlI4(int a, int b)
        {
            CodeSource = CreateTestCode("ShlI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "ShlI4", (a << b), a, b));
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
        public void ShlConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "ShlConstantI4Right", (a << b), a));
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
        public void ShlConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "ShlConstantI4Left", (a << b), b));
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
        delegate bool I8_I8_I4(long expect, long a, int b);
        delegate long I8_I8_I4_R(long expect, long a, int b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(unchecked((long)0x8000000000000000), 64)]
        [Test]
        public void ShlI8(long a, int b)
        {
            CodeSource = CreateTestCodeWithReturn("ShlI8", "long", "int", "long");
            Assert.AreEqual((a << b), Run<I8_I8_I4_R>("", "Test", "ShlI8", (a << b), a, b));
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
        [TestCase(0, 1)]
        [Test]
        public void ShlConstantI8Right(long a, int b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "ShlConstantI8Right", (a << b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [Test]
        public void ShlConstantI8Left(long a, int b)
        {
            CodeSource = CreateConstantTestCode("ShlConstantI8Left", "int", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "ShlConstantI8Left", (a << b), b));
        }
        #endregion
    }
}
