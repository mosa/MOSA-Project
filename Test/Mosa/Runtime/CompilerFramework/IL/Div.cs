using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    [TestFixture]
    public class Div : MosaCompilerTestRunner
    {
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, -2)]
        [Row(-4, 2)]
        [Row(-4, -2)]
        [Row(3, 2)]
        [Row(3, 4)]
        [Row(6, 6)]
        [Row(5, 6)]
        [Row(16, 4)]
        [Row(11, 9)]
        [Row(100, 10)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(sbyte.MinValue, sbyte.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void DivI1(sbyte a, sbyte b)
        {
            CodeSource = "static class Test { static bool DivI1(int expect, sbyte a, sbyte b) { return expect == (a / b); } }";
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "DivI1", a / b, a, b));
        }

        delegate bool I4_I2_I2(int expect, short a, short b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, -2)]
        [Row(-4, 2)]
        [Row(-4, -2)]
        [Row(3, 2)]
        [Row(3, 4)]
        [Row(6, 6)]
        [Row(5, 6)]
        [Row(16, 4)]
        [Row(11, 9)]
        [Row(100, 10)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Row(short.MinValue, short.MaxValue)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void DivI2(short a, short b)
        {
            CodeSource = "static class Test { static bool DivI2(int expect, short a, short b) { return expect == (a / b); } }";
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "DivI2", a / b, a, b));
        }
        
        delegate bool I4_I4_I4(int expect, int a, int b);
        [Row(8, 10)]
        [Row(4, 2)]
        [Row(4, -2)]
        [Row(-4, 2)]
        [Row(-4, -2)]
        [Row(3, 2)]
        [Row(3, 4)]
        [Row(6, 6)]
        [Row(5, 6)]
        [Row(16, 4)]
        [Row(11, 9)]
        [Row(100, 10)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void DivI4(int a, int b)
        {
            CodeSource = "static class Test { static bool DivI4(int expect, int a, int b) { return expect == (a / b); } }";
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "DivI4", (a / b), a, b));
        }

        delegate bool R4_R4_R4(float expect, float a, float b);
        [Row(1.0f, 2.0f)]
        [Row(2.0f, 1.0f)]
        [Row(1.0f, 2.5f)]
        [Row(1.7f, 2.3f)]
        [Row(2.0f, -1.0f)]
        [Row(1.0f, -2.5f)]
        [Row(1.7f, -2.3f)]
        [Row(-2.0f, 1.0f)]
        [Row(-1.0f, 2.5f)]
        [Row(-1.7f, 2.3f)]
        [Row(-2.0f, -1.0f)]
        [Row(-1.0f, -2.5f)]
        [Row(-1.7f, -2.3f)]
        [Row(1.0f, float.NaN)]
        [Row(float.NaN, 1.0f)]
        [Row(1.0f, float.PositiveInfinity)]
        [Row(float.PositiveInfinity, 1.0f)]
        [Row(1.0f, float.NegativeInfinity)]
        [Row(float.NegativeInfinity, 1.0f)]
        [Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void DivR4(float a, float b)
        {
            CodeSource = "static class Test { static bool DivR4(float expect, float a, float b) { return expect == (a / b); } }";
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "DivR4", (a / b), a, b));
        }

        delegate bool R8_R8_R8(double expect, double a, double b);
        [Row(1.0, 2.0)]
        [Row(2.0, 1.0)]
        [Row(1.0, 2.5)]
        [Row(1.7, 2.3)]
        [Row(2.0, -1.0)]
        [Row(1.0, -2.5)]
        [Row(1.7, -2.3)]
        [Row(-2.0, 1.0)]
        [Row(-1.0, 2.5)]
        [Row(-1.7, 2.3)]
        [Row(-2.0, -1.0)]
        [Row(-1.0, -2.5)]
        [Row(-1.7, -2.3)]
        [Row(1.0, double.NaN)]
        [Row(double.NaN, 1.0)]
        [Row(1.0, double.PositiveInfinity)]
        [Row(double.PositiveInfinity, 1.0)]
        [Row(1.0, double.NegativeInfinity)]
        [Row(double.NegativeInfinity, 1.0)]
        [Row(1.0, 0.0)]
        [Test, Author("alyman", "mail.alex.lyman@gmail.com")]
        public void DivR8(double a, double b)
        {
            CodeSource = "static class Test { static bool DivR8(double expect, double a, double b) { return expect == (a / b); } }";
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "DivR8", (a / b), a, b));
        }
    }
}
