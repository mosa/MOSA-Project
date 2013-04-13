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
using System.Globalization;

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Add : TestCompilerAdapter
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
							return expect == (" + constLeft + @" + x);
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
							return expect == (x + " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void AddConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantCRight", (char)(a + b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void AddConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantCLeft", (char)(a + b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void AddConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI1Right", (a + b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void AddConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI1Left", (a + b), b));
		}

		[Row(1)]
		[Test]
		public void AddConstantI1Right(sbyte a)
		{
			settings.CodeSource = "static class Test { static bool AddConstantI1Right(int expect, sbyte a) { return expect == (a + 1); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI1Right", a + 1, a));
		}

		#endregion I1

		#region U1

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void AddConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU1Right", (uint)(a + b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void AddConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU1Left", (uint)(a + b), b));
		}

		#endregion U1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void AddConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI2Right", (a + b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void AddConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI2Left", (a + b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void AddConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU2Right", (uint)(a + b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void AddConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU2Left", (uint)(a + b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void AddConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI4Right", (a + b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void AddConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI4Left", (a + b), b));
		}

		#endregion I4

		#region U4

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void AddConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU4Right", (uint)(a + b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void AddConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU4Left", (uint)(a + b), b));
		}

		#endregion U4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void AddConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI8Right", (a + b), a));
		}

		//[Row(-23, 148)]
		//[Row(17, 1)]
		[Row(0, 0)]

		//[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void AddConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantI8Left", (a + b), b));
		}

		#endregion I8

		#region U8

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void AddConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU8Right", (ulong)(a + b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void AddConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantU8Left", "ulong", "ulong", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantU8Left", (ulong)(a + b), b));
		}

		#endregion U8

		#region R4

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]

		// Obsolete. This test just fails because we're calculating with higher precision
		// [Row(float.MinValue, float.MaxValue)]
		[Test]
		public void AddConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR4Right", "float", "float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR4Right", (a + b), a));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]

		// Obsolete. This test just fails because we're calculating with higher precision
		// [Row(float.MinValue, float.MaxValue)]
		[Test]
		public void AddConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR4Left", "float", "float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR4Left", (a + b), b));
		}

		#endregion R4

		#region R8

		[Row(1.2, 2.1)]
		[Row(23.0, 21.2578)]
		[Row(1.0, -2.198)]
		[Row(-1.2, 2.11)]
		[Row(0.0, 0.0)]
		[Row(-17.1, -2.3)]

		// (MinValue, X) Cases
		[Row(double.MinValue, 0.0)]
		[Row(double.MinValue, 1.2)]
		[Row(double.MinValue, 17.6)]
		[Row(double.MinValue, 123.1)]
		[Row(double.MinValue, -0.0)]
		[Row(double.MinValue, -1.5)]
		[Row(double.MinValue, -17.99)]
		[Row(double.MinValue, -123.235)]

		// (MaxValue, X) Cases
		[Row(double.MaxValue, 0.0)]
		[Row(double.MaxValue, 1.67)]
		[Row(double.MaxValue, 17.875)]
		[Row(double.MaxValue, 123.283)]
		[Row(double.MaxValue, -0.0)]
		[Row(double.MaxValue, -1.1497)]
		[Row(double.MaxValue, -17.12)]
		[Row(double.MaxValue, -123.34)]

		// (X, MinValue) Cases
		[Row(0.0, double.MinValue)]
		[Row(1.2, double.MinValue)]
		[Row(17.4, double.MinValue)]
		[Row(123.561, double.MinValue)]
		[Row(-0.0, double.MinValue)]
		[Row(-1.78, double.MinValue)]
		[Row(-17.59, double.MinValue)]
		[Row(-123.41, double.MinValue)]

		// (X, MaxValue) Cases
		[Row(0.0, double.MaxValue)]
		[Row(1.00012, double.MaxValue)]
		[Row(17.094002, double.MaxValue)]
		[Row(123.001, double.MaxValue)]
		[Row(-0.0, double.MaxValue)]
		[Row(-1.045, double.MaxValue)]
		[Row(-17.0002501, double.MaxValue)]
		[Row(-123.023, double.MaxValue)]

		// Extremvaluecases
		[Row(double.MinValue, double.MaxValue)]
		[Row(1.0f, double.NaN)]
		[Row(double.NaN, 1.0f)]
		[Row(1.0f, double.PositiveInfinity)]
		[Row(double.PositiveInfinity, 1.0f)]
		[Row(1.0f, double.NegativeInfinity)]
		[Row(double.NegativeInfinity, 1.0f)]
		[Test]
		public void AddR8(double a, double b)
		{
			settings.CodeSource = "static class Test { static bool AddR8(double expect, double a, double b) { return expect == (a + b); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddR8", (a + b), a, b));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void AddConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR8Right", "double", "double", null, b.ToString(CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR8Right", (a + b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void AddConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR8Left", "double", "double", a.ToString(CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR8Left", (a + b), b));
		}

		#endregion R8
	}
}