using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Shl : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(1, 8)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShlI1(sbyte a, byte b)
        {
            CodeSource = "static class Test { static bool ShlI1(int expect, sbyte a, byte b) { return expect == (a << b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "ShlI1", a << b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(1, 16)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShlI2(short a, byte b)
        {
            CodeSource = "static class Test { static bool ShlI2(int expect, short a, byte b) { return expect == (a << b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "ShlI2", (a << b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(1, 32)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShlI4(int a, byte b)
        {
            CodeSource = "static class Test { static bool ShlI4(int expect, int a, byte b) { return expect == (a << b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "ShlI4", (a << b), a, b));
        }

        delegate bool I8_I8_I8(long expect, long a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(unchecked((long)0x8000000000000000), 64)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShlI8(long a, byte b)
        {
            CodeSource = "static class Test { static bool ShrI8(long expect, long a, byte b) { return expect == (a << b); } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "ShrI8", (a << b), a, b));
        }
    }
}
