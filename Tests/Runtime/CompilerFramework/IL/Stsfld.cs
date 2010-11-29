using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{

	[TestFixture]
	public class Stsfld : CodeDomTestRunner
	{
		private static string testCode = @"
			static class Test {
				private static #type fld;
				public static bool Stsfld(#type value) {
					fld = value;
					return (value == fld);
				}
			}
		" + Code.AllTestCode;

		[Row(true)]
		[Row(false)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldB(bool value)
		{
			CodeSource = testCode.Replace("#type", "bool");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
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
		public void StsfldC(char value)
		{
			CodeSource = testCode.Replace("#type", "char");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(SByte.MaxValue, SByte.MinValue, (sbyte)0, (sbyte)1, (sbyte)-1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldI1(sbyte value)
		{
			CodeSource = testCode.Replace("#type", "sbyte");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Int16.MaxValue, Int16.MinValue, (short)0, (short)1, (short)-1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldI2(short value)
		{
			CodeSource = testCode.Replace("#type", "short");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Int32.MaxValue, Int32.MinValue, 0, 1, -1)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldI4(int value)
		{
			CodeSource = testCode.Replace("#type", "int");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Int64.MaxValue, Int64.MinValue, 0L, 1L, -1L)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldI8(long value)
		{
			CodeSource = testCode.Replace("#type", "long");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Byte.MaxValue, Byte.MinValue, (byte)0U, (byte)1U, (byte)0xFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldU1(byte value)
		{
			CodeSource = testCode.Replace("#type", "byte");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

	
		[Column(UInt16.MaxValue, UInt16.MinValue, (ushort)0U, (ushort)1U, (ushort)0xFFFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldU2(ushort value)
		{
			CodeSource = testCode.Replace("#type", "ushort");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(UInt32.MaxValue, UInt32.MinValue, 0U, 1U, 0xFFFFFFFFU)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldU4(uint value)
		{
			CodeSource = testCode.Replace("#type", "uint");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(UInt64.MaxValue, UInt64.MinValue, 0UL, 1UL, 0xFFFFFFFFFFFFFFFFUL)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldU8(ulong value)
		{
			CodeSource = testCode.Replace("#type", "ulong");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Single.MaxValue, Single.MinValue, 0.0f, 1.0f, Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, Single.Epsilon)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldR4(float value)
		{
			CodeSource = testCode.Replace("#type", "float");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}

		[Column(Double.MaxValue, Double.MinValue, 0.0, 1.0, Double.NaN, Double.NegativeInfinity, Double.PositiveInfinity, Double.Epsilon)]
		[Test]
		[Author(@"Michael Ruck", @"sharpos@michaelruck.de")]
		[Importance(Importance.Severe)]
		public void StsfldR8(double value)
		{
			CodeSource = testCode.Replace("#type", "double");
			bool res = Run<bool>(@"", @"Test", @"Stsfld", value);
			Assert.IsTrue(res);
		}
	}
}
