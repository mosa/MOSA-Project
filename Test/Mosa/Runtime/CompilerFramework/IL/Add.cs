using System;
using Gallio.Framework;
using MbUnit.Framework;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Add : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        [Row(1, 2)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AddI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool AddI1(int expect, sbyte a, sbyte b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "AddI1", a + b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        [Row(1, 2)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AddI2(short a, short b)
        {
            CodeSource = "static class Test { static bool AddI2(int expect, short a, short b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "AddI2", (a + b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, int b);
        [Row(1, 2)]
        [Row(int.MinValue, int.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AddI4(int a, int b)
        {
            CodeSource = "static class Test { static bool AddI4(int expect, int a, int b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AddI4", (a + b), a, b));
        }

        delegate bool R4_R4_R4(float expect, float a, float b);
        [Row(1.0f, 2.0f)]
        [Row(1.0f, float.NaN)]
        [Row(float.NaN, 1.0f)]
        [Row(1.0f, float.PositiveInfinity)]
        [Row(float.PositiveInfinity, 1.0f)]
        [Row(1.0f, float.NegativeInfinity)]
        [Row(float.NegativeInfinity, 1.0f)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AddR4(float a, float b)
        {
            CodeSource = "static class Test { static bool AddR4(float expect, float a, float b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "AddR4", (a + b), a, b));
        }

        delegate bool R8_R8_R8(double expect, double a, double b);
        [Row(1.0, 2.0)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void AddR8(double a, double b)
        {
            CodeSource = "static class Test { static bool AddR8(double expect, double a, double b) { return expect == (a + b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "AddR8", (a + b), a, b));
        }
    }
}
