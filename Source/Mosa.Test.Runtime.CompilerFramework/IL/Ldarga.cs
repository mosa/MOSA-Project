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

namespace Mosa.Test.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class Ldarga : TestCompilerAdapter
	{
		private static string TestCodeCheckValue = @"
			static class Test
			{ 
				static bool #name(#type expect, #type a) 
				{
					return CheckValue(expect, ref a);
				}

				static bool CheckValue(#type expect, ref #type a)
				{
					return expect == a;
				}
			}";

		private static string CreateTestCodeCheckValue(string name, string type)
		{
			return TestCodeCheckValue
				.Replace("#name", name)
				.Replace("#type", type)
				+ Code.AllTestCode;
		}

		private static string TestCodeChangeValue = @"
			static class Test
			{ 
				static bool #name(#type expect, #type a) 
				{
					ChangeValue(expect, ref a);
					return expect == a;
				}

				static void ChangeValue(#type expect, ref #type a)
				{
					a = expect;
				}
			}";

		private static string CreateTestCodeChangeValue(string name, string type)
		{
			return TestCodeChangeValue
				.Replace("#name", name)
				.Replace("#type", type)
				+ Code.AllTestCode;
		}

		#region CheckValue

		[Test]
		public void LdargaI1_CheckValue([Column(0, 1, sbyte.MinValue, sbyte.MaxValue)] sbyte a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaI1_CheckValue", "sbyte");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI1_CheckValue", a, a));
		}

		[Test]
		public void LdargaU1_CheckValue([Column(0, 1, byte.MinValue, byte.MaxValue)] byte a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaU1_CheckValue", "byte");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaU1_CheckValue", a, a));
		}

		[Row(0)]
		[Row(1)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test]
		public void LdargaI2_CheckValue(short a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaI2_CheckValue", "short");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI2_CheckValue", a, a));
		}

		[Column(0, 1, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void LdargaU2_CheckValue(ushort a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaU2_CheckValue", "ushort");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaU2_CheckValue", a, a));
		}

		[Column(0, 1, int.MinValue, int.MaxValue)]
		[Test]
		public void LdargaI4_CheckValue(int a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaI4_CheckValue", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI4_CheckValue", a, a));
		}

		[Column(0, 1, uint.MinValue, uint.MaxValue)]
		[Test]
		public void LdargaU4_CheckValue(uint a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaU4_CheckValue", "uint");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaU4_CheckValue", a, a));
		}

		[Column(0, 1, long.MinValue, long.MaxValue)]
		[Test]
		public void LdargaI8_CheckValue(long a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaI8_CheckValue", "long");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI8_CheckValue", a, a));
		}

		[Column(0, 1, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void LdargaU8_CheckValue(ulong a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaU8_CheckValue", "ulong");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaU8_CheckValue", a, a));
		}

		[Column(0, 1, float.MinValue, float.MaxValue)]
		[Test]
		public void LdargaR4_CheckValue(float a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaR4_CheckValue", "float");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaR4_CheckValue", a, a));
		}

		[Column(0, 1, double.MinValue, double.MaxValue)]
		[Test]
		public void LdargaR8_CheckValue(double a)
		{
			compiler.CodeSource = CreateTestCodeCheckValue("LdargaR8_CheckValue", "double");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaR8_CheckValue", a, a));
		}

		#endregion

		#region ChangeValue

		[Row(1, 0), Row(0, 1), Row(1, sbyte.MinValue), Row(0, sbyte.MaxValue)]
		[Test]
		public void LdargaI1_ChangeValue(sbyte newValue, sbyte oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaI1_ChangeValue", "sbyte");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI1_ChangeValue", newValue, oldValue));
		}

		[Row(1, 0), Row(0, 1), Row(1, short.MinValue), Row(0, short.MaxValue)]
		[Test]
		public void LdargaI2_ChangeValue(short newValue, short oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaI2_ChangeValue", "short");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI2_ChangeValue", newValue, oldValue));
		}

		[Row(1, 0), Row(0, 1), Row(1, int.MinValue), Row(0, int.MaxValue)]
		[Test]
		public void LdargaI4_ChangeValue(int newValue, int oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaI4_ChangeValue", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI4_ChangeValue", newValue, oldValue));
		}

		[Row(1, 0), Row(0, 1), Row(1, long.MinValue), Row(0, long.MaxValue)]
		[Test]
		public void LdargaI8_ChangeValue(long newValue, long oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaI8_ChangeValue", "long");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaI8_ChangeValue", newValue, oldValue));
		}

		[Row(1, 0), Row(0, 1), Row(1, float.MinValue), Row(0, float.MaxValue)]
		[Test]
		public void LdargaR4_ChangeValue(float newValue, float oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaR4_ChangeValue", "float");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaR4_ChangeValue", newValue, oldValue));
		}

		[Row(1, 0), Row(0, 1), Row(1, double.MinValue), Row(0, double.MaxValue)]
		[Test]
		public void LdargaR8_ChangeValue(double newValue, double oldValue)
		{
			compiler.CodeSource = CreateTestCodeChangeValue("LdargaR8_ChangeValue", "double");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "LdargaR8_ChangeValue", newValue, oldValue));
		}

		#endregion
	}
}
