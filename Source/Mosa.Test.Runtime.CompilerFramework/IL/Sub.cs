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
	public class Sub : TestCompilerAdapter
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a - b);
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
	
		[Row(0, 0)]
		[Row(128, 17)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test]
		public void SubC(char a, char b)
		{
			settings.CodeSource = CreateTestCode("SubC", "char", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubC", (a - b), a, b));
		}

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
		public void SubI1(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateTestCode("SubI1", "sbyte", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubI1", a - b, a, b));
		}

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
		public void SubI2(short a, short b)
		{
			settings.CodeSource = CreateTestCode("SubI2", "short", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubI2", (a - b), a, b));
		}

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
		[Test]
		public void SubU2(ushort a, ushort b)
		{
			settings.CodeSource = CreateTestCode("SubU2", "ushort", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubU2", (a - b), a, b));
		}

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
		public void SubI4(int a, int b)
		{
			settings.CodeSource = CreateTestCode("SubI4", "int", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubI4", (a - b), a, b));
		}

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
		[Test]
		public void SubI8(long a, long b)
		{
			settings.CodeSource = CreateTestCode("SubI8", "long", "long");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubI8", (a - b), a, b));
		}

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
		#endregion

		#region R4
		
		[Row(1.2f, 2.1f)]
		[Row(23.0f, 21.2578f)]
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
		[Row(1.0f, float.NaN)]
		[Row(float.NaN, 1.0f)]
		[Row(1.0f, float.PositiveInfinity)]
		[Row(float.PositiveInfinity, 1.0f)]
		[Row(1.0f, float.NegativeInfinity)]
		[Row(float.NegativeInfinity, 1.0f)]
		[Test]
		public void SubR4(float a, float b)
		{
			settings.CodeSource = CreateTestCode("SubR4", "float", "float");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubR4", (a - b), a, b));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		[Test]
		public void SubConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
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
			settings.CodeSource = CreateConstantTestCode("SubConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR4Left", (a - b), b));
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
		public void SubR8(double a, double b)
		{
			settings.CodeSource = CreateTestCode("SubR8", "double", "double");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubR8", (a - b), a, b));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void SubConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR8Right", (a - b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void SubConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("SubConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "SubConstantR8Left", (a - b), b));
		}
		#endregion

	}
}
