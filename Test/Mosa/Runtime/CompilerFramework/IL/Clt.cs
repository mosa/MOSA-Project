/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
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
    /// Tests support for the IL clt operation with various operands.
    /// </summary>
    [TestFixture]
    public class Clt : CodeDomTestRunner
    {
        private static string s_testCode = @"
            static class Test {
                public static bool Clt(t1 a, t2 b) {
                    return (a < b);
                }
            }
        ";
        
        private static string CreateConstantTestCode(string typeIn, string constLeft, string constRight)
        {
            if (String.IsNullOrEmpty(constRight))
            {
                return @"
                    static class Test
                    {
                        static bool CltConstant(" + typeIn + @" x)
                        {
                            return (" + constLeft + @" < x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static bool CltConstant(" + typeIn + @" x)
                        {
                            return (x < " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        delegate bool B_C_C([MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);
        delegate bool B_I1_I1(sbyte a, sbyte b);
        delegate bool B_I2_I2(short a, short b);
        delegate bool B_I4_I4(int a, int b);
        delegate bool B_I8_I8(long a, long b);
        delegate bool B_U1_U1(byte a, byte b);
        delegate bool B_U2_U2(ushort a, ushort b);
        delegate bool B_U4_U4(uint a, uint b);
        delegate bool B_U8_U8(ulong a, ulong b);
        delegate bool B_R4_R4(float a, float b);
        delegate bool B_R8_R8(double a, double b);
        
        delegate bool B_Constant_C([MarshalAs(UnmanagedType.U2)]char x);
        delegate bool B_Constant_I1(sbyte x);
        delegate bool B_Constant_I2(short x);
        delegate bool B_Constant_I4(int x);
        delegate bool B_Constant_I8(long x);
        delegate bool B_Constant_U1(byte x);
        delegate bool B_Constant_U2(ushort x);
        delegate bool B_Constant_U4(uint x);
        delegate bool B_Constant_U8(ulong x);
        delegate bool B_Constant_R4(float x);
        delegate bool B_Constant_R8(double x);

        #region C
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
        public void CltC(char a, char b)
        {
            this.CodeSource = s_testCode.Replace("t1", "char").Replace("t2", "char");
            bool res = (bool)Run<B_C_C>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void CltConstantCRight(char a, char b)
        {
            this.CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
            bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
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
        public void CltConstantCLeft(char a, char b)
        {
            this.CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
            bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion
        
        #region I1
        /// <summary>
        /// Tests support for the clt IL operation for I1 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)1, (sbyte)1)]
        [TestCase((sbyte)2, (sbyte)1)]
        [TestCase(SByte.MinValue, (sbyte)(SByte.MinValue + 10))]
        [TestCase(SByte.MaxValue, SByte.MaxValue)]
        [TestCase((sbyte)0, SByte.MinValue)]
        [TestCase((sbyte)0, SByte.MaxValue)]
        [TestCase((sbyte)0, (sbyte)1)]
        [TestCase(SByte.MinValue, (sbyte)0)]
        [TestCase(SByte.MaxValue, (sbyte)0)]
        [TestCase((sbyte)1, (sbyte)0)]
        [Test]
        public void CltI1(sbyte a, sbyte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "sbyte").Replace("t2", "sbyte");
            bool res = (bool)Run<B_I1_I1>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I1 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)42)]
        [TestCase(sbyte.MinValue, sbyte.MinValue)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void CltConstantI1Right(sbyte a, sbyte b)
        {
            this.CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
            bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I1 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)42)]
        [TestCase(sbyte.MinValue, sbyte.MinValue)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void CltConstantI1Left(sbyte a, sbyte b)
        {
            this.CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region I2
        /// <summary>
        /// Tests support for the clt IL operation for I2 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)1, (short)1)]
        [TestCase((short)2, (short)1)]
        [TestCase(short.MinValue, (short)(short.MinValue + 10))]
        [TestCase(short.MaxValue, short.MaxValue)]
        [TestCase((short)0, short.MinValue)]
        [TestCase((short)0, short.MaxValue)]
        [TestCase((short)0, (short)1)]
        [TestCase(short.MinValue, (short)0)]
        [TestCase(short.MaxValue, (short)0)]
        [TestCase((short)1, (short)0)]
        [Test]
        public void CltI2(short a, short b)
        {
            this.CodeSource = s_testCode.Replace("t1", "short").Replace("t2", "short");
            bool res = (bool)Run<B_I2_I2>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)42)]
        [TestCase(short.MinValue, short.MinValue)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void CltConstantI2Right(short a, short b)
        {
            this.CodeSource = CreateConstantTestCode("short", null, b.ToString());
            bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)42)]
        [TestCase(short.MinValue, short.MinValue)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void CltConstantI2Left(short a, short b)
        {
            this.CodeSource = CreateConstantTestCode("short", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region I4
        /// <summary>
        /// Tests support for the clt IL operation for I4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((int)0, (int)0)]
        [TestCase((int)1, (int)1)]
        [TestCase((int)2, (int)1)]
        [TestCase(int.MinValue, (int)(int.MinValue + 10))]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase((int)0, int.MinValue)]
        [TestCase((int)0, int.MaxValue)]
        [TestCase((int)0, (int)1)]
        [TestCase(int.MinValue, (int)0)]
        [TestCase(int.MaxValue, (int)0)]
        [TestCase((int)1, (int)0)]
        [Test]
        public void CltI4(int a, int b)
        {
            this.CodeSource = s_testCode.Replace("t1", "int").Replace("t2", "int");
            bool res = (bool)Run<B_I4_I4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void CltConstantI4Right(int a, int b)
        {
            this.CodeSource = CreateConstantTestCode("int", null, b.ToString());
            bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void CltConstantI4Left(int a, int b)
        {
            this.CodeSource = CreateConstantTestCode("int", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region I8
        /// <summary>
        /// Tests support for the clt IL operation for I8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0L, 0L)]
        [TestCase(1L, 1L)]
        [TestCase(Int64.MinValue, Int64.MinValue + 10)]
        [TestCase(Int64.MaxValue, Int64.MaxValue)]
        [TestCase(0L, Int64.MinValue)]
        [TestCase(0L, Int64.MaxValue)]
        [TestCase(0L, 1L)]
        [TestCase(Int64.MinValue, 0L)]
        [TestCase(Int64.MaxValue, 0L)]
        [TestCase(1L, 0L)]
        [TestCase(-1L, -2L)]
        [TestCase(-2L, -1L)]
        [Test]
        public void CltI8(long a, long b)
        {
            this.CodeSource = s_testCode.Replace("t1", "long").Replace("t2", "long");
            bool res = (bool)Run<B_I8_I8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(long.MinValue, long.MinValue)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void CltConstantI8Right(long a, long b)
        {
            this.CodeSource = CreateConstantTestCode("long", null, b.ToString());
            bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for I8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(long.MinValue, long.MinValue)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void CltConstantI8Left(long a, long b)
        {
            this.CodeSource = CreateConstantTestCode("long", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region U1
        /// <summary>
        /// Tests support for the clt IL operation for U1 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)1, (byte)1)]
        [TestCase((byte)2, (byte)1)]
        [TestCase(byte.MinValue, (byte)(byte.MinValue + 10))]
        [TestCase(byte.MaxValue, byte.MaxValue)]
        [TestCase((byte)0, byte.MinValue)]
        [TestCase((byte)0, byte.MaxValue)]
        [TestCase((byte)0, (byte)1)]
        [TestCase(byte.MinValue, (byte)0)]
        [TestCase(byte.MaxValue, (byte)0)]
        [TestCase((byte)1, (byte)0)]
        [Test]
        public void CltU1(byte a, byte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "byte").Replace("t2", "byte");
            bool res = (bool)Run<B_U1_U1>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U1 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)17, (byte)42)]
        [TestCase(byte.MinValue, byte.MinValue)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void CltConstantU1Right(byte a, byte b)
        {
            this.CodeSource = CreateConstantTestCode("byte", null, b.ToString());
            bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U1 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)17, (byte)42)]
        [TestCase(byte.MinValue, byte.MinValue)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void CltConstantU1Left(byte a, byte b)
        {
            this.CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region U2
        /// <summary>
        /// Tests support for the clt IL operation for U2 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)2, (ushort)1)]
        [TestCase(ushort.MinValue, (ushort)(ushort.MinValue + 10))]
        [TestCase(ushort.MaxValue, ushort.MaxValue)]
        [TestCase((ushort)0, ushort.MinValue)]
        [TestCase((ushort)0, ushort.MaxValue)]
        [TestCase((ushort)0, (ushort)1)]
        [TestCase(ushort.MinValue, (ushort)0)]
        [TestCase(ushort.MaxValue, (ushort)0)]
        [TestCase((ushort)1, (ushort)0)]
        [Test]
        public void CltU2(ushort a, ushort b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ushort").Replace("t2", "ushort");
            bool res = (bool)Run<B_U2_U2>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)17, (ushort)42)]
        [TestCase(ushort.MinValue, ushort.MinValue)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void CltConstantU2Right(ushort a, ushort b)
        {
            this.CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
            bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)17, (ushort)42)]
        [TestCase(ushort.MinValue, ushort.MinValue)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void CltConstantU2Left(ushort a, ushort b)
        {
            this.CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region U4
        /// <summary>
        /// Tests support for the clt IL operation for U4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)1, (uint)1)]
        [TestCase((uint)2, (uint)1)]
        [TestCase(uint.MinValue, (uint)(uint.MinValue + 10))]
        [TestCase(uint.MaxValue, uint.MaxValue)]
        [TestCase((uint)0, uint.MinValue)]
        [TestCase((uint)0, uint.MaxValue)]
        [TestCase((uint)0, (uint)1)]
        [TestCase(uint.MinValue, (uint)0)]
        [TestCase(uint.MaxValue, (uint)0)]
        [TestCase((uint)1, (uint)0)]
        [Test]
        public void CltU4(uint a, uint b)
        {
            this.CodeSource = s_testCode.Replace("t1", "uint").Replace("t2", "uint");
            bool res = (bool)Run<B_U4_U4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U4 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)17, (uint)42)]
        [TestCase(uint.MinValue, uint.MinValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void CltConstantU4Right(uint a, uint b)
        {
            this.CodeSource = CreateConstantTestCode("uint", null, b.ToString());
            bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U4 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)17, (uint)42)]
        [TestCase(uint.MinValue, uint.MinValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void CltConstantU4Left(uint a, uint b)
        {
            this.CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region U8
        /// <summary>
        /// Tests support for the clt IL operation for U8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)1, (ulong)1)]
        [TestCase((ulong)2, (ulong)1)]
        [TestCase(ulong.MinValue, (ulong)(ulong.MinValue + 10))]
        [TestCase(ulong.MaxValue, ulong.MaxValue)]
        [TestCase((ulong)0, ulong.MinValue)]
        [TestCase((ulong)0, ulong.MaxValue)]
        [TestCase((ulong)0, (ulong)1)]
        [TestCase(ulong.MinValue, (ulong)0)]
        [TestCase(ulong.MaxValue, (ulong)0)]
        [TestCase((ulong)1, (ulong)0)]
        [Test]
        public void CltU8(ulong a, ulong b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ulong").Replace("t2", "ulong");
            bool res = (bool)Run<B_U8_U8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)17, (ulong)42)]
        [TestCase(ulong.MinValue, ulong.MinValue)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void CltConstantU8Right(ulong a, ulong b)
        {
            this.CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
            bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for U8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)17, (ulong)42)]
        [TestCase(ulong.MinValue, ulong.MinValue)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void CltConstantU8Left(ulong a, ulong b)
        {
            this.CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region R4
        /// <summary>
        /// Tests support for the clt IL operation for R4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(Single.MinValue, Single.MinValue + 10.0f)]
        [TestCase(Single.MaxValue - 0.5f, Single.MaxValue)]
        [TestCase(0.0f, Single.MinValue)]
        [TestCase(0.0f, Single.MaxValue)]
        [TestCase(1.0f, 3.0f)]
        [TestCase(Single.MinValue, 0.0f)]
        [TestCase(Single.MaxValue, 0.0f)]
        [TestCase(0.0f, 1.0f)]
        [Test]
        public void CltR4(float a, float b)
        {
            this.CodeSource = s_testCode.Replace("t1", "float").Replace("t2", "float");
            bool res = (bool)Run<B_R4_R4>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for R4 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(Single.MaxValue - 10.5f, Single.MaxValue)]
        [TestCase(0.0f, Single.MinValue)]
        [TestCase(0.0f, Single.MaxValue)]
        [TestCase(1.0f, 3.0f)]
        [TestCase(Single.MinValue, 0.0f)]
        [TestCase(Single.MaxValue, 0.0f)]
        [TestCase(0.0f, 1.0f)]
        [Test]
        public void CltConstantR4Right(float a, float b)
        {
            this.CodeSource = CreateConstantTestCode("float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture)+"f");
            bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for R4 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(Single.MinValue, Single.MinValue + 10.0f)]
        [TestCase(0.0f, Single.MinValue)]
        [TestCase(0.0f, Single.MaxValue)]
        [TestCase(1.0f, 3.0f)]
        [TestCase(Single.MinValue, 0.0f)]
        [TestCase(Single.MaxValue, 0.0f)]
        [TestCase(0.0f, 1.0f)]
        [Test]
        public void CltConstantR4Left(float a, float b)
        {
            this.CodeSource = CreateConstantTestCode("float", a.ToString(System.Globalization.CultureInfo.InvariantCulture)+"f", null);
            bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion

        #region R8
        /// <summary>
        /// Tests support for the clt IL operation for R8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.5)]
        [TestCase(1.0, 2.0)]
        [TestCase(Double.MinValue, Double.MinValue + 1)]
        [TestCase(Double.MaxValue - 1, Double.MaxValue)]
        [TestCase(0.0, Double.MinValue)]
        [TestCase(0.0, Double.MaxValue)]
        [TestCase(0.0, 1.0)]
        [TestCase(Double.MinValue, 0.0)]
        [TestCase(Double.MaxValue, 0.0)]
        [TestCase(1.0, 0.0)]
        [Test]
        public void CltR8(double a, double b)
        {
            this.CodeSource = s_testCode.Replace("t1", "double").Replace("t2", "double");
            bool res = (bool)Run<B_R8_R8>(@"", @"Test", @"Clt", a, b);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for R8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.0)]
        [TestCase(-17.0, 42.5)]
        [TestCase(1.79769313486231E+308, 1.79769313486231E+308)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void CltConstantR8Right(double a, double b)
        {
            this.CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CltConstant", a);
            Assert.IsTrue((a < b) == res);
        }
        
        /// <summary>
        /// Tests support for the clt IL operation for R8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.0)]
        [TestCase(-17.0, 42.5)]
        [TestCase(1.79769313486231E+308, 1.79769313486231E+308)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void CltConstantR8Left(double a, double b)
        {
            this.CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CltConstant", b);
            Assert.IsTrue((a < b) == res);
        }
        #endregion
    }
}
