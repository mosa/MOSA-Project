using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Provides test cases for the cpblk IL operation.
    /// </summary>
    [TestFixture]
    public class Stsfld : CodeDomTestRunner
    {
        private static string s_testCode = @"
            static class Test {
                private static type s_fld;
                public static bool Stsfld(type value) {
                    s_fld = value;
                    return (value == s_fld);
                }
            }
        ";


        private delegate bool B_B(bool value);
        private delegate bool B_C(char value);
        private delegate bool B_I1(sbyte value);
        private delegate bool B_I2(short value);
        private delegate bool B_I4(int value);
        private delegate bool B_I8(long value);
        private delegate bool B_U1(byte value);
        private delegate bool B_U2(ushort value);
        private delegate bool B_U4(uint value);
        private delegate bool B_U8(ulong value);
        private delegate bool B_R4(float value);
        private delegate bool B_R8(double value);


        /// <summary>
        /// Tests the stsfld operation for the boolean type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(true)]
        [TestCase(false)]
        [Test]
        
        
        public void StsfldB(bool value)
        {
            this.CodeSource = s_testCode.Replace("type", "bool");
            bool res = (bool)Run<B_B>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the char type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Char.MinValue)]
        [TestCase(Char.MaxValue)]
        [TestCase('a')]
        [TestCase('z')]
        [TestCase('0')]
        [TestCase('9')]
        [Test]
        
        
        public void StsfldC(char value)
        {
            this.CodeSource = s_testCode.Replace("type", "char");
            bool res = (bool)Run<B_C>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the sbyte type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(SByte.MaxValue)]
        [TestCase(SByte.MinValue)]
        [TestCase((sbyte)0)]
        [TestCase((sbyte)1)]
        [TestCase((sbyte)-1)]
        [Test]
        
        
        public void StsfldI1(sbyte value)
        {
            this.CodeSource = s_testCode.Replace("type", "sbyte");
            bool res = (bool)Run<B_I1>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the short type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Int16.MaxValue)]
        [TestCase(Int16.MinValue)]
        [TestCase((short)0)]
        [TestCase((short)1)]
        [TestCase((short)-1)]
        [Test]
        
        
        public void StsfldI2(short value)
        {
            this.CodeSource = s_testCode.Replace("type", "short");
            bool res = (bool)Run<B_I2>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the int type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Int32.MaxValue)]
        [TestCase(Int32.MinValue)]
        [TestCase(0, 1, -1)]
        [Test]
        
        
        public void StsfldI4(int value)
        {
            this.CodeSource = s_testCode.Replace("type", "int");
            bool res = (bool)Run<B_I4>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the long type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Int64.MaxValue)]
        [TestCase(Int64.MinValue)]
        [TestCase(0L, 1L, -1L)]
        [Test]
        
        
        public void StsfldI8(long value)
        {
            this.CodeSource = s_testCode.Replace("type", "long");
            bool res = (bool)Run<B_I8>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the byte type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Byte.MaxValue)]
        [TestCase(Byte.MinValue)]
        [TestCase((byte)0U)]
        [TestCase((byte)1U)]
        [TestCase((byte)0xFFU)]
        [Test]
        
        
        public void StsfldU1(byte value)
        {
            this.CodeSource = s_testCode.Replace("type", "byte");
            bool res = (bool)Run<B_U1>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the ushort type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(UInt16.MaxValue)]
        [TestCase(UInt16.MinValue)]
        [TestCase((ushort)0U)]
        [TestCase((ushort)1U)]
        [TestCase((ushort)0xFFFFU)]
        [Test]
        
        
        public void StsfldU2(ushort value)
        {
            this.CodeSource = s_testCode.Replace("type", "ushort");
            bool res = (bool)Run<B_U2>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the uint type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(UInt32.MaxValue)]
        [TestCase(UInt32.MinValue)]
        [TestCase(0U, 1U, 0xFFFFFFFFU)]
        [Test]
        
        
        public void StsfldU4(uint value)
        {
            this.CodeSource = s_testCode.Replace("type", "uint");
            bool res = (bool)Run<B_U4>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the ulong type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(UInt64.MaxValue)]
        [TestCase(UInt64.MinValue)]
        [TestCase(0UL, 1UL, 0xFFFFFFFFFFFFFFFFUL)]
        [Test]
        
        
        public void StsfldU8(ulong value)
        {
            this.CodeSource = s_testCode.Replace("type", "ulong");
            bool res = (bool)Run<B_U8>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the float type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Single.MaxValue)]
        [TestCase(Single.MinValue)]
        [TestCase(0.0f)]
        [TestCase(1.0f)]
        [TestCase(Single.NaN)]
        [TestCase(Single.NegativeInfinity)]
        [TestCase(Single.PositiveInfinity)]
        [TestCase(Single.Epsilon)]
        [Test]
        
        
        public void StsfldR4(float value)
        {
            this.CodeSource = s_testCode.Replace("type", "float");
            bool res = (bool)Run<B_R4>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the double type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [TestCase(Double.MaxValue)]
        [TestCase(Double.MinValue)]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(Double.NaN)]
        [TestCase(Double.NegativeInfinity)]
        [TestCase(Double.PositiveInfinity)]
        [TestCase(Double.Epsilon)]
        [Test]
        
        
        public void StsfldR8(double value)
        {
            this.CodeSource = s_testCode.Replace("type", "double");
            bool res = (bool)Run<B_R8>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }
    }
}
