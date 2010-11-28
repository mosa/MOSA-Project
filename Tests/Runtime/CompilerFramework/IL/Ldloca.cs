/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class Ldloca : CodeDomTestRunner
	{

		private static string TestCode = @"
			static class Test
			{
				static bool #name(#type expect)
				{
					#type a = expect;
					return DoCheckValue(expect, ref a);
				}

				static bool DoCheckValue(#type expect, ref #type value)
				{
					return (expect == value);
				}
			}";

		private static string CreateTestCode(string name, string type)
		{
			return TestCode.Replace("#name", name).Replace("#type", type) + Code.AllTestCode;
		}

		#region CheckValue

		[Column(0, 1, SByte.MinValue, SByte.MaxValue, SByte.MinValue + 1, SByte.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaI1_CheckValue(sbyte value)
		{
			CodeSource = CreateTestCode("LdlocaI1_CheckValue", "sbyte");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaI1_CheckValue", value));
		}

		[Column(0, 1, Int16.MinValue, Int16.MaxValue, Int16.MinValue + 1, Int16.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaI2_CheckValue(short value)
		{
			CodeSource = CreateTestCode("LdlocaI2_CheckValue", "short");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaI2_CheckValue", value));
		}

		[Column(0, 1, Int32.MinValue, Int32.MaxValue, Int32.MinValue + 1, Int32.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaI4_CheckValue(int value)
		{
			CodeSource = CreateTestCode("LdlocaI4_CheckValue", "sbintyte");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaI4_CheckValue", value));
		}

		[Column(0, 1, Int64.MinValue, Int64.MaxValue, Int64.MinValue + 1, Int64.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaI8_CheckValue(long value)
		{
			CodeSource = CreateTestCode("LdlocaI8_CheckValue", "long");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaI8_CheckValue", value));
		}

		[Column(0, 1, Byte.MinValue, Byte.MaxValue, Byte.MinValue + 1, Byte.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaU1_CheckValue(byte value)
		{
			CodeSource = CreateTestCode("LdlocaU1_CheckValue", "byte");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaU1_CheckValue", value));
		}

		[Column(0, 1, UInt16.MinValue, UInt16.MaxValue, UInt16.MinValue + 1, UInt16.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaU2_CheckValue(ushort value)
		{
			CodeSource = CreateTestCode("LdlocaU2_CheckValue", "ushort");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaU2_CheckValue", value));
		}

		[Column(0, 1, UInt32.MinValue, UInt32.MaxValue, UInt32.MinValue + 1, UInt32.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaU4_CheckValue(uint value)
		{
			CodeSource = CreateTestCode("LdlocaU4_CheckValue", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaU4_CheckValue", value));
		}

		[Column(0, 1, UInt64.MinValue, UInt64.MaxValue, UInt64.MinValue + 1, UInt64.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaU8_CheckValue(ulong value)
		{
			CodeSource = CreateTestCode("LdlocaU8_CheckValue", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaU8_CheckValue", value));
		}

		[Column(0, 1, Single.MinValue, Single.MaxValue, Single.MinValue + 1, Single.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaR4_CheckValue(float value)
		{
			CodeSource = CreateTestCode("LdlocaR4_CheckValue", "float");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaR4_CheckValue", value));
		}

		[Column(0, 1, Double.MinValue, Double.MaxValue, Double.MinValue + 1, Double.MaxValue - 1)]
		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void LdlocaR8_CheckValue(double value)
		{
			CodeSource = CreateTestCode("LdlocaR8_CheckValue", "double");
			Assert.IsTrue(Run<bool>("", "Test", "LdlocaR8_CheckValue", value));
		}

		#endregion // CheckValue
	}
}
