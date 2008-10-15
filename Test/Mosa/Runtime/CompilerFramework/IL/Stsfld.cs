using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

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
        [Row(true)]
        [Row(false)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Row(Char.MinValue)]
        [Row(Char.MaxValue)]
        [Row('a')]
        [Row('z')]
        [Row('0')]
        [Row('9')]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(SByte.MaxValue, SByte.MinValue, (sbyte)0, (sbyte)1, (sbyte)-1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(Int16.MaxValue, Int16.MinValue, (short)0, (short)1, (short)-1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(Int32.MaxValue, Int32.MinValue, 0, 1, -1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(Int64.MaxValue, Int64.MinValue, 0L, 1L, -1L)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(Byte.MaxValue, Byte.MinValue, (byte)0U, (byte)1U, (byte)0xFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(UInt16.MaxValue, UInt16.MinValue, (ushort)0U, (ushort)1U, (ushort)0xFFFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(UInt32.MaxValue, UInt32.MinValue, 0U, 1U, 0xFFFFFFFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(UInt64.MaxValue, UInt64.MinValue, 0UL, 1UL, 0xFFFFFFFFFFFFFFFFUL)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
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
        [Column(Single.MaxValue, Single.MinValue, 0.0f, 1.0f, Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, Single.Epsilon)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StsfldR4(uint value)
        {
            this.CodeSource = s_testCode.Replace("type", "float");
            bool res = (bool)Run<B_R4>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the stsfld operation for the double type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Double.MaxValue, Double.MinValue, 0.0, 1.0, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity, Double.Epsilon)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StsfldR8(ulong value)
        {
            this.CodeSource = s_testCode.Replace("type", "double");
            bool res = (bool)Run<B_R8>(@"", @"Test", @"Stsfld", value);
            Assert.IsTrue(res);
        }
    }
}
