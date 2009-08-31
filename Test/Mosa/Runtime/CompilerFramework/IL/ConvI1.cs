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
using NUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConvI1 : CodeDomTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I1(sbyte expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(sbyte.MinValue), TestCase(sbyte.MaxValue)]
        [Test]
        public void ConvI1_I1(sbyte a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_I1(sbyte expect, sbyte a) 
                    { 
                        return expect == (sbyte)a; 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I1>("", "Test", "ConvI1_I1", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I2(sbyte expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(short.MinValue), TestCase(short.MaxValue)]
        [Test]
        public void ConvI1_I2(short a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_I2(sbyte expect, short a) 
                    { 
                        return expect == (sbyte)a; 
                    }
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I2>("", "Test", "ConvI1_I2", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I4(sbyte expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(int.MinValue), TestCase(int.MaxValue)]
        [Test]
        public void ConvI1_I4(int a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_I4(sbyte expect, int a) 
                    { 
                        return expect == (sbyte)a; 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I4>("", "Test", "ConvI1_I4", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_I8(sbyte expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0), TestCase(1), TestCase(2), TestCase(sbyte.MinValue), TestCase(sbyte.MaxValue), TestCase(long.MinValue), TestCase(long.MaxValue)]
        [Test]
        public void ConvI1_I8(long a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_I8(sbyte expect, long a) 
                    { 
                        return expect == (sbyte)a; 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_I8>("", "Test", "ConvI1_I8", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_R4(sbyte expect, float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0.0f), TestCase(1.0f), TestCase(2.0f), TestCase((float)sbyte.MinValue), TestCase((float)sbyte.MaxValue), TestCase(Single.MinValue), TestCase(Single.MaxValue)]
        [Test]
        public void ConvI1_R4(float a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_R4(sbyte expect, float a) 
                    { 
                        return expect == (sbyte)a; 
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_R4>("", "Test", "ConvI1_R4", ((sbyte)a), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool Native_ConvI1_R8(sbyte expect, double a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [TestCase(0.0), TestCase(1.0), TestCase(2.0), TestCase((double)sbyte.MinValue), TestCase((double)sbyte.MaxValue), TestCase(Double.MinValue), TestCase(Double.MaxValue)]
        [Test]
        public void ConvI1_R8(double a)
        {
            CodeSource = @"
                static class Test { 
                    static bool ConvI1_R8(sbyte expect, double a) 
                    { 
                        return expect == (sbyte)a;
                    } 
                }";
            Assert.IsTrue((bool)Run<Native_ConvI1_R8>("", "Test", "ConvI1_R8", ((sbyte)a), a));
        }
    }
}
