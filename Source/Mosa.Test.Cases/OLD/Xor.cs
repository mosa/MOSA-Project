/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *
 */

using System;

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Xor : TestCompilerAdapter
	{
		private static string CreateConstantTestCode(string name, string typeIn, string typeOut, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return expect == (" + constLeft + @" ^ x);
						}
					}";
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return expect == (x ^ " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region B

		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test]
		public void XorConstantBRight(bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantBRight", "bool", "bool", null, b.ToString().ToLower());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantBRight", (a ^ b), a));
		}

		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test]
		public void XorConstantBLeft(bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantBLeft", (a ^ b), b));
		}

		#endregion B

		#region C

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void XorConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantCRight", (char)(a ^ b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void XorConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantCLeft", (char)(a ^ b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void XorConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI1Right", (a ^ b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void XorConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI1Left", (a ^ b), b));
		}

		#endregion I1

		#region U1

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void XorConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU1Right", (uint)(a ^ b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void XorConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU1Left", (uint)(a ^ b), b));
		}

		#endregion U1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void XorConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI2Right", (a ^ b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void XorConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI2Left", (a ^ b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void XorConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU2Right", (uint)(a ^ b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void XorConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU2Left", (uint)(a ^ b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void XorConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI4Right", (a ^ b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void XorConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI4Left", (a ^ b), b));
		}

		#endregion I4

		#region U4

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void XorConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU4Right", (uint)(a ^ b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void XorConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU4Left", (uint)(a ^ b), b));
		}

		#endregion U4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void XorConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI8Right", (a ^ b), a));
		}

		//[Row(-23, 148)]
		//[Row(17, 1)]
		//[Row(0, 0)]
		//[Row(long.MinValue, long.MaxValue)]
		[Row(4294977296, 42)] // Constant > int.Maxvalue but < long.Maxvalue
		[Test]
		public void XorConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantI8Left", (a ^ b), b));
		}

		#endregion I8

		#region U8

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void XorConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU8Right", (ulong)(a ^ b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void XorConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("XorConstantU8Left", "ulong", "ulong", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "XorConstantU8Left", (ulong)(a ^ b), b));
		}

		#endregion U8
	}
}