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
using System.Reflection.Emit;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class ConvI8 : MosaCompilerTestRunner
    {
        delegate bool Native_ConvI8_I1(long expect, sbyte a);
        [Column(0, 1, 2, sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_I1(sbyte a)
        {
            CodeSource = "static class Test { static bool ConvI8_I1(long expect, sbyte a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I1>("", "Test", "ConvI8_I1", ((long)a), a));
        }

        delegate bool Native_ConvI8_I2(long expect, short a);
        [Column(0, 1, 2, short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_I2(short a)
        {
            CodeSource = "static class Test { static bool ConvI8_I2(long expect, short a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I2>("", "Test", "ConvI8_I2", ((long)a), a));
        }

        delegate bool Native_ConvI8_I4(long expect, int a);
        [Column(0, 1, 2, int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_I4(int a)
        {
            CodeSource = "static class Test { static bool ConvI8_I4(long expect, int a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I4>("", "Test", "ConvI8_I4", ((long)a), a));
        }

        delegate bool Native_ConvI8_I8(long expect, long a);
        [Column(0, 1, 2, int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_I8(long a)
        {
            CodeSource = "static class Test { static bool ConvI8_I8(long expect, long a) { return expect == ((long)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI8_I4>("", "Test", "ConvI8_I8", ((long)a), a));
        }

        delegate bool Native_ConvI4_R4(int expect, float a);
        [Column(0, 1, 2, long.MinValue, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_R4(float a)
        {
            CodeSource = "static class Test { static bool ConvI1_R4(int expect, float a) { return expect == ((int)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI4_R4>("", "Test", "ConvI1_R4", ((int)a), a));
        }

        delegate bool Native_ConvI4_R8(int expect, double a);
        [Column(0, 1, 2, long.MinValue, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ConvI8_R8(double a)
        {
            CodeSource = "static class Test { static bool ConvI1_R8(int expect, double a) { return expect == ((int)a); } }";
            Assert.IsTrue((bool)Run<Native_ConvI4_R8>("", "Test", "ConvI1_R8", ((int)a), a));
        }
    }
}
