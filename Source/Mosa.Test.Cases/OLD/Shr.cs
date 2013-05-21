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
using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Shr : TestCompilerAdapter
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
							return expect == (" + constLeft + @" >> x);
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
							return expect == (x >> " + constRight + @");
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
		public void ShrConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantCRight", (char)(a >> b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void ShrConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantCLeft", (char)(a >> b), b));
		}

		#endregion C

		#region I1

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShrConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI1Right", (a >> b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShrConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI1Left", (a >> b), b));
		}

		#endregion I1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShrConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI2Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShrConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI2Left", (a >> b), b));
		}

		#endregion I2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI4Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI4Left", (a >> b), b));
		}

		#endregion I4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI8Right(long a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI8Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI8Left(long a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("ShrConstantI8Left", "int", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "ShrConstantI8Left", (a >> b), b));
		}

		#endregion I8
	}
}