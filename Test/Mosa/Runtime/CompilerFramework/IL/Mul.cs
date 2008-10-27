/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
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
    public class Mul : CodeDomTestRunner
    {
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(sbyte.MinValue, 0)]
        [Row(sbyte.MinValue, 1)]
        [Row(sbyte.MinValue, 17)]
        [Row(sbyte.MinValue, 123)]
        [Row(sbyte.MinValue, -0)]
        [Row(sbyte.MinValue, -1)]
        [Row(sbyte.MinValue, -17)]
        [Row(sbyte.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(sbyte.MaxValue, 0)]
        [Row(sbyte.MaxValue, 1)]
        [Row(sbyte.MaxValue, 17)]
        [Row(sbyte.MaxValue, 123)]
        [Row(sbyte.MaxValue, -0)]
        [Row(sbyte.MaxValue, -1)]
        [Row(sbyte.MaxValue, -17)]
        [Row(sbyte.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, sbyte.MinValue)]
        [Row(1, sbyte.MinValue)]
        [Row(17, sbyte.MinValue)]
        [Row(123, sbyte.MinValue)]
        [Row(-0, sbyte.MinValue)]
        [Row(-1, sbyte.MinValue)]
        [Row(-17, sbyte.MinValue)]
        [Row(-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, sbyte.MaxValue)]
        [Row(1, sbyte.MaxValue)]
        [Row(17, sbyte.MaxValue)]
        [Row(123, sbyte.MaxValue)]
        [Row(-0, sbyte.MaxValue)]
        [Row(-1, sbyte.MaxValue)]
        [Row(-17, sbyte.MaxValue)]
        [Row(-123, sbyte.MaxValue)]
        // Extremvaluecases
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Row(sbyte.MaxValue, sbyte.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool MulI1(int expect, sbyte a, sbyte b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "MulI1", a * b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x); 
        delegate bool I4_Constant(int expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(2, -17)]
        [Row(0, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI1Right(sbyte a, sbyte b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantI1Right(int expect, sbyte a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "MulConstantI1Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(2, -17)]
        [Row(0, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI1Left(sbyte a, sbyte b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantI1Left(int expect, sbyte b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "MulConstantI1Left", (a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(2, -17)]
        [Row(0, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI1Both(sbyte a, sbyte b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantI1Both(int expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "MulConstantI1Both", (a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(byte.MinValue, 0)]
        [Row(byte.MinValue, 1)]
        [Row(byte.MinValue, 17)]
        [Row(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(byte.MaxValue, 0)]
        [Row(byte.MaxValue, 1)]
        [Row(byte.MaxValue, 17)]
        [Row(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, byte.MinValue)]
        [Row(1, byte.MinValue)]
        [Row(17, byte.MinValue)]
        [Row(123, byte.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, byte.MaxValue)]
        [Row(1, byte.MaxValue)]
        [Row(17, byte.MaxValue)]
        [Row(123, byte.MaxValue)]
        // Extremvaluecases
        [Row(byte.MinValue, byte.MaxValue)]
        [Row(byte.MaxValue, byte.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulU1(byte a, byte b)
        {
            CodeSource = "static class Test { static bool MulU1(uint expect, byte a, byte b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "MulU1", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x); 
        delegate bool U4_Constant(uint expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(byte.MinValue, byte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU1Right(byte a, byte b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantU1Right(uint expect, byte a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "MulConstantU1Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(byte.MinValue, byte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU1Left(byte a, byte b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantU1Left(uint expect, byte b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "MulConstantU1Left", (uint)(a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(byte.MinValue, byte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU1Both(byte a, byte b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantU1Both(uint expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant>("", "Test", "MulConstantU1Both", (uint)(a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(short.MinValue, 0)]
        [Row(short.MinValue, 1)]
        [Row(short.MinValue, 17)]
        [Row(short.MinValue, 123)]
        [Row(short.MinValue, -0)]
        [Row(short.MinValue, -1)]
        [Row(short.MinValue, -17)]
        [Row(short.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(short.MaxValue, 0)]
        [Row(short.MaxValue, 1)]
        [Row(short.MaxValue, 17)]
        [Row(short.MaxValue, 123)]
        [Row(short.MaxValue, -0)]
        [Row(short.MaxValue, -1)]
        [Row(short.MaxValue, -17)]
        [Row(short.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, short.MinValue)]
        [Row(1, short.MinValue)]
        [Row(17, short.MinValue)]
        [Row(123, short.MinValue)]
        [Row(-0, short.MinValue)]
        [Row(-1, short.MinValue)]
        [Row(-17, short.MinValue)]
        [Row(-123, short.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, short.MaxValue)]
        [Row(1, short.MaxValue)]
        [Row(17, short.MaxValue)]
        [Row(123, short.MaxValue)]
        [Row(-0, short.MaxValue)]
        [Row(-1, short.MaxValue)]
        [Row(-17, short.MaxValue)]
        [Row(-123, short.MaxValue)]
        // Extremvaluecases
        [Row(short.MinValue, short.MaxValue)]
        [Row(short.MaxValue, short.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI2(short a, short b)
        {
            CodeSource = "static class Test { static bool MulI2(int expect, short a, short b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "MulI2", (a * b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI2Right(short a, short b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantI2Right(int expect, short a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "MulConstantI2Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI2Left(short a, short b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantI2Left(int expect, short b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "MulConstantI2Left", (a * b), b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI2Both(short a, short b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantI2Both(int expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "MulConstantI2Both", (a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(ushort.MinValue, 0)]
        [Row(ushort.MinValue, 1)]
        [Row(ushort.MinValue, 17)]
        [Row(ushort.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ushort.MaxValue, 0)]
        [Row(ushort.MaxValue, 1)]
        [Row(ushort.MaxValue, 17)]
        [Row(ushort.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ushort.MinValue)]
        [Row(1, ushort.MinValue)]
        [Row(17, ushort.MinValue)]
        [Row(123, ushort.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, ushort.MaxValue)]
        [Row(1, ushort.MaxValue)]
        [Row(17, ushort.MaxValue)]
        [Row(123, ushort.MaxValue)]
        // Extremvaluecases
        [Row(ushort.MinValue, ushort.MaxValue)]
        [Row(ushort.MaxValue, ushort.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulU2(ushort a, ushort b)
        {
            CodeSource = "static class Test { static bool MulU2(uint expect, ushort a, ushort b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "MulU2", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(ushort.MinValue, ushort.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU2Right(ushort a, ushort b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantU2Right(uint expect, ushort a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "MulConstantU2Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(ushort.MinValue, ushort.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU2Left(ushort a, ushort b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantU2Left(uint expect, ushort b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "MulConstantU2Left", (uint)(a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(ushort.MinValue, ushort.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU2Both(ushort a, ushort b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantU2Both(uint expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant>("", "Test", "MulConstantU2Both", (uint)(a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(int.MinValue, 0)]
        [Row(int.MinValue, 1)]
        [Row(int.MinValue, 17)]
        [Row(int.MinValue, 123)]
        [Row(int.MinValue, -0)]
        [Row(int.MinValue, -1)]
        [Row(int.MinValue, -17)]
        [Row(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue, 0)]
        [Row(int.MaxValue, 1)]
        [Row(int.MaxValue, 17)]
        [Row(int.MaxValue, 123)]
        [Row(int.MaxValue, -0)]
        [Row(int.MaxValue, -1)]
        [Row(int.MaxValue, -17)]
        [Row(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, int.MinValue)]
        [Row(1, int.MinValue)]
        [Row(17, int.MinValue)]
        [Row(123, int.MinValue)]
        [Row(-0, int.MinValue)]
        [Row(-1, int.MinValue)]
        [Row(-17, int.MinValue)]
        [Row(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, int.MaxValue)]
        [Row(1, int.MaxValue)]
        [Row(17, int.MaxValue)]
        [Row(123, int.MaxValue)]
        [Row(-0, int.MaxValue)]
        [Row(-1, int.MaxValue)]
        [Row(-17, int.MaxValue)]
        [Row(-123, int.MaxValue)]
        // Extremvaluecases
        [Row(int.MinValue, int.MaxValue)]
        [Row(int.MaxValue, int.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI4(int a, int b)
        {
            CodeSource = "static class Test { static bool MulI4(int expect, int a, int b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "MulI4", (a * b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        //[Row(-23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI4Right(int a, int b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantI4Right(int expect, int a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "MulConstantI4Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        //[Row(-23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI4Left(int a, int b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantI4Left(int expect, int b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "MulConstantI4Left", (a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 21)]
        //[Row(-23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(0, int.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI4Both(int a, int b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantI4Both(int expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "MulConstantI4Both", (a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(uint.MinValue, 0)]
        [Row(uint.MinValue, 1)]
        [Row(uint.MinValue, 17)]
        [Row(uint.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(uint.MaxValue, 0)]
        [Row(uint.MaxValue, 1)]
        [Row(uint.MaxValue, 17)]
        [Row(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, uint.MinValue)]
        [Row(1, uint.MinValue)]
        [Row(17, uint.MinValue)]
        [Row(123, uint.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, uint.MaxValue)]
        [Row(1, uint.MaxValue)]
        [Row(17, uint.MaxValue)]
        [Row(123, uint.MaxValue)]
        // Extremvaluecases
        [Row(uint.MinValue, uint.MaxValue)]
        [Row(uint.MaxValue, uint.MinValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulU4(uint a, uint b)
        {
            CodeSource = "static class Test { static bool MulU4(uint expect, uint a, uint b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "MulU4", (uint)(a * b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(uint.MinValue, uint.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU4Right(uint a, uint b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantU4Right(uint expect, uint a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "MulConstantU4Right", (uint)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(uint.MinValue, uint.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU4Left(uint a, uint b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantU4Left(uint expect, uint b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "MulConstantU4Left", (uint)(a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 21)]
        //[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(uint.MinValue, uint.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU4Both(uint a, uint b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantU4Both(uint expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant>("", "Test", "MulConstantU4Both", (uint)(a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(1, -2)]
        [Row(-1, 2)]
        [Row(0, 0)]
        [Row(-17, -2)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        [Row(-2, 1)]
        [Row(2, -1)]
        [Row(-2, -17)]
        // (MinValue, X) Cases
        [Row(long.MinValue, 0)]
        [Row(long.MinValue, 1)]
        [Row(long.MinValue, 17)]
        [Row(long.MinValue, 123)]
        [Row(long.MinValue, -0)]
        [Row(long.MinValue, -1)]
        [Row(long.MinValue, -17)]
        [Row(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue, 0)]
        [Row(long.MaxValue, 1)]
        [Row(long.MaxValue, 17)]
        [Row(long.MaxValue, 123)]
        [Row(long.MaxValue, -0)]
        [Row(long.MaxValue, -1)]
        [Row(long.MaxValue, -17)]
        [Row(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [Row(0, long.MinValue)]
        [Row(1, long.MinValue)]
        [Row(17, long.MinValue)]
        [Row(123, long.MinValue)]
        [Row(-0, long.MinValue)]
        [Row(-1, long.MinValue)]
        [Row(-17, long.MinValue)]
        [Row(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, long.MaxValue)]
        [Row(1, long.MaxValue)]
        [Row(17, long.MaxValue)]
        [Row(123, long.MaxValue)]
        [Row(-0, long.MaxValue)]
        [Row(-1, long.MaxValue)]
        [Row(-17, long.MaxValue)]
        [Row(-123, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI8(long a, long b)
        {
            CodeSource = "static class Test { static bool MulI8(long expect, long a, long b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "MulI8", (a * b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);
        delegate bool I8_Constant(long expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(-123, long.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI8Right(long a, long b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantI8Right(long expect, long a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "MulConstantI8Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(-123, long.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI8Left(long a, long b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantI8Left(long expect, long b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "MulConstantI8Left", (a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(-1, long.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantI8Both(long a, long b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantI8Both(long expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I8_Constant>("", "Test", "MulConstantI8Both", (a * b)));
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
        [Row(1, 2)]
        [Row(23, 21)]
        [Row(0, 0)]
        // And reverse
        [Row(2, 1)]
        [Row(21, 23)]
        // (MinValue, X) Cases
        [Row(ulong.MinValue, 0)]
        [Row(ulong.MinValue, 1)]
        [Row(ulong.MinValue, 17)]
        [Row(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [Row(ulong.MaxValue, 0)]
        [Row(ulong.MaxValue, 1)]
        [Row(ulong.MaxValue, 17)]
        [Row(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [Row(0, ulong.MinValue)]
        [Row(1, ulong.MinValue)]
        [Row(17, ulong.MinValue)]
        [Row(123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [Row(0, ulong.MaxValue)]
        [Row(1, ulong.MaxValue)]
        [Row(17, ulong.MaxValue)]
        [Row(123, ulong.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulU8(ulong a, ulong b)
        {
            CodeSource = "static class Test { static bool MulU8(ulong expect, ulong a, ulong b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "MulU8", (ulong)(a * b), a, b));
        }
        
        delegate bool U8_Constant_U8(ulong expect, ulong x);
        delegate bool U8_Constant(ulong expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(1, ulong.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU8Right(ulong a, ulong b)
        {
            // right side constant
            CodeSource = "static class Test { static bool MulConstantU8Right(ulong expect, ulong a) { return expect == (a * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "MulConstantU8Right", (ulong)(a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(1, ulong.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU8Left(ulong a, ulong b)
        {
            // left side constant
            CodeSource = "static class Test { static bool MulConstantU8Left(ulong expect, ulong b) { return expect == (" + a.ToString() + " * b); } }";
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "MulConstantU8Left", (ulong)(a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(1, ulong.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantU8Both(ulong a, ulong b)
        {
            // both constant
            CodeSource = "static class Test { static bool MulConstantU8Both(ulong expect) { return expect == (" + a.ToString() + " * " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U8_Constant>("", "Test", "MulConstantU8Both", (ulong)(a * b)));
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
        [Row(1.0f, 2.0f)]
        [Row(2.0f, 0.0f)]
        [Row(1.0f, float.NaN)]
        [Row(float.NaN, 1.0f)]
        [Row(1.0f, float.PositiveInfinity)]
        [Row(float.PositiveInfinity, 1.0f)]
        [Row(1.0f, float.NegativeInfinity)]
        [Row(float.NegativeInfinity, 1.0f)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulR4(float a, float b)
        {
            CodeSource = "static class Test { static bool MulR4(float expect, float a, float b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "MulR4", (a * b), a, b));
        }
        
        delegate bool R4_Constant_R4(float expect, float x);
        delegate bool R4_Constant(float expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23f, 148.0016f)]
        [Row(17.2f, 1f)]
        [Row(0f, 0f)]
        [Row(float.MinValue, float.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR4Right(float a, float b)
        {
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            
            // right side constant
            CodeSource = "static class Test { static bool MulConstantR4Right(float expect, float a) { return expect == (a * " + y + "); } }";
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "MulConstantR4Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23f, 148.0016f)]
        [Row(17.2f, 1f)]
        [Row(0f, 0f)]
        [Row(float.MinValue, float.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR4Left(float a, float b)
        {
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            
            // left side constant
            CodeSource = "static class Test { static bool MulConstantR4Left(float expect, float b) { return expect == (" + x + " * b); } }";
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "MulConstantR4Left", (a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23f, 148.0016f)]
        [Row(17.2f, 1f)]
        [Row(0f, 0f)]
        [Row(float.MinValue, float.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR4Both(float a, float b)
        {
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";

            // both constant
            CodeSource = "static class Test { static bool MulConstantR4Both(float expect) { return expect == (" + x + " * " + y + "); } }";
            Assert.IsTrue((bool)Run<R4_Constant>("", "Test", "MulConstantR4Both", (a * b)));
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
        [Row(1.0, 2.0)]
        [Row(2.0, 0.0)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulR8(double a, double b)
        {
            CodeSource = "static class Test { static bool MulR8(double expect, double a, double b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "MulR8", (a * b), a, b));
        }
        
        delegate bool R8_Constant_R8(double expect, double x);
        delegate bool R8_Constant(double expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148.0016)]
        [Row(17.2, 1.0)]
        [Row(0.0, 0.0)]
        [Row(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR8Right(double a, double b)
        {                
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture);
            
            // right side constant
            CodeSource = "static class Test { static bool MulConstantR8Right(double expect, double a) { return expect == (a * " + y + "); } }";
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "MulConstantR8Right", (a * b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148.0016)]
        [Row(17.2, 1.0)]
        [Row(0.0, 0.0)]
        [Row(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR8Left(double a, double b)
        {                
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture);

            // left side constant
            CodeSource = "static class Test { static bool MulConstantR8Left(double expect, double b) { return expect == (" + x + " * b); } }";
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "MulConstantR8Left", (a * b), b));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148.0016)]
        [Row(17.2, 1.0)]
        [Row(0.0, 0.0)]
        [Row(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void MulConstantR8Both(double a, double b)
        {                
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture);

            // both constant
            CodeSource = "static class Test { static bool MulConstantR8Both(double expect) { return expect == (" + x + " * " + y + "); } }";
            Assert.IsTrue((bool)Run<R8_Constant>("", "Test", "MulConstantR8Both", (a * b)));
        }
        #endregion
    }
}
