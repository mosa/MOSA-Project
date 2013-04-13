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
	public class Sub : TestCompilerAdapter
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
							return expect == ((" + constLeft + @") - x);
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
							return expect == (x - (" + constRight + @"));
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
		public void SubConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantCRight", (a - b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void SubConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantCLeft", (a - b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(0, 10)]
		[Row(0, -10)]
		[Row(10, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void SubConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI1Right", (a - b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void SubConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI1Left", (a - b), b));
		}

		#endregion I1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void SubConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI2Right", (a - b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void SubConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI2Left", (a - b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void SubConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantU2Right", "ushort", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantU2Right", (a - b), a));
		}

		////[Row(23, 148)]
		////[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void SubConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantU2Left", "ushort", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantU2Left", (a - b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void SubConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI4Right", (a - b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void SubConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI4Left", (a - b), b));
		}

		#endregion I4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void SubConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI8Right", (a - b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void SubConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantI8Left", (a - b), b));
		}

		#endregion I8

		#region R4

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		[Test]
		public void SubConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR4Right", "float", "float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR4Right", (a - b), a));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]

		// Obsolete, because of higher precision
		// [Row(-17.0002501f, float.MaxValue)]
		[Test]
		public void SubConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR4Left", "float", "float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR4Left", (a - b), b));
		}

		#endregion R4

		#region R8

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void SubConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR8Right", "double", "double", null, b.ToString(CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR8Right", (a - b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void SubConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR8Left", "double", "double", a.ToString(CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR8Left", (a - b), b));
		}

		#endregion R8
	}
}