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
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class AndFixture : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a & b);
					}
				}" + Code.ObjectClassDefinition;
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
							return expect == (" + constLeft + @" & x);
						}
					}" + Code.ObjectClassDefinition;
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
					}" + Code.ObjectClassDefinition;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region B
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool B_B_B(bool expect, bool a, bool b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndB(bool a, bool b)
		{
			CodeSource = CreateTestCode("AndB", "bool", "bool");
			Assert.IsTrue((bool)Run<B_B_B>("", "Test", "AndB", (a & b), a, b));
		}

		delegate bool B_Constant_B(bool expect, bool x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantBRight(bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("AndConstantBRight", "bool", "bool", null, b.ToString().ToLower());
			Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "AndConstantBRight", (a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(true, true)]
		[Row(true, false)]
		[Row(false, false)]
		[Row(false, true)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantBLeft(bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("AndConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
			Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "AndConstantBLeft", (a & b), b));
		}
		#endregion

		#region C
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool C_C_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(0, 0)]
		[Row(0, 1)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndC(char a, char b)
		{
			CodeSource = CreateTestCode("AndC", "char", "char");
			Assert.IsTrue((bool)Run<C_C_C>("", "Test", "AndC", (char)(a & b), a, b));
		}

		delegate bool C_Constant_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantCRight(char a, char b)
		{
			CodeSource = CreateConstantTestCode("AndConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "AndConstantCRight", (char)(a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantCLeft(char a, char b)
		{
			CodeSource = CreateConstantTestCode("AndConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "AndConstantCLeft", (char)(a & b), b));
		}
		#endregion

		#region I1
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
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
		[Row(sbyte.MaxValue, sbyte.MinValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void AndI1(sbyte a, sbyte b)
		{
			CodeSource = CreateTestCode("AndI1", "sbyte", "int");
			Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "AndI1", (a & b), a, b));
		}

		delegate bool I4_Constant_I1(int expect, sbyte x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI1Right(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "AndConstantI1Right", (a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI1Left(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "AndConstantI1Left", (a & b), b));
		}
		#endregion

		#region I2
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool I4_I2_I2(int expect, short a, short b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
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
		public void AndI2(short a, short b)
		{
			CodeSource = CreateTestCode("AndI2", "short", "int");
			Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "AndI2", (a & b), a, b));
		}

		delegate bool I4_Constant_I2(int expect, short x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI2Right(short a, short b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "AndConstantI2Right", (a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI2Left(short a, short b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "AndConstantI2Left", (a & b), b));
		}
		#endregion

		#region I4
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool I4_I4_I4(int expect, int a, int b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
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
		public void AndI4(int a, int b)
		{
			CodeSource = CreateTestCode("AndI4", "int", "int");
			Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "AndI4", (a & b), a, b));
		}

		delegate bool I4_Constant_I4(int expect, int x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "AndConstantI4Right", (a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "AndConstantI4Left", (a & b), b));
		}
		#endregion

		#region I8
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool I8_I8_I8(long expect, long a, long b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
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
		public void AndI8(long a, long b)
		{
			CodeSource = CreateTestCode("AndI8", "long", "long");
			Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "AndI8", (a & b), a, b));
		}

		delegate bool I8_Constant_I8(long expect, long x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI8Right(long a, long b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "AndConstantI8Right", (a & b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void AndConstantI8Left(long a, long b)
		{
			CodeSource = CreateConstantTestCode("AndConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "AndConstantI8Left", (a & b), b));
		}
		#endregion
	}
}
