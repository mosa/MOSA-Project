/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConvI2 : CodeDomTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_I1(short expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((sbyte)0), TestCase((sbyte)1), TestCase((sbyte)2), TestCase(sbyte.MinValue), TestCase(sbyte.MaxValue)]
        [Test]
        public void ConvI2_I1(sbyte a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI2_I1(short expect, sbyte a) 
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_I1>("", "Test", "ConvI2_I1", ((short)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_I2(short expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase((short)0), TestCase((short)1), TestCase((short)2), TestCase(short.MinValue), TestCase(short.MaxValue)]
        [Test]
        public void ConvI2_I2(short a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI2_I2(short expect, short a)
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_I2>("", "Test", "ConvI2_I2", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_I4(short expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(int.MinValue), TestCase(int.MaxValue)]
        [Test]
        public void ConvI2_I4(int a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI2_I4(short expect, int a) 
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_I4>("", "Test", "ConvI2_I4", ((short)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_I8(short expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(long.MinValue), TestCase(long.MaxValue)]
        [Test]
        public void ConvI2_I8(long a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI2_I8(short expect, long a) 
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_I8>("", "Test", "ConvI2_I8", ((short)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_R4(short expect, float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0.0f), TestCase(1.0f), TestCase(2.0f), TestCase(Single.MinValue), TestCase(Single.MaxValue)]
        [Test]
        public void ConvI2_R4(float a)
        {
            CodeSource = @"
                static class Test 
                { 
                    static bool ConvI2_R4(short expect, float a) 
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_R4>("", "Test", "ConvI2_R4", ((short)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI2_R8(short expect, double a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0.0), TestCase(1.0), TestCase(2.0), TestCase(Double.MinValue), TestCase(Double.MaxValue)]
        [Test]
        public void ConvI2_R8(double a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI2_R8(short expect, double a) 
                    { 
                        return expect == ((short)a); 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI2_R8>("", "Test", "ConvI2_R8", ((short)a), a));
        }
    }
}
