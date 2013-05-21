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
	public class AndFixture : TestCompilerAdapter
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
							return expect == (" + constLeft + @" & x);
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
							return expect == (x & " + constRight + @");
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
		public void AndConstantBRight(bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantBRight", "bool", "bool", null, b.ToString().ToLower());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantBRight", (a & b), a));
		}

		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test]
		public void AndConstantBLeft(bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantBLeft", (a & b), b));
		}

		#endregion B

		#region C

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void AndConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantCRight", (char)(a & b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void AndConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantCLeft", (char)(a & b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void AndConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI1Right", (a & b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void AndConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI1Left", (a & b), b));
		}

		#endregion I1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void AndConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI2Right", (a & b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void AndConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI2Left", (a & b), b));
		}

		#endregion I2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void AndConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI4Right", (a & b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void AndConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI4Left", (a & b), b));
		}

		#endregion I4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void AndConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI8Right", (a & b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void AndConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("AndConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AndConstantI8Left", (a & b), b));
		}

		#endregion I8
	}
}