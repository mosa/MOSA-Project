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
using Gallio.Framework;
using MbUnit.Framework;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Sub : CodeDomTestRunner
    {
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
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool SubI1(int expect, sbyte a, sbyte b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "SubI1", a - b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x); 
        delegate bool I4_Constant(int expect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-42, 48)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantI1(sbyte a, sbyte b)
        {
            // right side constant
            CodeSource = "static class Test { static bool SubConstantI1Right(int expect, sbyte a) { return expect == (a - (" + b.ToString() + ")); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "SubConstantI1Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantI1Left(int expect, sbyte b) { return expect == ((" + a.ToString() + ") - b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "SubConstantI1Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantI1Both(int expect) { return expect == ((" + a.ToString() + ") - (" + b.ToString() + ")); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "SubConstantI1Both", (a - b)));
        }
        

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
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubI2(short a, short b)
        {
            CodeSource = "static class Test { static bool SubI2(int expect, short a, short b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "SubI2", (a - b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantI2(short a, short b)
        {
            // right side constant
            CodeSource = "static class Test { static bool SubConstantI2Right(int expect, short a) { return expect == (a - (" + b.ToString() + ")); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "SubConstantI2Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantI2Left(int expect, short b) { return expect == ((" + a.ToString() + ") - b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "SubConstantI2Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantI2Both(int expect) { return expect == ((" + a.ToString() + ") - (" + b.ToString() + ")); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "SubConstantI2Both", (a - b)));
        }

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
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubU2(ushort a, ushort b)
        {
            CodeSource = "static class Test { static bool SubU2(uint expect, ushort a, ushort b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "SubU2", (uint)(a - b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x);
        delegate bool U4_Constant(uint expect); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(ushort.MinValue, ushort.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantU2(ushort a, ushort b)
        {
            // right side constant
            CodeSource = "static class Test { static bool SubConstantU2Right(uint expect, ushort a) { return expect == (a - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "SubConstantU2Right", (uint)(a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantU2Left(uint expect, ushort b) { return expect == (" + a.ToString() + " - b); } }";
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "SubConstantU2Left", (uint)(a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantU2Both(uint expect) { return expect == (" + a.ToString() + " - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<U4_Constant>("", "Test", "SubConstantU2Both", (uint)(a - b)));
        }

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
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubI4(int a, int b)
        {
            CodeSource = "static class Test { static bool SubI4(int expect, int a, int b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "SubI4", (a - b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Row(-23, 148)]
        [Row(17, 1)]
        [Row(0, 0)]
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantI4(int a, int b)
        {
            // right side constant
            CodeSource = "static class Test { static bool SubConstantI4Right(int expect, int a) { return expect == (a - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "SubConstantI4Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantI4Left(int expect, int b) { return expect == (" + a.ToString() + " - b); } }";
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "SubConstantI4Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantI4Both(int expect) { return expect == (" + a.ToString() + " - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I4_Constant>("", "Test", "SubConstantI4Both", (a - b)));
        }

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
        // Extremvaluecases
        [Row(long.MinValue, long.MaxValue)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubI8(long a, long b)
        {
            CodeSource = "static class Test { static bool SubI8(long expect, long a, long b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "SubI8", (a - b), a, b));
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
        [Row(long.MinValue, long.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantI8(long a, long b)
        {
            // right side constant
            CodeSource = "static class Test { static bool SubConstantI8Right(long expect, long a) { return expect == (a - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "SubConstantI8Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantI8Left(long expect, long b) { return expect == (" + a.ToString() + " - b); } }";
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "SubConstantI8Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantI8Both(long expect) { return expect == (" + a.ToString() + " - " + b.ToString() + "); } }";
            Assert.IsTrue((bool)Run<I8_Constant>("", "Test", "SubConstantI8Both", (a - b)));
        }
        
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
        [Row(1.2f, 2.1f)]
        [Row(23.0f, 21.2578f)]
        [Row(1.0f, -2.198f)]
        [Row(-1.2f, 2.11f)]
        [Row(0.0f, 0.0f)]
        // (MinValue, X) Cases
        [Row(float.MinValue, 0.0f)]
        [Row(float.MinValue, 1.2f)]
        [Row(float.MinValue, 17.6f)]
        [Row(float.MinValue, 123.1f)]
        [Row(float.MinValue, -0.0f)]
        [Row(float.MinValue, -1.5f)]
        [Row(float.MinValue, -17.99f)]
        [Row(float.MinValue, -123.235f)]
        // (MaxValue, X) Cases
        [Row(float.MaxValue, 0.0f)]
        [Row(float.MaxValue, 1.67f)]
        [Row(float.MaxValue, 17.875f)]
        [Row(float.MaxValue, 123.283f)]
        [Row(float.MaxValue, -0.0f)]
        [Row(float.MaxValue, -1.1497f)]
        [Row(float.MaxValue, -17.12f)]
        [Row(float.MaxValue, -123.34f)]
        // (X, MinValue) Cases
        [Row(0.0f, float.MinValue)]
        [Row(1.2, float.MinValue)]
        [Row(17.4f, float.MinValue)]
        [Row(123.561f, float.MinValue)]
        [Row(-0.0f, float.MinValue)]
        [Row(-1.78f, float.MinValue)]
        [Row(-17.59f, float.MinValue)]
        [Row(-123.41f, float.MinValue)]
        // (X, MaxValue) Cases
        [Row(0.0f, float.MaxValue)]
        [Row(1.00012f, float.MaxValue)]
        [Row(17.094002f, float.MaxValue)]
        [Row(123.001f, float.MaxValue)]
        [Row(-0.0f, float.MaxValue)]
        [Row(-1.045f, float.MaxValue)]
        [Row(-17.0002501f, float.MaxValue)]
        [Row(-123.023f, float.MaxValue)]
        // Extremvaluecases
        [Row(1.0f, float.NaN)]
        [Row(float.NaN, 1.0f)]
        [Row(1.0f, float.PositiveInfinity)]
        [Row(float.PositiveInfinity, 1.0f)]
        [Row(1.0f, float.NegativeInfinity)]
        [Row(float.NegativeInfinity, 1.0f)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubR4(float a, float b)
        {
            CodeSource = "static class Test { static bool SubR4(float expect, float a, float b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "SubR4", (a - b), a, b));
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
        [Row(-17.0002501f, float.MaxValue)]
        [Test, Author("boddlnagg", "kpreisert@googlemail.com")]
        public void SubConstantR4(float a, float b)
        {
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f";
            
            // right side constant
            CodeSource = "static class Test { static bool SubConstantR4Right(float expect, float a) { return expect == (a - " + y + "); } }";
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "SubConstantR4Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantR4Left(float expect, float b) { return expect == (" + x + " - b); } }";
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "SubConstantR4Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantR4Both(float expect) { return expect == (" + x + " - " + y + "); } }";
            Assert.IsTrue((bool)Run<R4_Constant>("", "Test", "SubConstantR4Both", (a - b)));
        }
        
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
        [Row(1.2, 2.1)]
        [Row(23.0, 21.2578)]
        [Row(1.0, -2.198)]
        [Row(-1.2, 2.11)]
        [Row(0.0, 0.0)]
        [Row(-17.1, -2.3)]
        // (MinValue, X) Cases
        [Row(double.MinValue, 0.0)]
        [Row(double.MinValue, 1.2)]
        [Row(double.MinValue, 17.6)]
        [Row(double.MinValue, 123.1)]
        [Row(double.MinValue, -0.0)]
        [Row(double.MinValue, -1.5)]
        [Row(double.MinValue, -17.99)]
        [Row(double.MinValue, -123.235)]
        // (MaxValue, X) Cases
        [Row(double.MaxValue, 0.0)]
        [Row(double.MaxValue, 1.67)]
        [Row(double.MaxValue, 17.875)]
        [Row(double.MaxValue, 123.283)]
        [Row(double.MaxValue, -0.0)]
        [Row(double.MaxValue, -1.1497)]
        [Row(double.MaxValue, -17.12)]
        [Row(double.MaxValue, -123.34)]
        // (X, MinValue) Cases
        [Row(0.0, double.MinValue)]
        [Row(1.2, double.MinValue)]
        [Row(17.4, double.MinValue)]
        [Row(123.561, double.MinValue)]
        [Row(-0.0, double.MinValue)]
        [Row(-1.78, double.MinValue)]
        [Row(-17.59, double.MinValue)]
        [Row(-123.41, double.MinValue)]
        // (X, MaxValue) Cases
        [Row(0.0, double.MaxValue)]
        [Row(1.00012, double.MaxValue)]
        [Row(17.094002, double.MaxValue)]
        [Row(123.001, double.MaxValue)]
        [Row(-0.0, double.MaxValue)]
        [Row(-1.045, double.MaxValue)]
        [Row(-17.0002501, double.MaxValue)]
        [Row(-123.023, double.MaxValue)]
        // Extremvaluecases
        [Row(double.MinValue, double.MaxValue)]
        [Row(1.0f, double.NaN)]
        [Row(double.NaN, 1.0f)]
        [Row(1.0f, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0f)]
        [Row(1.0f, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0f)]
        [Test, Author("rootnode", "rootnode@mosa-project.org")]
        public void SubR8(double a, double b)
        {
            CodeSource = "static class Test { static bool SubR8(double expect, double a, double b) { return expect == (a - b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "SubR8", (a - b), a, b));
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
        public void SubConstantR8(double a, double b)
        {                
            string x = a.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string y = b.ToString(System.Globalization.CultureInfo.InvariantCulture);
            
            // right side constant
            CodeSource = "static class Test { static bool SubConstantR8Right(double expect, double a) { return expect == (a - " + y + "); } }";
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "SubConstantR8Right", (a - b), a));
            
            // left side constant
            CodeSource = "static class Test { static bool SubConstantR8Left(double expect, double b) { return expect == (" + x + " - b); } }";
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "SubConstantR8Left", (a - b), b));
            
            // both constant
            CodeSource = "static class Test { static bool SubConstantR8Both(double expect) { return expect == (" + x + " - " + y + "); } }";
            Assert.IsTrue((bool)Run<R8_Constant>("", "Test", "SubConstantR8Both", (a - b)));
        }
    }
}
