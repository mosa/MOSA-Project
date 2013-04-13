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
	public class Shl : TestCompilerAdapter
	{
		private static string CreateTestCodeWithReturn(string name, string typeInA, string typeInB, string typeOut)
		{
			return @"
				static class Test
				{
					static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
					{
						return (a << b);
					}
				}";
		}

		private static string CreateConstantTestCode(string name, string typeIn, string typeOut, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return expect == (" + constLeft + @" << x);
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
							return expect == (x << " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private static string CreateConstantTestCodeWithReturn(string name, string typeIn, string typeOut, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return (" + constLeft + @" << x);
						}
					}";
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return (x << " + constRight + @");
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
		public void ShlConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("ShlConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
			Assert.AreEqual(a << b, Run<int>(string.Empty, "Test", "ShlConstantCRight", (char)(a << b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void ShlConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("ShlConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
			Assert.AreEqual(a << b, Run<int>(string.Empty, "Test", "ShlConstantCLeft", (char)(a << b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShlConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI1Right", (a << b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShlConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI1Left", (a << b), b));
		}

		#endregion I1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShlConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI2Right", (a << b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShlConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI2Left", (a << b), b));
		}

		#endregion I2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShlConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI4Right", (a << b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShlConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI4Left", (a << b), b));
		}

		#endregion I4

		#region I8

		[Row(1, 1)]
		[Row(1, 0)]
		[Row(0, 1)]
		[Row(unchecked((long)0x8000000000000000), 64)]
		[Test]
		public void ShlI8(long a, int b)
		{
			settings.CodeSource = CreateTestCodeWithReturn("ShlI8", "long", "int", "long");
			Assert.AreEqual((a << b), Run<long>(string.Empty, "Test", "ShlI8", (a << b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(0, 1)]
		[Test]
		public void ShlConstantI8Right(long a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI8Right", (a << b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(0, 1)]
		[Test]
		public void ShlConstantI8Left(long a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShlConstantI8Left", "int", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShlConstantI8Left", (a << b), b));
		}

		#endregion I8
	}
}