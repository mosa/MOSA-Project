/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
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
    /// Tests support for the IL cgt operation with various operands.
    /// </summary>
    [TestFixture]
    public class Cgt : CodeDomTestRunner
    {
        private static string s_testCode = @"
            static class Test {
                public static bool Cgt(t1 a, t2 b) {
                    return (a > b);
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
                        static bool CgtConstant(" + typeIn + @" x)
                        {
                            return (" + constLeft + @" > x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static bool CgtConstant(" + typeIn + @" x)
                        {
                            return (x > " + constRight + @");
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
        public void CgtC(char a, char b)
        {
            this.CodeSource = s_testCode.Replace("t1", "char").Replace("t2", "char");
            bool res = (bool)Run<B_C_C>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
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
        public void CgtConstantCRight(char a, char b)
        {
            this.CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
            bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
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
        public void CgtConstantCLeft(char a, char b)
        {
            this.CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
            bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion
        
        #region I1
        /// <summary>
        /// Tests support for the cgt IL operation for I1 operands.
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
        public void CgtI1(sbyte a, sbyte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "sbyte").Replace("t2", "sbyte");
            bool res = (bool)Run<B_I1_I1>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I1 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)42)]
        [TestCase(sbyte.MinValue, sbyte.MinValue)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void CgtConstantI1Right(sbyte a, sbyte b)
        {
            this.CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
            bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I1 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)42)]
        [TestCase(sbyte.MinValue, sbyte.MinValue)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void CgtConstantI1Left(sbyte a, sbyte b)
        {
            this.CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region I2
        /// <summary>
        /// Tests support for the cgt IL operation for I2 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)1, (short)1)]
        [TestCase((short)2, (short)1)]
        [TestCase(Int16.MinValue, (short)(Int16.MinValue + 10))]
        [TestCase(Int16.MaxValue, Int16.MaxValue)]
        [TestCase((short)0, Int16.MinValue)]
        [TestCase((short)0, Int16.MaxValue)]
        [TestCase((short)0, (short)1)]
        [TestCase(Int16.MinValue, (short)0)]
        [TestCase(Int16.MaxValue, (short)0)]
        [TestCase((short)1, (short)0)]
        [Test]
        public void CgtI2(short a, short b)
        {
            this.CodeSource = s_testCode.Replace("t1", "short").Replace("t2", "short");
            bool res = (bool)Run<B_I2_I2>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)42)]
        [TestCase(short.MinValue, short.MinValue)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void CgtConstantI2Right(short a, short b)
        {
            this.CodeSource = CreateConstantTestCode("short", null, b.ToString());
            bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)42)]
        [TestCase(short.MinValue, short.MinValue)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void CgtConstantI2Left(short a, short b)
        {
            this.CodeSource = CreateConstantTestCode("short", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region I4
        /// <summary>
        /// Tests support for the cgt IL operation for I4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(Int32.MinValue, Int32.MinValue + 10)]
        [TestCase(Int32.MaxValue, Int32.MaxValue)]
        [TestCase(0, Int32.MinValue)]
        [TestCase(0, Int32.MaxValue)]
        [TestCase(0, 1)]
        [TestCase(Int32.MinValue, 0)]
        [TestCase(Int32.MaxValue, 0)]
        [TestCase(1, 0)]
        [Test]
        public void CgtI4(int a, int b)
        {
            this.CodeSource = s_testCode.Replace("t1", "int").Replace("t2", "int");
            bool res = (bool)Run<B_I4_I4>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void CgtConstantI4Right(int a, int b)
        {
            this.CodeSource = CreateConstantTestCode("int", null, b.ToString());
            bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void CgtConstantI4Left(int a, int b)
        {
            this.CodeSource = CreateConstantTestCode("int", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region I8
        /// <summary>
        /// Tests support for the cgt IL operation for I8 operands.
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
        [Test]
        public void CgtI8(long a, long b)
        {
            this.CodeSource = s_testCode.Replace("t1", "long").Replace("t2", "long");
            bool res = (bool)Run<B_I8_I8>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(long.MinValue + 1, long.MinValue)]
        [TestCase(long.MinValue, long.MaxValue)]
        [TestCase(12377513, 1237751)]
        [TestCase(42, 17)]
        [TestCase(long.MaxValue, long.MinValue)]
        [Test]
        public void CgtConstantI8Right(long a, long b)
        {
            this.CodeSource = CreateConstantTestCode("long", null, b.ToString());
            bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for I8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0, 0)]
        [TestCase(-17, 42)]
        [TestCase(long.MinValue + 1, long.MinValue)]
        [TestCase(long.MinValue, long.MaxValue)]
        [TestCase(12377513, 1237751)]
        [TestCase(42, 17)]
        [TestCase(long.MaxValue, long.MinValue)]
        [Test]
        public void CgtConstantI8Left(long a, long b)
        {
            this.CodeSource = CreateConstantTestCode("long", a.ToString(), null);
            bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region U1
        /// <summary>
        /// Tests support for the cgt IL operation for U1 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)1, (byte)1)]
        [TestCase(Byte.MinValue, (byte)(Byte.MinValue + 10))]
        [TestCase(Byte.MaxValue, Byte.MaxValue)]
        [TestCase((byte)1, Byte.MinValue)]
        [TestCase((byte)0, Byte.MaxValue)]
        [TestCase((byte)0, (byte)1)]
        [TestCase(Byte.MinValue, (byte)1)]
        [TestCase(Byte.MaxValue, (byte)0)]
        [TestCase((byte)1, (byte)0)]
        [Test]
        public void CgtU1(byte a, byte b)
        {
            this.CodeSource = s_testCode.Replace("t1", "byte").Replace("t2", "byte");
            bool res = (bool)Run<B_U1_U1>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U1 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)17, (byte)142)]
        [TestCase(byte.MaxValue, byte.MaxValue)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void CgtConstantU1Right(byte a, byte b)
        {
            this.CodeSource = CreateConstantTestCode("byte", null, b.ToString());
            bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U1 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((byte)0, (byte)0)]
        [TestCase((byte)17, (byte)142)]
        [TestCase(byte.MaxValue, byte.MaxValue)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void CgtConstantU1Left(byte a, byte b)
        {
            this.CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region U2
        /// <summary>
        /// Tests support for the cgt IL operation for U2 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)1, (ushort)1)]
        [TestCase(UInt16.MinValue, (ushort)(UInt16.MinValue + 10))]
        [TestCase(UInt16.MaxValue, UInt16.MaxValue)]
        [TestCase((ushort)1, UInt16.MinValue)]
        [TestCase((ushort)0, UInt16.MaxValue)]
        [TestCase((ushort)0, (ushort)1)]
        [TestCase(UInt16.MinValue, (ushort)2)]
        [TestCase(UInt16.MaxValue, (ushort)0)]
        [TestCase((ushort)1, (ushort)0)]
        [Test]
        public void CgtU2(ushort a, ushort b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ushort").Replace("t2", "ushort");
            bool res = (bool)Run<B_U2_U2>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U2 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)17, (ushort)142)]
        [TestCase(ushort.MaxValue, ushort.MaxValue)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void CgtConstantU2Right(ushort a, ushort b)
        {
            this.CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
            bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U2 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ushort)0, (ushort)0)]
        [TestCase((ushort)17, (ushort)142)]
        [TestCase(ushort.MaxValue, ushort.MaxValue)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void CgtConstantU2Left(ushort a, ushort b)
        {
            this.CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region U4
        /// <summary>
        /// Tests support for the cgt IL operation for U4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)1, (uint)1)]
        [TestCase(UInt32.MinValue, (uint)(UInt32.MinValue + 10))]
        [TestCase(UInt32.MaxValue, UInt32.MaxValue)]
        [TestCase((uint)1, UInt32.MinValue)]
        [TestCase((uint)0, UInt32.MaxValue)]
        [TestCase((uint)3, (uint)1)]
        [TestCase(UInt32.MinValue, (uint)1)]
        [TestCase(UInt32.MaxValue, (uint)0)]
        [TestCase((uint)1, (uint)0)]
        [Test]
        public void CgtU4(uint a, uint b)
        {
            this.CodeSource = s_testCode.Replace("t1", "uint").Replace("t2", "uint");
            bool res = (bool)Run<B_U4_U4>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U4 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)17, (uint)142)]
        [TestCase(uint.MaxValue, uint.MaxValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void CgtConstantU4Right(uint a, uint b)
        {
            this.CodeSource = CreateConstantTestCode("uint", null, b.ToString());
            bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U4 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)17, (uint)142)]
        [TestCase(uint.MaxValue, uint.MaxValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void CgtConstantU4Left(uint a, uint b)
        {
            this.CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region U8
        /// <summary>
        /// Tests support for the cgt IL operation for U8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)1, (ulong)1)]
        [TestCase(UInt64.MinValue, UInt64.MinValue + 10)]
        [TestCase(UInt64.MaxValue, UInt64.MaxValue)]
        [TestCase((ulong)1, UInt64.MinValue)]
        [TestCase((ulong)0, UInt64.MaxValue)]
        [TestCase((ulong)3, (ulong)2)]
        [TestCase(UInt64.MinValue, (ulong)1)]
        [TestCase(UInt64.MaxValue, (ulong)0)]
        [TestCase((ulong)1, (ulong)0)]
        [Test]
        public void CgtU8(ulong a, ulong b)
        {
            this.CodeSource = s_testCode.Replace("t1", "ulong").Replace("t2", "ulong");
            bool res = (bool)Run<B_U8_U8>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)17, (ulong)142)]
        [TestCase(ulong.MaxValue, ulong.MaxValue)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void CgtConstantU8Right(ulong a, ulong b)
        {
            this.CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
            bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for U8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase((ulong)0, (ulong)0)]
        [TestCase((ulong)17, (ulong)142)]
        [TestCase(ulong.MaxValue, ulong.MaxValue)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void CgtConstantU8Left(ulong a, ulong b)
        {
            this.CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
            bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region R4
        /// <summary>
        /// Tests support for the cgt IL operation for R4 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(Single.MinValue, Single.MinValue + 10)]
        [TestCase(Single.MaxValue, Single.MaxValue)]
        [TestCase(0.0f, Single.MinValue)]
        [TestCase(0.0f, Single.MaxValue)]
        [TestCase(3.0f, 1.0f)]
        [TestCase(Single.MinValue, 0.0f)]
        [TestCase(Single.MaxValue, 0.0f)]
        [TestCase(1.0f, 0.0f)]
        [Test]
        public void CgtR4(float a, float b)
        {
            this.CodeSource = s_testCode.Replace("t1", "float").Replace("t2", "float");
            bool res = (bool)Run<B_R4_R4>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for R4 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 1.0f)]
        [TestCase(Single.MinValue, Single.MinValue + 10)]
        [TestCase(0.0f, Single.MinValue)]
        [TestCase(0.0f, Single.MaxValue)]
        [TestCase(3.0f, 1.0f)]
        [TestCase(Single.MinValue, 0.0f)]
        [TestCase(Single.MaxValue, 0.0f)]
        [TestCase(1.0f, 0.0f)]
        [Test]
        public void CgtConstantR4Right(float a, float b)
        {
            this.CodeSource = CreateConstantTestCode("float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture)+"f");
            bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for R4 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0f, 0f)]
        [TestCase(-17f, 42.5f)]
        [TestCase(float.MaxValue, float.MaxValue)]
        [TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void CgtConstantR4Left(float a, float b)
        {
            this.CodeSource = CreateConstantTestCode("float", a.ToString(System.Globalization.CultureInfo.InvariantCulture)+"f", null);
            bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion

        #region R8
        /// <summary>
        /// Tests support for the cgt IL operation for R8 operands.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.5)]
        [TestCase(1.0, 2.0)]
        [TestCase(Double.MinValue, Double.MinValue + 1)]
        [TestCase(Double.MaxValue - 1, Double.MaxValue)]
        [TestCase(0.0, Double.MinValue)]
        [TestCase(0.0, Double.MaxValue)]
        [TestCase(3.0, 1.0)]
        [TestCase(Double.MinValue, 0.0)]
        [TestCase(Double.MaxValue, 0.0)]
        [TestCase(1.0, 0.0)]
        [Test]
        public void CgtR8(double a, double b)
        {
            this.CodeSource = s_testCode.Replace("t1", "double").Replace("t2", "double");
            bool res = (bool)Run<B_R8_R8>(@"", @"Test", @"Cgt", a, b);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for R8 operands with right value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.0)]
        [TestCase(-17.0, 42.5)]
        [TestCase(1.79769313486231E+308, 1.79769313486231E+308)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void CgtConstantR8Right(double a, double b)
        {
            this.CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CgtConstant", a);
            Assert.IsTrue((a > b) == res);
        }
        
        /// <summary>
        /// Tests support for the cgt IL operation for R8 operands with left value constant.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        [TestCase(0.0, 0.0)]
        [TestCase(-17.0, 42.5)]
        [TestCase(1.79769313486231E+308, 1.79769313486231E+308)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void CgtConstantR8Left(double a, double b)
        {
            this.CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CgtConstant", b);
            Assert.IsTrue((a > b) == res);
        }
        #endregion
    }
}
