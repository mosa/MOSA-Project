/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Switch : MosaCompilerTestRunner
    {
        delegate bool I1_I1(sbyte expect, sbyte a);
        // Normal Testcases + (0, 0)
        [Row(1)]
        [Row(23)]
        [Row(-1)]
        [Row(0)]
        // And reverse
        [Row(2)]
        [Row(-2)]       
        // (MinValue, X) Cases
        [Row(sbyte.MinValue)]
        // (MaxValue, X) Cases
        [Row(sbyte.MaxValue)]
        [Test, Author("rootnode", "simon_wollwage@yahoo.co.jp")]
        public void SwitchI1(sbyte a)
        {
            CodeSource = @"static class Test { 
                static bool SwitchI1(sbyte expect, sbyte a) { return expect == Switch_Target(a); } 
                static sbyte Switch_Target(sbyte a)
                {
                    switch(a)
                    {
                        case 0:
                            return 0;
                            break;
                        case 1:
                            return 1;
                            break;
                        case -1:
                            return -1;
                            break;
                        case 2:
                            return 2;
                            break;
                        case -2:
                            return -2;
                            break;
                        case 23:
                            return 23;
                            break;
                        case sbyte.MinValue:
                            return sbyte.MinValue;
                            break;
                        case sbyte.MaxValue:
                            return sbyte.MaxValue;
                            break;
                        default:
                            return 42;
                            break;
                    }
                }
            }";
            Assert.IsTrue((bool)Run<I1_I1>("", "Test", "SwitchI1", a, a));
        }

        delegate bool I2_I2(short expect, short a);
        // Normal Testcases + (0, 0)
        [Row(1)]
        [Row(23)]
        [Row(-1)]
        [Row(0)]
        // And reverse
        [Row(2)]
        [Row(-2)]
        // (MinValue, X) Cases
        [Row(short.MinValue)]
        // (MaxValue, X) Cases
        [Row(short.MaxValue)]
        [Test, Author("rootnode", "simon_wollwage@yahoo.co.jp")]
        public void SwitchI2(short a)
        {
            CodeSource = @"static class Test { 
                static bool SwitchI2(short expect, short a) { return expect == Switch_Target(a); } 
                static short Switch_Target(short a)
                {
                    switch(a)
                    {
                        case 0:
                            return 0;
                            break;
                        case 1:
                            return 1;
                            break;
                        case -1:
                            return -1;
                            break;
                        case 2:
                            return 2;
                            break;
                        case -2:
                            return -2;
                            break;
                        case 23:
                            return 23;
                            break;
                        case short.MinValue:
                            return short.MinValue;
                            break;
                        case short.MaxValue:
                            return short.MaxValue;
                            break;
                        default:
                            return 42;
                            break;
                    }
                }
            }";
            Assert.IsTrue((bool)Run<I2_I2>("", "Test", "SwitchI2", a, a));
        }

        delegate bool I4_I4(int expect, int a);
        // Normal Testcases + (0, 0)
        [Row(1)]
        [Row(23)]
        [Row(-1)]
        [Row(0)]
        // And reverse
        [Row(2)]
        [Row(-2)]
        // (MinValue, X) Cases
        [Row(int.MinValue)]
        // (MaxValue, X) Cases
        [Row(int.MaxValue)]
        [Test, Author("rootnode", "simon_wollwage@yahoo.co.jp")]
        public void SwitchI4(int a)
        {
            CodeSource = @"static class Test { 
                static bool SwitchI4(int expect, int a) { return expect == Switch_Target(a); } 
                static int Switch_Target(int a)
                {
                    switch(a)
                    {
                        case 0:
                            return 0;
                            break;
                        case 1:
                            return 1;
                            break;
                        case -1:
                            return -1;
                            break;
                        case 2:
                            return 2;
                            break;
                        case -2:
                            return -2;
                            break;
                        case 23:
                            return 23;
                            break;
                        case int.MinValue:
                            return int.MinValue;
                            break;
                        case int.MaxValue:
                            return int.MaxValue;
                            break;
                        default:
                            return 42;
                            break;
                    }
                }
            }";
            Assert.IsTrue((bool)Run<I4_I4>("", "Test", "SwitchI4", a, a));
        }

        delegate bool I8_I8(long expect, long a);
        // Normal Testcases + (0, 0)
        [Row(1)]
        [Row(23)]
        [Row(-1)]
        [Row(0)]
        // And reverse
        [Row(2)]
        [Row(-2)]
        // (MinValue, X) Cases
        [Row(long.MinValue)]
        // (MaxValue, X) Cases
        [Row(long.MaxValue)]
        [Test, Author("rootnode", "simon_wollwage@yahoo.co.jp")]
        public void SwitchI8(long a)
        {
            CodeSource = @"static class Test { 
                static bool SwitchI8(long expect, long a) { return expect == Switch_Target(a); } 
                static long Switch_Target(long a)
                {
                    switch(a)
                    {
                        case 0:
                            return 0;
                            break;
                        case 1:
                            return 1;
                            break;
                        case -1:
                            return -1;
                            break;
                        case 2:
                            return 2;
                            break;
                        case -2:
                            return -2;
                            break;
                        case 23:
                            return 23;
                            break;
                        case long.MinValue:
                            return long.MinValue;
                            break;
                        case long.MaxValue:
                            return long.MaxValue;
                            break;
                        default:
                            return 42;
                            break;
                    }
                }
            }";
            Assert.IsTrue((bool)Run<I8_I8>("", "Test", "SwitchI8", a, a));
        }
    }
}