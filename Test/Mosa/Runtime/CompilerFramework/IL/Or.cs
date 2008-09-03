using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Or : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Row(0, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void OrI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool OrI1(int expect, sbyte a, sbyte b) { return expect == (a | b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "OrI1", a | b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void OrI2(short a, short b)
        {
            short e = (short)(a | b);
            CodeSource = "static class Test { static bool OrI2(int expect, short a, short b) { return expect == (a | b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "OrI2", (a | b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, int b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void OrI4(int a, int b)
        {
            CodeSource = "static class Test { static bool OrI4(int expect, int a, int b) { return expect == (a | b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "OrI4", (a | b), a, b));
        }
    }
}
