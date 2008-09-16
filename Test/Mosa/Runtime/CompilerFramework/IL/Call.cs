/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
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
    public class Call : MosaCompilerTestRunner
    {
        /// <summary>
        /// 
        /// </summary>
        delegate void V();
        /// <summary>
        /// 
        /// </summary>
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void CallEmpty()
        {
            CodeSource = @"
                static class Test { 
                    static void CallEmpty() { CallEmpty_Target(); } 
                    static void CallEmpty_Target() { }
                }";
            Run<V>("", "Test", "CallEmpty");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        delegate bool B__I1(sbyte arg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(sbyte.MinValue)]
        [Row(sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void CallI1(sbyte value)
        {
            CodeSource = @"
                static class Test {
                    static bool CallI1(sbyte value) { return value == CallI1_Target(value); } 
                    static sbyte CallI1_Target(sbyte value) { return value; }
                }";
            Assert.IsTrue((bool)Run<B__I1>("", "Test", "CallI1", value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        delegate bool B__I2(short arg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(short.MinValue)]
        [Row(short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void CallI2(short value)
        {
            CodeSource = @"
                static class Test {
                    static bool CallI2(short value) { return value == CallI2_Target(value); } 
                    static short CallI2_Target(short value) { return value; }
                }";
            Assert.IsTrue((bool)Run<B__I2>("", "Test", "CallI2", value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        delegate bool B__I4(int arg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(int.MinValue)]
        [Row(int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void CallI4(int value)
        {
            CodeSource = @"
                static class Test {
                    static bool CallI4(int value) { return value == CallI4_Target(value); } 
                    static int CallI4_Target(int value) { return value; }
                }";
            Assert.IsTrue((bool)Run<B__I4>("", "Test", "CallI4", value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        delegate bool B__I8(int arg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [Row(0)]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(11)]
        [Row(100)]
        [Row(-0)]
        [Row(-1)]
        [Row(-2)]
        [Row(-5)]
        [Row(-10)]
        [Row(-11)]
        [Row(-100)]
        [Row(long.MinValue)]
        [Row(long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void CallI8(long value)
        {
            CodeSource = @"
                static class Test {
                    static bool CallI8(long value) { return value == CallI8_Target(value); } 
                    static long CallI8_Target(long value) { return value; }
                }";
            Assert.IsTrue((bool)Run<B__I8>("", "Test", "CallI8", value));
        }

        /// <summary>
        /// 
        /// </summary>
        private delegate bool B_I4_I4_I4_I4(int a, int b, int c, int d);

        /// <summary>
        /// Checks the method call parameter order.
        /// </summary>
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void CallOrder()
        {
            CodeSource = @"
                static class Test {
                    static bool CallOrder(int a, int b, int c, int d) {
                        return (a == 1 && b == 2 && c == 3 && d == 4);
                    }
                }
            ";

            Assert.IsTrue((bool)Run<B_I4_I4_I4_I4>(@"", @"Test", @"CallOrder", 1, 2, 3, 4));
        }

        /// <summary>
        /// 
        /// </summary>
        private delegate bool B_I4(int arg);

        /// <summary>
        /// Tests intrinsic compiler calls.
        /// </summary>
        [Column(1, 2, Int32.MaxValue, Int32.MinValue)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void CallIntrinsic(int arg)
        {
            CodeSource = @"
                static class Test {
                    static bool CallIntrinsic(int arg) {
                        return (arg == CallMov(arg));
                    }

                    [Mosa.Runtime.CompilerFramework.IntrinsicAttribute(typeof(Mosa.Platforms.x86.Architecture), typeof(Mosa.Runtime.CompilerFramework.IR.MoveInstruction))]
                    static int CallMov(int arg) {
                        return 0;
                    }
                }
            ";

            this.References = new string[] {
                @"Mosa.Runtime.dll",
                @"Mosa.Platforms.x86.dll"
            };
            Assert.IsTrue((bool)Run<B_I4>(@"", @"Test", @"CallIntrinsic", arg));
        }
    }
}
