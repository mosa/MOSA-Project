using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Shr : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(unchecked((sbyte)0x80), 8)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI1(sbyte a, byte b)
        {
            CodeSource = "static class Test { static bool ShrI1(int expect, sbyte a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "ShrI1", a >> b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(unchecked((short)0x8000), 16)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI2(short a, byte b)
        {
            CodeSource = "static class Test { static bool ShrI2(int expect, short a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "ShrI2", (a >> b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(unchecked((int)0x80000000), 32)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI4(int a, byte b)
        {
            CodeSource = "static class Test { static bool ShrI4(int expect, int a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "ShrI4", (a >> b), a, b));
        }

        delegate bool I8_I8_I8(long expect, long a, byte b);
        [Row(1, 1)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(unchecked((long)0x8000000000000000), 64)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void ShrI4(long a, byte b)
        {
            CodeSource = "static class Test { static bool ShrI4(long expect, long a, byte b) { return expect == (a >> b); } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "ShrI4", (a >> b), a, b));
        }
    }
}
