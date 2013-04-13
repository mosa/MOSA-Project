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
	public class Mul : TestCompilerAdapter
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
							return expect == (" + constLeft + @" * x);
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
							return expect == (x * " + constRight + @");
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
		public void MulConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantCRight", (char)(a * b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void MulConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantCLeft", (char)(a * b), b));
		}

		#endregion C

		#region I1

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void MulConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI1Right", (a * b), a));
		}

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void MulConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI1Left", (a * b), b));
		}

		#endregion I1

		#region U1

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void MulConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU1Right", (uint)(a * b), a));
		}

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void MulConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU1Left", (uint)(a * b), b));
		}

		#endregion U1

		#region I2

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void MulConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI2Right", (a * b), a));
		}

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void MulConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI2Left", (a * b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 21)]

		//[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void MulConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU2Right", (uint)(a * b), a));
		}

		[Row(23, 21)]

		//[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void MulConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU2Left", (uint)(a * b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 21)]

		//[Row(-23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void MulConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI4Right", (a * b), a));
		}

		[Row(-23, 21)]

		//[Row(-23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void MulConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI4Left", (a * b), b));
		}

		#endregion I4

		#region U4

		[Row(23, 21)]

		//[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void MulConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU4Right", (uint)(a * b), a));
		}

		[Row(23, 21)]

		//[Row(23, 148)] FIXME: Uncommenting this crashes the testrunner
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void MulConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU4Left", (uint)(a * b), b));
		}

		#endregion U4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(-123, long.MaxValue)]
		[Test]
		public void MulConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI8Right", (a * b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(-123, long.MaxValue)]
		[Test]
		public void MulConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantI8Left", (a * b), b));
		}

		#endregion I8

		#region U8

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(1, ulong.MaxValue)]
		[Test]
		public void MulConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU8Right", (ulong)(a * b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(1, ulong.MaxValue)]
		[Test]
		public void MulConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantU8Left", "ulong", "ulong", a.ToString(), null);

			// left side constant
			settings.CodeSource = "static class Test { static bool MulConstantU8Left(ulong expect, ulong b) { return expect == (" + a.ToString() + " * b); } }";
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantU8Left", (ulong)(a * b), b));
		}

		#endregion U8

		#region R4

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void MulConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantR4Right", "float", "float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantR4Right", (a * b), a));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void MulConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantR4Left", "float", "float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantR4Left", (a * b), b));
		}

		#endregion R4

		#region R8

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void MulConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantR8Right", "double", "double", null, b.ToString(CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantR8Right", (a * b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void MulConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("MulConstantR8Left", "double", "double", a.ToString(CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "MulConstantR8Left", (a * b), b));
		}

		#endregion R8
	}
}