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

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.OLD.IL
{
	[TestFixture]
	public class Ldloca : TestCompilerAdapter
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
			return TestCode.Replace("#name", name).Replace("#type", type);
		}

		#region CheckValue

		[Column(0, 1, SByte.MinValue, SByte.MaxValue, SByte.MinValue + 1, SByte.MaxValue - 1)]
		[Test]
		public void LdlocaI1_CheckValue(sbyte value)
		{
			settings.CodeSource = CreateTestCode("LdlocaI1_CheckValue", "sbyte");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaI1_CheckValue", value));
		}

		[Column(0, 1, Int16.MinValue, Int16.MaxValue, Int16.MinValue + 1, Int16.MaxValue - 1)]
		[Test]
		public void LdlocaI2_CheckValue(short value)
		{
			settings.CodeSource = CreateTestCode("LdlocaI2_CheckValue", "short");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaI2_CheckValue", value));
		}

		[Column(0, 1, Int32.MinValue, Int32.MaxValue, Int32.MinValue + 1, Int32.MaxValue - 1)]
		[Test]
		public void LdlocaI4_CheckValue(int value)
		{
			settings.CodeSource = CreateTestCode("LdlocaI4_CheckValue", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaI4_CheckValue", value));
		}

		[Column(0, 1, Int64.MinValue, Int64.MaxValue, Int64.MinValue + 1, Int64.MaxValue - 1)]
		[Test]
		public void LdlocaI8_CheckValue(long value)
		{
			settings.CodeSource = CreateTestCode("LdlocaI8_CheckValue", "long");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaI8_CheckValue", value));
		}

		[Column(0, 1, Byte.MinValue, Byte.MaxValue, Byte.MinValue + 1, Byte.MaxValue - 1)]
		[Test]
		public void LdlocaU1_CheckValue(byte value)
		{
			settings.CodeSource = CreateTestCode("LdlocaU1_CheckValue", "byte");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaU1_CheckValue", value));
		}

		[Column(0, 1, UInt16.MinValue, UInt16.MaxValue, UInt16.MinValue + 1, UInt16.MaxValue - 1)]
		[Test]
		public void LdlocaU2_CheckValue(ushort value)
		{
			settings.CodeSource = CreateTestCode("LdlocaU2_CheckValue", "ushort");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaU2_CheckValue", value));
		}

		[Column(0, 1, UInt32.MinValue, UInt32.MaxValue, UInt32.MinValue + 1, UInt32.MaxValue - 1)]
		[Test]
		public void LdlocaU4_CheckValue(uint value)
		{
			settings.CodeSource = CreateTestCode("LdlocaU4_CheckValue", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaU4_CheckValue", value));
		}

		[Column(0, 1, UInt64.MinValue, UInt64.MaxValue, UInt64.MinValue + 1, UInt64.MaxValue - 1)]
		[Test]
		public void LdlocaU8_CheckValue(ulong value)
		{
			settings.CodeSource = CreateTestCode("LdlocaU8_CheckValue", "ulong");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaU8_CheckValue", value));
		}

		[Column(0, 1, Single.MinValue, Single.MaxValue, Single.MinValue + 1, Single.MaxValue - 1)]
		[Test]
		public void LdlocaR4_CheckValue(float value)
		{
			settings.CodeSource = CreateTestCode("LdlocaR4_CheckValue", "float");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaR4_CheckValue", value));
		}

		[Column(0, 1, Double.MinValue, Double.MaxValue, Double.MinValue + 1, Double.MaxValue - 1)]
		[Test]
		public void LdlocaR8_CheckValue(double value)
		{
			settings.CodeSource = CreateTestCode("LdlocaR8_CheckValue", "double");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "LdlocaR8_CheckValue", value));
		}

		#endregion // CheckValue
	}
}
