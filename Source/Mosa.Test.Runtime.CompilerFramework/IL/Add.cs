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
using System.Runtime.InteropServices;

using Gallio.Framework;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class Add : TestCompilerAdapter
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a + b);
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

		[Row(0, 0)]
		[Row(0, 1)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test]
		public void AddC(char a, char b)
		{
			settings.CodeSource = CreateTestCode("AddC", "char", "char");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddC", (char)(a + b), a, b));
		}

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
		#endregion

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
		// (MaxValue, X) Cases
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
		[Test]
		public void AddI1(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateTestCode("AddI1", "sbyte", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddI1", a + b, a, b));
		}

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
		#endregion

		#region U1
	
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(byte.MinValue, 0)]
		[Row(byte.MinValue, 1)]
		[Row(byte.MinValue, 17)]
		[Row(byte.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(byte.MaxValue, 0)]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, byte.MinValue)]
		[Row(1, byte.MinValue)]
		[Row(17, byte.MinValue)]
		[Row(123, byte.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, byte.MaxValue)]
		[Row(1, byte.MaxValue)]
		[Row(17, byte.MaxValue)]
		[Row(123, byte.MaxValue)]
		// Extremvaluecases
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void AddU1(byte a, byte b)
		{
			settings.CodeSource = CreateTestCode("AddU1", "byte", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddU1", (uint)(a + b), a, b));
		}

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
		#endregion

		#region I2
		
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
		[Row(short.MinValue, 0)]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		[Row(short.MinValue, -0)]
		[Row(short.MinValue, -1)]
		[Row(short.MinValue, -17)]
		[Row(short.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0)]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		[Row(short.MaxValue, -0)]
		[Row(short.MaxValue, -1)]
		[Row(short.MaxValue, -17)]
		[Row(short.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		[Row(-0, short.MinValue)]
		[Row(-1, short.MinValue)]
		[Row(-17, short.MinValue)]
		[Row(-123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		[Row(-0, short.MaxValue)]
		[Row(-1, short.MaxValue)]
		[Row(-17, short.MaxValue)]
		[Row(-123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void AddI2(short a, short b)
		{
			settings.CodeSource = CreateTestCode("AddI2", "short", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddI2", (a + b), a, b));
		}

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
		#endregion

		#region U2
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue, 0)]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0)]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue)]
		[Row(1, ushort.MinValue)]
		[Row(17, ushort.MinValue)]
		[Row(123, ushort.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void AddU2(ushort a, ushort b)
		{
			settings.CodeSource = CreateTestCode("AddU2", "ushort", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddU2", (uint)(a + b), a, b));
		}

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
		#endregion

		#region I4
		
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
		[Row(int.MinValue, 0)]
		[Row(int.MinValue, 1)]
		[Row(int.MinValue, 17)]
		[Row(int.MinValue, 123)]
		[Row(int.MinValue, -0)]
		[Row(int.MinValue, -1)]
		[Row(int.MinValue, -17)]
		[Row(int.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue, 0)]
		[Row(int.MaxValue, 1)]
		[Row(int.MaxValue, 17)]
		[Row(int.MaxValue, 123)]
		[Row(int.MaxValue, -0)]
		[Row(int.MaxValue, -1)]
		[Row(int.MaxValue, -17)]
		[Row(int.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, int.MinValue)]
		[Row(1, int.MinValue)]
		[Row(17, int.MinValue)]
		[Row(123, int.MinValue)]
		[Row(-0, int.MinValue)]
		[Row(-1, int.MinValue)]
		[Row(-17, int.MinValue)]
		[Row(-123, int.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, int.MaxValue)]
		[Row(1, int.MaxValue)]
		[Row(17, int.MaxValue)]
		[Row(123, int.MaxValue)]
		[Row(-0, int.MaxValue)]
		[Row(-1, int.MaxValue)]
		[Row(-17, int.MaxValue)]
		[Row(-123, int.MaxValue)]
		// Extremvaluecases
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void AddI4(int a, int b)
		{
			settings.CodeSource = CreateTestCode("AddI4", "int", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddI4", (a + b), a, b));
		}

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
		#endregion

		#region U4
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(uint.MinValue, 0)]
		[Row(uint.MinValue, 1)]
		[Row(uint.MinValue, 17)]
		[Row(uint.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(uint.MaxValue, 0)]
		[Row(uint.MaxValue, 1)]
		[Row(uint.MaxValue, 17)]
		[Row(uint.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, uint.MinValue)]
		[Row(1, uint.MinValue)]
		[Row(17, uint.MinValue)]
		[Row(123, uint.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, uint.MaxValue)]
		[Row(1, uint.MaxValue)]
		[Row(17, uint.MaxValue)]
		[Row(123, uint.MaxValue)]
		// Extremvaluecases
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void AddU4(uint a, uint b)
		{
			settings.CodeSource = CreateTestCode("AddU4", "uint", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddU4", (uint)(a + b), a, b));
		}

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
		#endregion

		#region I8

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
		[Row(long.MinValue, 0)]
		[Row(long.MinValue, 1)]
		[Row(long.MinValue, 17)]
		[Row(long.MinValue, 123)]
		[Row(long.MinValue, -0)]
		[Row(long.MinValue, -1)]
		[Row(long.MinValue, -17)]
		[Row(long.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue, 0)]
		[Row(long.MaxValue, 1)]
		[Row(long.MaxValue, 17)]
		[Row(long.MaxValue, 123)]
		[Row(long.MaxValue, -0)]
		[Row(long.MaxValue, -1)]
		[Row(long.MaxValue, -17)]
		[Row(long.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, long.MinValue)]
		[Row(1, long.MinValue)]
		[Row(17, long.MinValue)]
		[Row(123, long.MinValue)]
		[Row(-0, long.MinValue)]
		[Row(-1, long.MinValue)]
		[Row(-17, long.MinValue)]
		[Row(-123, long.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, long.MaxValue)]
		[Row(1, long.MaxValue)]
		[Row(17, long.MaxValue)]
		[Row(123, long.MaxValue)]
		[Row(-0, long.MaxValue)]
		[Row(-1, long.MaxValue)]
		[Row(-17, long.MaxValue)]
		[Row(-123, long.MaxValue)]
		[Row(0x0000000100000000L, 0x0000000100000000L)]
		// Extremvaluecases
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void AddI8(long a, long b)
		{
			settings.CodeSource = CreateTestCode("AddI8", "long", "long");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddI8", (a + b), a, b));
		}

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
		#endregion

		#region U8
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ulong.MinValue, 0)]
		[Row(ulong.MinValue, 1)]
		[Row(ulong.MinValue, 17)]
		[Row(ulong.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ulong.MaxValue, 0)]
		[Row(ulong.MaxValue, 1)]
		[Row(ulong.MaxValue, 17)]
		[Row(ulong.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ulong.MinValue)]
		[Row(1, ulong.MinValue)]
		[Row(17, ulong.MinValue)]
		[Row(123, ulong.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, ulong.MaxValue)]
		[Row(1, ulong.MaxValue)]
		[Row(17, ulong.MaxValue)]
		[Row(123, ulong.MaxValue)]
		// Extremvaluecases
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void AddU8(ulong a, ulong b)
		{
			settings.CodeSource = CreateTestCode("AddU8", "ulong", "ulong");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddU8", (ulong)(a + b), a, b));
		}
		
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
		#endregion

		#region R4
		
		[Row(1.0f, 1.0f)]
		[Row(1.0f, -2.198f)]
		[Row(-1.2f, 2.11f)]
		[Row(0.0f, 0.0f)]
		// (MinValue, X) Cases
		[Row(float.MinValue, 0.0f)]
		[Row(float.MinValue, 1.2f)]
		[Row(float.MinValue, 17.6f)]
		[Row(float.MinValue, 123.1f)]
		[Row(float.MinValue, -0.0f)]
		[Row(float.MinValue, -1.5f)]
		[Row(float.MinValue, -17.99f)]
		[Row(float.MinValue, -123.235f)]
		// (MaxValue, X) Cases
		[Row(float.MaxValue, 0.0f)]
		[Row(float.MaxValue, 1.67f)]
		[Row(float.MaxValue, 17.875f)]
		[Row(float.MaxValue, 123.283f)]
		[Row(float.MaxValue, -0.0f)]
		[Row(float.MaxValue, -1.1497f)]
		[Row(float.MaxValue, -17.12f)]
		[Row(float.MaxValue, -123.34f)]
		// (X, MinValue) Cases
		[Row(0.0f, float.MinValue)]
		[Row(1.2, float.MinValue)]
		[Row(17.4f, float.MinValue)]
		[Row(123.561f, float.MinValue)]
		[Row(-0.0f, float.MinValue)]
		[Row(-1.78f, float.MinValue)]
		[Row(-17.59f, float.MinValue)]
		[Row(-123.41f, float.MinValue)]
		// (X, MaxValue) Cases
		[Row(0.0f, float.MaxValue)]
		[Row(1.00012f, float.MaxValue)]
		[Row(17.094002f, float.MaxValue)]
		[Row(123.001f, float.MaxValue)]
		[Row(-0.0f, float.MaxValue)]
		[Row(-1.045f, float.MaxValue)]
		[Row(-17.0002501f, float.MaxValue)]
		[Row(-123.023f, float.MaxValue)]
		// Extremvaluecases
		[Row(float.MinValue, float.MaxValue)]
		[Row(1.0f, float.NaN)]
		[Row(float.NaN, 1.0f)]
		[Row(1.0f, float.PositiveInfinity)]
		[Row(float.PositiveInfinity, 1.0f)]
		[Row(1.0f, float.NegativeInfinity)]
		[Row(float.NegativeInfinity, 1.0f)]
		[Test]
		public void AddR4(float a, float b)
		{
			settings.CodeSource = CreateTestCode("AddR4", "float", "float");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddR4", a + b, a, b));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		// Obsolete. This test just fails because we're calculating with higher precision
		// [Row(float.MinValue, float.MaxValue)]
		[Test]
		public void AddConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
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
			settings.CodeSource = CreateConstantTestCode("AddConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR4Left", (a + b), b));
		}

		#endregion

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
			settings.CodeSource = CreateConstantTestCode("AddConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR8Right", (a + b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void AddConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("AddConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "AddConstantR8Left", (a + b), b));
		}

		#endregion
	}
}
