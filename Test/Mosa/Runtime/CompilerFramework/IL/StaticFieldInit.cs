using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Globalization;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Provides test cases for the cpblk IL operation.
    /// </summary>
    [TestFixture]
    class StaticFieldInit : CodeDomTestRunner
    {
        private static string s_testCode = @"
            using System;
            static class Test {
                private static type s_fld = s_value;
                public static bool StaticFieldInit(type value) {
                    return (s_fld == value);
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

        private void RunTestCode<DelegateType, Value>(string code, Value value)
        {
            this.CodeSource = code;
            bool res = (bool)Run<DelegateType>(@"", @"Test", @"StaticFieldInit", value);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the boolean type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Row(true)]
        //[Row(false)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitB(bool value)
        {
            RunTestCode<B_B, bool>(s_testCode.Replace("type", "bool").Replace("s_value", value.ToString().ToLower()), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the char type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Row(Char.MaxValue)]
        [Row('a')]
        [Row('z')]
        [Row('0')]
        [Row('9')]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitC(char value)
        {
            RunTestCode<B_C, char>(s_testCode.Replace("type", "char").Replace("s_value", "'" + value + "'"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the sbyte type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(SByte.MaxValue, SByte.MinValue, (sbyte)0, (sbyte)1, (sbyte)-1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitI1(sbyte value)
        {
            RunTestCode<B_I1, sbyte>(s_testCode.Replace("type", "sbyte").Replace("s_value", "(sbyte)" + value.ToString()), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the short type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Int16.MaxValue, Int16.MinValue, (short)0, (short)1, (short)-1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitI2(short value)
        {
            RunTestCode<B_I2, short>(s_testCode.Replace("type", "short").Replace("s_value", "(short)" + value.ToString()), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the int type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Int32.MaxValue, Int32.MinValue, 0, 1, -1)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitI4(int value)
        {
            RunTestCode<B_I4, int>(s_testCode.Replace("type", "int").Replace("s_value", value.ToString()), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the long type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Int64.MaxValue/*, Int64.MinValue, 0L, 1L, -1L*/)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitI8(long value)
        {
            RunTestCode<B_I8, long>(s_testCode.Replace("type", "long").Replace("s_value", value.ToString() + "L"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the byte type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Byte.MaxValue, Byte.MinValue, (byte)0U, (byte)1U, (byte)0xFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitU1(byte value)
        {
            RunTestCode<B_U1, byte>(s_testCode.Replace("type", "byte").Replace("s_value", "(byte)" + value.ToString() + "U"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the ushort type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(UInt16.MaxValue, UInt16.MinValue, (ushort)0U, (ushort)1U, (ushort)0xFFFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitU2(ushort value)
        {
            RunTestCode<B_U2, ushort>(s_testCode.Replace("type", "ushort").Replace("s_value", "(ushort)" + value.ToString() + "U"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the uint type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(UInt32.MaxValue, UInt32.MinValue, 0U, 1U, 0xFFFFFFFFU)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitU4(uint value)
        {
            RunTestCode<B_U4, uint>(s_testCode.Replace("type", "uint").Replace("s_value", value.ToString() + "U"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the ulong type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(UInt64.MaxValue, UInt64.MinValue, 0UL, 1UL, 0xFFFFFFFFFFFFFFFFUL)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitU8(ulong value)
        {
            RunTestCode<B_U8, ulong>(s_testCode.Replace("type", "ulong").Replace("s_value", value.ToString() + "UL"), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the float type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(/*Single.MaxValue, Single.MinValue, 0.0f, 1.0f,*/ Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, Single.Epsilon)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitR4(float value)
        {
            string sValue = value.ToString("R", CultureInfo.InvariantCulture) + "f";
            if (Single.IsNaN(value) == true)
                sValue = "Single.NaN";
            else if (value == Single.PositiveInfinity)
                sValue = "Single.PositiveInfinity";
            else if (value == Single.NegativeInfinity)
                sValue = "Single.NegativeInfinity";

            RunTestCode<B_R4, float>(s_testCode.Replace("type", "float").Replace("s_value", sValue), value);
        }

        /// <summary>
        /// Tests the StaticFieldInit for the double type.
        /// </summary>
        /// <param name="value">The value to store in the static field</param>
        [Column(Double.MaxValue, Double.MinValue, 0.0, 1.0, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity, Double.Epsilon)]
        [Test]
        [Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
        [Importance(Importance.Severe)]
        public void StaticFieldInitR8(double value)
        {
            string sValue = value.ToString("R", CultureInfo.InvariantCulture);
            if (Double.IsNaN(value) == true)
                sValue = "Double.NaN";
            else if (value == Double.PositiveInfinity)
                sValue = "Double.PositiveInfinity";
            else if (value == Double.NegativeInfinity)
                sValue = "Double.NegativeInfinity";

            RunTestCode<B_R8, double>(s_testCode.Replace("type", "double").Replace("s_value", sValue), value);
        }
    }
}
