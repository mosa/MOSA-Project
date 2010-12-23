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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.IL
{
	
	[TestFixture]
	public class Shr : TestCompilerAdapter
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return CreateTestCode(name, typeIn, typeIn, typeOut);
		}

		private static string CreateTestCode(string name, string typeInA, string typeInB, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeInA + " a, " + typeInB + @" b)
					{
						return expect == (a >> b);
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
							return expect == (" + constLeft + @" >> x);
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
							return expect == (x >> " + constRight + @");
						}
					}" + Code.AllTestCode;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C
	
		[Row(0, 0)]
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test]
		public void ShrC(char a, char b)
		{
			compiler.CodeSource = CreateTestCode("AddC", "char", "char");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "AddC", (char)(a >> b), a, b));
		}

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void ShrConstantCRight(char a, char b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantCRight", (char)(a >> b), a));
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void ShrConstantCLeft(char a, char b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantCLeft", (char)(a >> b), b));
		}
		#endregion

		#region I1
		
		[Row(1, 2)]
		[Row(23, 3)]
		// And reverse
		[Row(2, 0)]
		[Row(21, -1)]
		// (MinValue, X) Cases
		[Row(sbyte.MinValue, 0)]
		[Row(sbyte.MinValue, 1)]
		[Row(sbyte.MinValue, 17)]
		[Row(sbyte.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(sbyte.MaxValue, 0)]
		[Row(sbyte.MaxValue, 1)]
		[Row(sbyte.MaxValue, 17)]
		[Row(sbyte.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, sbyte.MinValue)]
		[Row(1, sbyte.MinValue)]
		[Row(17, sbyte.MinValue)]
		[Row(123, sbyte.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, sbyte.MaxValue)]
		[Row(1, sbyte.MaxValue)]
		[Row(17, sbyte.MaxValue)]
		[Row(123, sbyte.MaxValue)]
		// Extremvaluecases
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Row(sbyte.MaxValue, sbyte.MinValue)]
		[Row(unchecked((sbyte)0x80), 8)]
		[Test]
		public void ShrI1(sbyte a, sbyte b)
		{
			compiler.CodeSource = CreateTestCode("ShrI1", "sbyte", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrI1", a >> b, a, b));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShrConstantI1Right(sbyte a, sbyte b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI1Right", (a >> b), a));
		}

		[Row(-42, 48)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void ShrConstantI1Left(sbyte a, sbyte b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI1Left", (a >> b), b));
		}
		#endregion

		#region I2
	
		[Row(1, 2)]
		[Row(23, 3)]
		// And reverse
		[Row(2, 0)]
		[Row(21, -1)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0)]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0)]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Row(short.MaxValue, short.MinValue)]
		[Row(unchecked((short)0x8000), 16)]
		[Test]
		public void ShrI2(short a, short b)
		{
			compiler.CodeSource = CreateTestCode("ShrI2", "short", "int");
			int v = (a >> b);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrI2", v, a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShrConstantI2Right(short a, short b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI2Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void ShrConstantI2Left(short a, short b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI2Left", (a >> b), b));
		}
		#endregion

		#region I4
	
		[Row(1, 2)]
		[Row(23, 3)]
		// And reverse
		[Row(2, 0)]
		[Row(21, -1)]
		// (MinValue, X) Cases
		[Row(int.MinValue, 0)]
		[Row(int.MinValue, 1)]
		[Row(int.MinValue, 17)]
		[Row(int.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue, 0)]
		[Row(int.MaxValue, 1)]
		[Row(int.MaxValue, 17)]
		[Row(int.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, int.MinValue)]
		[Row(1, int.MinValue)]
		[Row(17, int.MinValue)]
		[Row(123, int.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, int.MaxValue)]
		[Row(1, int.MaxValue)]
		[Row(17, int.MaxValue)]
		[Row(123, int.MaxValue)]
		// Extremvaluecases
		[Row(int.MinValue, int.MaxValue)]
		[Row(int.MaxValue, int.MinValue)]
		[Row(unchecked((int)0x80000000), 32)]
		[Test]
		public void ShrI4(int a, int b)
		{
			compiler.CodeSource = CreateTestCode("ShrI4", "int", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrI4", (a >> b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI4Right(int a, int b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI4Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI4Left(int a, int b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI4Left", (a >> b), b));
		}
		#endregion

		#region I8
		
		[Row(1, 2)]
		[Row(23, 3)]
		// And reverse
		[Row(2, 0)]
		[Row(21, -1)]
		// (MinValue, X) Cases
		[Row(long.MinValue, 0)]
		[Row(long.MinValue, 1)]
		[Row(long.MinValue, 17)]
		[Row(long.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue, 0)]
		[Row(long.MaxValue, 1)]
		[Row(long.MaxValue, 17)]
		[Row(long.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, int.MinValue)]
		[Row(1, int.MinValue)]
		[Row(17, int.MinValue)]
		[Row(123, int.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, int.MaxValue)]
		[Row(1, int.MaxValue)]
		[Row(17, int.MaxValue)]
		[Row(123, int.MaxValue)]
		// Extremvaluecases
		[Row(long.MinValue, int.MaxValue)]
		[Row(long.MaxValue, int.MinValue)]
		[Row(unchecked((long)0x8000000000000000), 64)]
		[Test]
		public void ShrI8(long a, int b)
		{
			compiler.CodeSource = CreateTestCode("ShrI8", "long", "int", "long");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrI8", (a >> b), a, b));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI8Right(long a, int b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI8Right", (a >> b), a));
		}

		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, int.MaxValue)]
		[Test]
		public void ShrConstantI8Left(long a, int b)
		{
			compiler.CodeSource = CreateConstantTestCode("ShrConstantI8Left", "int", "long", a.ToString(), null);
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "ShrConstantI8Left", (a >> b), b));
		}
		#endregion
	}
}
