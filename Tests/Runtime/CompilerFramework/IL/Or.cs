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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class OrFixture : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a | b);
					}
				}" + Code.AllTestCode;
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
							return expect == (" + constLeft + @" | x);
						}
					}" + Code.AllTestCode;
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
					}" + Code.AllTestCode;
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
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrB(bool a, bool b)
		{
			CodeSource = CreateTestCode("OrB", "bool", "bool");
			Assert.IsTrue(Run<bool>("", "Test", "OrB", (a | b), a, b));
		}
	
		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantBRight(bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("OrConstantBRight", "bool", "bool", null, b.ToString().ToLower());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantBRight", (a | b), a));
		}

		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantBLeft(bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("OrConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantBLeft", (a | b), b));
		}
		#endregion

		#region C
		
		[Row(0, 0)]
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrC(char a, char b)
		{
			CodeSource = CreateTestCode("OrC", "char", "char");
			Assert.IsTrue(Run<bool>("", "Test", "OrC", (char)(a | b), a, b));
		}

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantCRight(char a, char b)
		{
			CodeSource = CreateConstantTestCode("OrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantCRight", (char)(a | b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantCLeft(char a, char b)
		{
			CodeSource = CreateConstantTestCode("OrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantCLeft", (char)(a | b), b));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrI1(sbyte a, sbyte b)
		{
			CodeSource = CreateTestCode("OrI1", "sbyte", "int");
			Assert.IsTrue(Run<bool>("", "Test", "OrI1", a | b, a, b));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI1Right(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI1Right", (a | b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI1Left(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI1Left", (a | b), b));
		}
		#endregion

		#region U1
		
		[Row(1, 2)]
		[Row(23, 21)]
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
		[Row(byte.MaxValue, byte.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrU1(byte a, byte b)
		{
			CodeSource = CreateTestCode("OrU1", "byte", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "OrU1", (uint)(a | b), a, b));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU1Right(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU1Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU1Left(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU1Left", (uint)(a | b), b));
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
		[Row(short.MaxValue, short.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrI2(short a, short b)
		{
			short e = (short)(a | b);
			CodeSource = CreateTestCode("OrI2", "short", "int");
			Assert.IsTrue(Run<bool>("", "Test", "OrI2", (a | b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI2Right(short a, short b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI2Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI2Left(short a, short b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI2Left", (a | b), b));
		}
		#endregion

		#region U2
		
		[Row(1, 2)]
		[Row(23, 21)]
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
		[Row(ushort.MaxValue, ushort.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrU2(ushort a, ushort b)
		{
			ushort e = (ushort)(a | b);
			CodeSource = CreateTestCode("OrU2", "ushort", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "OrU2", (uint)(a | b), a, b));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU2Right(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU2Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU2Left(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU2Left", (uint)(a | b), b));
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
		[Row(int.MaxValue, int.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrI4(int a, int b)
		{
			CodeSource = CreateTestCode("OrI4", "int", "int");
			Assert.IsTrue(Run<bool>("", "Test", "OrI4", (a | b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI4Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI4Left", (a | b), b));
		}
		#endregion

		#region U4
		
		[Row(1, 2)]
		[Row(23, 21)]
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
		[Row(uint.MaxValue, uint.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrU4(uint a, uint b)
		{
			CodeSource = CreateTestCode("OrU4", "uint", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "OrU4", (uint)(a | b), a, b));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU4Right(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU4Right", (uint)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU4Left(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU4Left", (uint)(a | b), b));
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
		// Extremvaluecases
		[Row(long.MinValue, long.MaxValue)]
		[Row(long.MaxValue, long.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrI8(long a, long b)
		{
			CodeSource = CreateTestCode("OrI8", "long", "long");
			Assert.IsTrue(Run<bool>("", "Test", "OrI8", (a | b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI8Right(long a, long b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI8Right", (a | b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantI8Left(long a, long b)
		{
			CodeSource = CreateConstantTestCode("OrConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantI8Left", (a | b), b));
		}
		#endregion

		#region U8
	
		[Row(1, 2)]
		[Row(23, 21)]
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
		[Row(ulong.MaxValue, ulong.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void OrU8(ulong a, ulong b)
		{
			CodeSource = CreateTestCode("OrU8", "ulong", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "OrU8", (ulong)(a | b), a, b));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU8Right(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU8Right", (ulong)(a | b), a));
		}

		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void OrConstantU8Left(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("OrConstantU8Left", "ulong", "ulong", a.ToString(), null);
			Assert.IsTrue(Run<bool>("", "Test", "OrConstantU8Left", (ulong)(a | b), b));
		}
		#endregion
	}
}
