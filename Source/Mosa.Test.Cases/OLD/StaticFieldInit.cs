using System;
using System.Globalization;
using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{

	[TestFixture]
	class StaticFieldInit : TestCompilerAdapter
	{
		private static string testCode = @"
			using System;
			static class Test {
				private static #type fld = #value;
				public static bool StaticFieldInit(#type value) {
					return (fld == value);
				}
			}
		";

		private void RunTestCode<Value>(string code, Value value)
		{
			settings.CodeSource = code;
			Assert.IsTrue(Run<bool>(string.Empty, @"Test", @"StaticFieldInit", value));
		}

		[Test]
		public void StaticFieldInitB([B]bool value)
		{
			RunTestCode<bool>(testCode.Replace("#type", "bool").Replace("#value", value.ToString().ToLower()), value);
		}

		[Test]
		public void StaticFieldInitC([C]char value)
		{
			RunTestCode<char>(testCode.Replace("#type", "char").Replace("#value", "'\\u" + ((int)value).ToString("x4") + "'"), value);
		}

		[Test]
		public void StaticFieldInitI1([I1]sbyte value)
		{
			RunTestCode<sbyte>(testCode.Replace("#type", "sbyte").Replace("#value", "(sbyte)" + value.ToString()), value);
		}

		[Test]
		public void StaticFieldInitI2([I2]short value)
		{
			RunTestCode<short>(testCode.Replace("#type", "short").Replace("#value", "(short)" + value.ToString()), value);
		}

		[Test]
		public void StaticFieldInitI4([I4]int value)
		{
			RunTestCode<int>(testCode.Replace("#type", "int").Replace("#value", value.ToString()), value);
		}

		[Test]
		public void StaticFieldInitI8([I8]long value)
		{
			RunTestCode<long>(testCode.Replace("#type", "long").Replace("#value", value.ToString() + "L"), value);
		}

		[Test]
		public void StaticFieldInitU1([U1]byte value)
		{
			RunTestCode<byte>(testCode.Replace("#type", "byte").Replace("#value", "(byte)" + value.ToString() + "U"), value);
		}

		[Test]
		public void StaticFieldInitU2([U2]ushort value)
		{
			RunTestCode<ushort>(testCode.Replace("#type", "ushort").Replace("#value", "(ushort)" + value.ToString() + "U"), value);
		}

		[Test]
		public void StaticFieldInitU4([U4]uint value)
		{
			RunTestCode<uint>(testCode.Replace("#type", "uint").Replace("#value", value.ToString() + "U"), value);
		}

		[Test]
		public void StaticFieldInitU8([U8]ulong value)
		{
			RunTestCode<ulong>(testCode.Replace("#type", "ulong").Replace("#value", value.ToString() + "UL"), value);
		}

		[Test]
		public void StaticFieldInitR4([R4Number]float value)
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

		[Test]
		public void StaticFieldInitR8([R8Number]double value)
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
