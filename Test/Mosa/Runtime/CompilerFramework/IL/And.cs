using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class And : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        [Row(1, 2)]
        [Row(0, 2)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool AndI1(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "AndI1", (a & b), a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        [Row(1, 2)]
        [Row(0, 2)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI2(short a, short b)
        {
            CodeSource = "static class Test { static bool AndI2(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "AndI2", (a & b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, int b);
        [Row(1, 2)]
        [Row(0, 2)]
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI4(int a, int b)
        {
            CodeSource = "static class Test { static bool AndI4(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AndI4", (a & b), a, b));
        }

        delegate bool I8_I8_I8(long expect, long a, long b);
        [Row(1, 2)]
        [Row(0, 2)]
        [Row(long.MinValue, long.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AndI8(long a, long b)
        {
            CodeSource = "static class Test { static bool AndI8(int expect, sbyte a, sbyte b) { return (a & b) == expect; } }";
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "AndI8", (a & b), a, b));
        }
    }
}
