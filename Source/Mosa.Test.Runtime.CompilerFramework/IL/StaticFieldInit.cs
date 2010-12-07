using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{

	[TestFixture]
	class StaticFieldInit : CodeDomTestRunner
	{
		private static string testCode = @"
			using System;
			static class Test {
				private static #type fld = #value;
				public static bool StaticFieldInit(#type value) {
					return (fld == value);
				}
			}
		" + Code.AllTestCode;

		private void RunTestCode<Value>(string code, Value value)
		{
			CodeSource = code;
			bool res = Run<bool>(@"", @"Test", @"StaticFieldInit", value);
			Assert.IsTrue(res);
		}

		[Row(true)]
		//[Row(false)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitB(bool value)
		{
			RunTestCode<bool>(testCode.Replace("#type", "bool").Replace("#value", value.ToString().ToLower()), value);
		}

		[Row(Char.MinValue)]
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
			RunTestCode<char>(testCode.Replace("#type", "char").Replace("#value", "'\\u" + ((int)value).ToString("x4") + "'"), value);
		}

		[Column(SByte.MaxValue, SByte.MinValue, (sbyte)0, (sbyte)1, (sbyte)-1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitI1(sbyte value)
		{
			RunTestCode<sbyte>(testCode.Replace("#type", "sbyte").Replace("#value", "(sbyte)" + value.ToString()), value);
		}

		[Column(Int16.MaxValue, Int16.MinValue, (short)0, (short)1, (short)-1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitI2(short value)
		{
			RunTestCode<short>(testCode.Replace("#type", "short").Replace("#value", "(short)" + value.ToString()), value);
		}

		[Column(Int32.MaxValue, Int32.MinValue, 0, 1, -1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitI4(int value)
		{
			RunTestCode<int>(testCode.Replace("#type", "int").Replace("#value", value.ToString()), value);
		}

		[Column(Int64.MaxValue/*, Int64.MinValue, 0L, 1L, -1L*/)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitI8(long value)
		{
			RunTestCode<long>(testCode.Replace("#type", "long").Replace("#value", value.ToString() + "L"), value);
		}

		[Column(Byte.MaxValue, Byte.MinValue, (byte)0U, (byte)1U, (byte)0xFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitU1(byte value)
		{
			RunTestCode<byte>(testCode.Replace("#type", "byte").Replace("#value", "(byte)" + value.ToString() + "U"), value);
		}

		[Column(UInt16.MaxValue, UInt16.MinValue, (ushort)0U, (ushort)1U, (ushort)0xFFFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitU2(ushort value)
		{
			RunTestCode<ushort>(testCode.Replace("#type", "ushort").Replace("#value", "(ushort)" + value.ToString() + "U"), value);
		}

		[Column(UInt32.MaxValue, UInt32.MinValue, 0U, 1U, 0xFFFFFFFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitU4(uint value)
		{
			RunTestCode<uint>(testCode.Replace("#type", "uint").Replace("#value", value.ToString() + "U"), value);
		}

		[Column(UInt64.MaxValue, UInt64.MinValue, 0UL, 1UL, 0xFFFFFFFFFFFFFFFFUL)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitU8(ulong value)
		{
			RunTestCode<ulong>(testCode.Replace("#type", "ulong").Replace("#value", value.ToString() + "UL"), value);
		}

		[Column(/*Single.MaxValue, Single.MinValue, 0.0f, 1.0f,*/ Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, Single.Epsilon)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitR4(float value)
		{
			string sValue = value.ToString("R", CultureInfo.InvariantCulture) + "f";
			if (Single.IsNaN(value))
				sValue = "Single.NaN";
			else if (value == Single.PositiveInfinity)
				sValue = "Single.PositiveInfinity";
			else if (value == Single.NegativeInfinity)
				sValue = "Single.NegativeInfinity";

			RunTestCode<float>(testCode.Replace("#type", "float").Replace("#value", sValue), value);
		}

		[Column(Double.MaxValue, Double.MinValue, 0.0, 1.0, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity, Double.Epsilon)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StaticFieldInitR8(double value)
		{
			string sValue = value.ToString("R", CultureInfo.InvariantCulture);
			if (Double.IsNaN(value))
				sValue = "Double.NaN";
			else if (value == Double.PositiveInfinity)
				sValue = "Double.PositiveInfinity";
			else if (value == Double.NegativeInfinity)
				sValue = "Double.NegativeInfinity";

			RunTestCode<double>(testCode.Replace("#type", "double").Replace("#value", sValue), value);
		}
	}
}
