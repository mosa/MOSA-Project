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
	public class OrFixture : TestCompilerAdapter
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
							return expect == (" + constLeft + @" | x);
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
							return expect == (x | " + constRight + @");
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
		public void OrConstantBLeft(bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantBLeft", (a | b), b));
		}

		#endregion B

		#region C

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void OrConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantCRight", (char)(a | b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void OrConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantCLeft", (char)(a | b), b));
		}

		#endregion C

		#region I1

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0)]
		[Row(-17, -2)]

		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]

		// (MinValue, X) Cases
		[Row(sbyte.MinValue, 0)]
		[Row(sbyte.MinValue, 1)]
		[Row(sbyte.MinValue, 17)]
		[Row(sbyte.MinValue, 123)]
		[Row(sbyte.MinValue, -0)]
		[Row(sbyte.MinValue, -1)]
		[Row(sbyte.MinValue, -17)]
		[Row(sbyte.MinValue, -123)]

		// (MaxValueee, X) Cases
		[Row(sbyte.MaxValue, 0)]
		[Row(sbyte.MaxValue, 1)]
		[Row(sbyte.MaxValue, 17)]
		[Row(sbyte.MaxValue, 123)]
		[Row(sbyte.MaxValue, -0)]
		[Row(sbyte.MaxValue, -1)]
		[Row(sbyte.MaxValue, -17)]
		[Row(sbyte.MaxValue, -123)]

		// (X, MinValue) Cases
		[Row(0, sbyte.MinValue)]
		[Row(1, sbyte.MinValue)]
		[Row(17, sbyte.MinValue)]
		[Row(123, sbyte.MinValue)]
		[Row(-0, sbyte.MinValue)]
		[Row(-1, sbyte.MinValue)]
		[Row(-17, sbyte.MinValue)]
		[Row(-123, sbyte.MinValue)]

		// (X, MaxValue) Cases
		[Row(0, sbyte.MaxValue)]
		[Row(1, sbyte.MaxValue)]
		[Row(17, sbyte.MaxValue)]
		[Row(123, sbyte.MaxValue)]
		[Row(-0, sbyte.MaxValue)]
		[Row(-1, sbyte.MaxValue)]
		[Row(-17, sbyte.MaxValue)]
		[Row(-123, sbyte.MaxValue)]

		// Extremvaluecases
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Row(sbyte.MaxValue, sbyte.MinValue)]

		//[Test]
		public void OrI1(sbyte a, sbyte b)
		{
			//settings.CodeSource = CreateTestCode("OrI1", "sbyte", "int");
			//Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrI1", a | b, a, b));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]

		//[Test]
		public void OrConstantI1Right(sbyte a, sbyte b)
		{
			//settings.CodeSource = CreateConstantTestCode("OrConstantI1Right", "sbyte", "int", null, b.ToString());
			//Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI1Right", (a | b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]

		//[Test]
		public void OrConstantI1Left(sbyte a, sbyte b)
		{
			//settings.CodeSource = CreateConstantTestCode("OrConstantI1Left", "sbyte", "int", a.ToString(), null);
			//Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI1Left", (a | b), b));
		}

		#endregion I1

		#region U1

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void OrConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU1Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void OrConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU1Left", (uint)(a | b), b));
		}

		#endregion U1

		#region I2

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void OrConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI2Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void OrConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI2Left", (a | b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void OrConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU2Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void OrConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU2Left", (uint)(a | b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void OrConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI4Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void OrConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI4Left", (a | b), b));
		}

		#endregion I4

		#region U4

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void OrConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU4Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void OrConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU4Left", (uint)(a | b), b));
		}

		#endregion U4

		#region I8

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void OrConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI8Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void OrConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantI8Left", (a | b), b));
		}

		#endregion I8

		#region U8

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void OrConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU8Right", (ulong)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void OrConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("OrConstantU8Left", "ulong", "ulong", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "OrConstantU8Left", (ulong)(a | b), b));
		}

		#endregion U8
	}
}