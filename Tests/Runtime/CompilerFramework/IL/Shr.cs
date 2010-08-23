/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
	public class Shr : CodeDomTestRunner
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
							return expect == (" + constLeft + @" >> x);
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
							return expect == (x >> " + constRight + @");
						}
					}" + Code.ObjectClassDefinition;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

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
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void ShrC(char a, char b)
		{
			CodeSource = CreateTestCode("AddC", "char", "char");
			Assert.IsTrue((bool)Run<C_C_C>("", "Test", "AddC", (char)(a >> b), a, b));
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
		public void ShrConstantCRight(char a, char b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "ShrConstantCRight", (char)(a >> b), a));
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
		public void ShrConstantCLeft(char a, char b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "ShrConstantCLeft", (char)(a >> b), b));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ShrI1(sbyte a, sbyte b)
		{
			CodeSource = CreateTestCode("ShrI1", "sbyte", "int");
			Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "ShrI1", a >> b, a, b));
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
		public void ShrConstantI1Right(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "ShrConstantI1Right", (a >> b), a));
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
		public void ShrConstantI1Left(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "ShrConstantI1Left", (a >> b), b));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ShrI2(short a, short b)
		{
			CodeSource = CreateTestCode("ShrI2", "short", "int");
			int v = (a >> b);
			Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "ShrI2", v, a, b));
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
		public void ShrConstantI2Right(short a, short b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "ShrConstantI2Right", (a >> b), a));
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
		public void ShrConstantI2Left(short a, short b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "ShrConstantI2Left", (a >> b), b));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ShrI4(int a, int b)
		{
			CodeSource = CreateTestCode("ShrI4", "int", "int");
			Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "ShrI4", (a >> b), a, b));
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
		public void ShrConstantI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "ShrConstantI4Right", (a >> b), a));
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
		public void ShrConstantI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "ShrConstantI4Left", (a >> b), b));
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
		delegate bool I8_I8_I4(long expect, long a, int b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void ShrI8(long a, int b)
		{
			CodeSource = CreateTestCode("ShrI8", "long", "int", "long");
			Assert.IsTrue((bool)Run<I8_I8_I4>("", "Test", "ShrI8", (a >> b), a, b));
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
		[Row(long.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void ShrConstantI8Right(long a, int b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "ShrConstantI8Right", (a >> b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0)]
		[Row(long.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void ShrConstantI8Left(long a, int b)
		{
			CodeSource = CreateConstantTestCode("ShrConstantI8Left", "int", "long", a.ToString(), null);
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "ShrConstantI8Left", (a >> b), b));
		}
		#endregion
	}
}
