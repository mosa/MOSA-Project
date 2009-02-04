/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
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
    /// 
    /// </summary>
    [TestFixture]
    public class Ldarga : CodeDomTestRunner
    {
        #region CheckValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I1_I1(sbyte expect, sbyte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI1_CheckValue(sbyte a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaI1_CheckValue(sbyte expect, sbyte a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(sbyte expect, ref sbyte a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<I1_I1>("", "Test", "LdargaI1_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U1_U1(byte expect, byte a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, byte.MinValue, byte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaU1_CheckValue(byte a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaU1_CheckValue(byte expect, byte a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(byte expect, ref byte a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<U1_U1>("", "Test", "LdargaU1_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I2_I2(short expect, short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI2_CheckValue(short a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaI2_CheckValue(short expect, short a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(short expect, ref short a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<I2_I2>("", "Test", "LdargaI2_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U2_U2(ushort expect, ushort a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, ushort.MinValue, ushort.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaU2_CheckValue(ushort a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaU2_CheckValue(ushort expect, ushort a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(ushort expect, ref ushort a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<U2_U2>("", "Test", "LdargaU2_CheckValue", a, a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I4_I4(int expect, int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI4_CheckValue(int a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaI4_CheckValue(int expect, int a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(int expect, ref int a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<I4_I4>("", "Test", "LdargaI4_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U4_U4(uint expect, uint a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, uint.MinValue, uint.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaU4_CheckValue(uint a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaU4_CheckValue(uint expect, uint a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(uint expect, ref uint a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<U4_U4>("", "Test", "LdargaU4_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool I8_I8(long expect, long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, long.MinValue, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI8_CheckValue(long a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaI8_CheckValue(long expect, long a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(long expect, ref long a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<I8_I8>("", "Test", "LdargaI8_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool U8_U8(ulong expect, ulong a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, ulong.MinValue, ulong.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaU8_CheckValue(ulong a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaU8_CheckValue(ulong expect, ulong a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(ulong expect, ref ulong a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<U8_U8>("", "Test", "LdargaU8_CheckValue", a, a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool R4_R4(float expect, float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, float.MinValue, float.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaR4_CheckValue(float a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaR4_CheckValue(float expect, float a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(float expect, ref float a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<R4_R4>("", "Test", "LdargaR4_CheckValue", a, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate bool R8_R8(double expect, double a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [Column(0, 1, double.MinValue, double.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaR8_CheckValue(double a)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaR8_CheckValue(double expect, double a) 
                    {
                        return CheckValue(expect, ref a);
                    }

                    static bool CheckValue(double expect, ref double a)
                    {
                        return expect == a;
                    }
                }";
            Assert.IsTrue((bool)Run<R8_R8>("", "Test", "LdargaR8_CheckValue", a, a));
        }
        

        #endregion

        #region ChangeValue
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate bool B_I1_I1(sbyte value, ref sbyte a);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, sbyte.MinValue), Row(0, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI1_ChangeValue(sbyte newValue, sbyte oldValue)
        {
            CodeSource = @"
                static class Test
                { 
                    static bool LdargaI1_ChangeValue(sbyte expect, sbyte a) 
                    {
                        ChangeValue(expect, ref a);
                        return expect == a;
                    }

                    static void ChangeValue(sbyte expect, ref sbyte a)
                    {
                        a = expect;
                    }
                }";
            Assert.IsTrue((bool)Run<B_I1_I1>("", "Test", "LdargaI1_ChangeValue", newValue, oldValue));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate void V_I2_I2(short value, ref short a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, short.MinValue), Row(0, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI2_ChangeValue(short newValue, short oldValue)
        {
            CodeSource = "static class Test { static void LdargaI2_ChangeValue(short value, ref short a) { a = value; } }";
            object[] args = new object[] { newValue, oldValue };
            Run<V_I2_I2>("", "Test", "LdargaI2_ChangeValue", args);
            Console.WriteLine("{0} {1} {2}", newValue, args[0], args[1]);
            Assert.AreEqual(newValue, args[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate void V_I4_I4(int value, ref int a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, int.MinValue), Row(0, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI4_ChangeValue(int newValue, int oldValue)
        {
            CodeSource = @"static class Test { static void LdargaI4_ChangeValue(int value, ref int a) { a = value; } }";
            object[] args = new object[] { newValue, oldValue };
            Run<V_I4_I4>("", "Test", "LdargaI4_ChangeValue", args);
            Console.WriteLine("{0} {1} {2}", newValue, args[0], args[1]);
            Assert.AreEqual(newValue, args[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate void V_I8_I8(long value, ref long a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, long.MinValue), Row(0, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaI8_ChangeValue(long newValue, long oldValue)
        {
            CodeSource = "static class Test { static void LdargaI8_ChangeValue(long value, ref long a) { a = value; } }";
            object[] args = new object[] { newValue, oldValue };
            Run<V_I8_I8>("", "Test", "LdargaI8_ChangeValue", args);
            Console.WriteLine("{0} {1} {2}", newValue, args[0], args[1]);
            Assert.AreEqual(newValue, args[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate void V_R4_R4(float value, ref float a);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, float.MinValue), Row(0, float.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaR4_ChangeValue(float newValue, float oldValue)
        {
            CodeSource = "static class Test { static void LdargaR4_ChangeValue(float value, ref float a) { a = value; } }";
            object[] args = new object[] { newValue, oldValue };
            Run<V_R4_R4>("", "Test", "LdargaR4_ChangeValue", args);
            Console.WriteLine("{0} {1} {2}", newValue, args[0], args[1]);
            Assert.AreEqual(newValue, args[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        delegate void V_R8_R8(double value, ref double a);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [Row(1, 0), Row(0, 1), Row(1, double.MinValue), Row(0, double.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void LdargaR8_ChangeValue(double newValue, double oldValue)
        {
            CodeSource = "static class Test { static void LdargaR8_ChangeValue(double value, ref double a) { a = value; } }";
            object[] args = new object[] { newValue, oldValue };
            Run<V_R8_R8>("", "Test", "LdargaR8_ChangeValue", args);
            Console.WriteLine("{0} {1} {2}", newValue, args[0], args[1]);
            Assert.AreEqual(newValue, args[1]);
        }

        #endregion
    }
}
