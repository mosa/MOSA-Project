using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Mul : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Row(2, 0)]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool MulI1(int expect, sbyte a, sbyte b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "MulI1", a * b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Row(2, 0)]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI2(short a, short b)
        {
            CodeSource = "static class Test { static bool MulI2(int expect, short a, short b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "MulI2", (a * b), a, b));
        }

        delegate bool I4_I4_I4(int expect, int a, int b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, 2)]
        [Row(2, 0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulI4(int a, int b)
        {
            CodeSource = "static class Test { static bool MulI4(int expect, int a, int b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "MulI4", (a * b), a, b));
        }

        delegate bool R4_R4_R4(float expect, float a, float b);
        [Row(1.0f, 2.0f)]
        [Row(2.0f, 0.0f)]
        [Row(1.0f, float.NaN)]
        [Row(float.NaN, 1.0f)]
        [Row(1.0f, float.PositiveInfinity)]
        [Row(float.PositiveInfinity, 1.0f)]
        [Row(1.0f, float.NegativeInfinity)]
        [Row(float.NegativeInfinity, 1.0f)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulR4(float a, float b)
        {
            CodeSource = "static class Test { static bool MulR4(float expect, float a, float b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "MulR4", (a * b), a, b));
        }

        delegate bool R8_R8_R8(double expect, double a, double b);
        [Row(1.0, 2.0)]
        [Row(2.0, 0.0)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void MulR8(double a, double b)
        {
            CodeSource = "static class Test { static bool MulR8(double expect, double a, double b) { return expect == (a * b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "MulR8", (a * b), a, b));
        }
    }
}
