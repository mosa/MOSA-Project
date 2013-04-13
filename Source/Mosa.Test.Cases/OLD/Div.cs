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
using System.Globalization;

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Div : TestCompilerAdapter
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
							return expect == (" + constLeft + @" / x);
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
							return expect == (x / " + constRight + @");
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
							return (" + constLeft + @" / x);
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
							return (x / " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C

		//[Row(0, 'a')]
		//[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void DivConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("DivConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
			Assert.AreEqual(a / b, Run<int>(string.Empty, "Test", "DivConstantCRight", (a / b), a));
		}

		[Row('a', 0, ExpectedException = typeof(DivideByZeroException))]
		[Row('-', '.')]
		[Row((char)97, (char)90)]
		[Test]
		public void DivConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("DivConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
			Assert.AreEqual(a / b, Run<int>(string.Empty, "Test", "DivConstantCLeft", (a / b), (char)b));
		}

		#endregion C

		#region I1

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void DivConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI1Right", (a / b), a));
		}

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void DivConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI1Left", (a / b), b));
		}

		#endregion I1

		#region U1

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void DivConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU1Right", (uint)(a / b), a));
		}

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void DivConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU1Left", (uint)(a / b), b));
		}

		#endregion U1

		#region I2

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void DivConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI2Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void DivConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI2Left", (a / b), b));
		}

		#endregion I2

		#region U2

		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void DivConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU2Right", (uint)(a / b), a));
		}

		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void DivConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU2Left", (uint)(a / b), b));
		}

		#endregion U2

		#region I4

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void DivConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI4Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void DivConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI4Left", (a / b), b));
		}

		#endregion I4

		#region U4

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void DivConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU4Right", (uint)(a / b), a));
		}

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void DivConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU4Left", (uint)(a / b), b));
		}

		#endregion U4

		#region U8

		#endregion U8

		#region I8

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void DivConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI8Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void DivConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI8Left", (a / b), b));
		}

		#endregion I8

		#region R4

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]

		//[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void DivConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR4Right", "float", "float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR4Right", (a / b), a));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]

		//[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void DivConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR4Left", "float", "float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR4Left", (a / b), b));
		}

		#endregion R4

		#region R8

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void DivConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR8Right", "double", "double", null, b.ToString(CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR8Right", (a / b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void DivConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR8Left", "double", "double", a.ToString(CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR8Left", (a / b), b));
		}

		#endregion R8
	}
}