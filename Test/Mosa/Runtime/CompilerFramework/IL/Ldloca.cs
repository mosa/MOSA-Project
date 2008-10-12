/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Tests the compiler for proper support of the ldloca, call and stind opcodes.
    /// </summary>
    [TestFixture]
    public class Ldloca : CodeDomTestRunner
    {
        #region CheckValue

        delegate bool B_I1(sbyte value);

        /// <summary>
        /// Checks support for ldloca on I1 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, SByte.MinValue, SByte.MaxValue, SByte.MinValue + 1, SByte.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaI1_CheckValue(sbyte value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaI1_CheckValue(sbyte expect)
                    {
                        sbyte a = expect;
                        return LdlocaI1_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaI1_DoCheckValue(sbyte expect, ref sbyte value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_I1>("", "Test", "LdlocaI1_CheckValue", value));
        }

        delegate bool B_I2(short value);

        /// <summary>
        /// Checks support for ldloca on I2 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, Int16.MinValue, Int16.MaxValue, Int16.MinValue + 1, Int16.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaI2_CheckValue(short value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaI2_CheckValue(short expect)
                    {
                        short a = expect;
                        return LdlocaI2_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaI2_DoCheckValue(short expect, ref short value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_I2>("", "Test", "LdlocaI2_CheckValue", value));
        }

        delegate bool B_I4(int value);

        /// <summary>
        /// Checks support for ldloca on I4 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [ Column(0, 1, Int32.MinValue, Int32.MaxValue, Int32.MinValue + 1, Int32.MaxValue-1) ]
        [ Test, Author(@"grover", @"sharpos@michaelruck.de") ]
        public void LdlocaI4_CheckValue(int value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaI4_CheckValue(int expect)
                    {
                        int a = expect;
                        return LdlocaI4_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaI4_DoCheckValue(int expect, ref int value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_I4>("", "Test", "LdlocaI4_CheckValue", value));
        }

        delegate bool B_I8(long value);

        /// <summary>
        /// Checks support for ldloca on I8 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, Int64.MinValue, Int64.MaxValue, Int64.MinValue + 1, Int64.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaI8_CheckValue(long value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaI8_CheckValue(long expect)
                    {
                        long a = expect;
                        return LdlocaI8_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaI8_DoCheckValue(long expect, ref long value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_I8>("", "Test", "LdlocaI8_CheckValue", value));
        }

        delegate bool B_U1(byte value);

        /// <summary>
        /// Checks support for ldloca on U1 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, Byte.MinValue, Byte.MaxValue, Byte.MinValue + 1, Byte.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaU1_CheckValue(byte value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaU1_CheckValue(byte expect)
                    {
                        byte a = expect;
                        return LdlocaU1_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaU1_DoCheckValue(byte expect, ref byte value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_U1>("", "Test", "LdlocaU1_CheckValue", value));
        }

        delegate bool B_U2(ushort value);

        /// <summary>
        /// Checks support for ldloca on U2 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, UInt16.MinValue, UInt16.MaxValue, UInt16.MinValue + 1, UInt16.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaU2_CheckValue(ushort value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaU2_CheckValue(ushort expect)
                    {
                        ushort a = expect;
                        return LdlocaU2_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaU2_DoCheckValue(ushort expect, ref ushort value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_U2>("", "Test", "LdlocaU2_CheckValue", value));
        }

        delegate bool B_U4(uint value);

        /// <summary>
        /// Checks support for ldloca on U4 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, UInt32.MinValue, UInt32.MaxValue, UInt32.MinValue + 1, UInt32.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaU4_CheckValue(uint value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaU4_CheckValue(uint expect)
                    {
                        uint a = expect;
                        return LdlocaU4_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaU4_DoCheckValue(uint expect, ref uint value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_U4>("", "Test", "LdlocaU4_CheckValue", value));
        }

        delegate bool B_U8(ulong value);

        /// <summary>
        /// Checks support for ldloca on U8 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, UInt64.MinValue, UInt64.MaxValue, UInt64.MinValue + 1, UInt64.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaU8_CheckValue(ulong value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaU8_CheckValue(ulong expect)
                    {
                        ulong a = expect;
                        return LdlocaU8_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaU8_DoCheckValue(ulong expect, ref ulong value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_U8>("", "Test", "LdlocaU8_CheckValue", value));
        }

        delegate bool B_R4(float value);

        /// <summary>
        /// Checks support for ldloca on R4 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, Single.MinValue, Single.MaxValue, Single.MinValue + 1, Single.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaR4_CheckValue(float value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaR4_CheckValue(float expect)
                    {
                        float a = expect;
                        return LdlocaR4_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaR4_DoCheckValue(float expect, ref float value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_R4>("", "Test", "LdlocaR4_CheckValue", value));
        }

        delegate bool B_R8(double value);

        /// <summary>
        /// Checks support for ldloca on R8 operands.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Column(0, 1, Double.MinValue, Double.MaxValue, Double.MinValue + 1, Double.MaxValue - 1)]
        [Test, Author(@"grover", @"sharpos@michaelruck.de")]
        public void LdlocaR8_CheckValue(double value)
        {
            CodeSource = @"
                static class Test
                {
                    static bool LdlocaR8_CheckValue(double expect)
                    {
                        double a = expect;
                        return LdlocaR8_DoCheckValue(expect, ref a);
                    }

                    static bool LdlocaR8_DoCheckValue(double expect, ref double value)
                    {
                        return (expect == value);
                    }
                }
            ";
            Assert.IsTrue((bool)Run<B_R8>("", "Test", "LdlocaR8_CheckValue", value));
        }

        #endregion // CheckValue
    }
}
