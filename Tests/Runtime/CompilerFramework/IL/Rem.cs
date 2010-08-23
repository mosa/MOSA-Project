/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:kintaro@think-in-co.de>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
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
	public class Rem : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a % b);
					}
				}" + Code.ObjectClassDefinition;
		}

		private static string CreateTestCodeWithReturn(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return (a % b);
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
							return expect == (" + constLeft + @" % x);
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
							return expect == (x % " + constRight + @");
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
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemC(char a, char b)
		{
			CodeSource = CreateTestCode("RemC", "char", "char");
			Assert.IsTrue((bool)Run<C_C_C>("", "Test", "RemC", (char)(a % b), a, b));
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
		public void RemConstantCRight(char a, char b)
		{
			CodeSource = CreateConstantTestCode("RemConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "RemConstantCRight", (char)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row('a', 0, ExpectedException = typeof(DivideByZeroException))]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantCLeft(char a, char b)
		{
			CodeSource = CreateConstantTestCode("RemConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
			Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "RemConstantCLeft", (char)(a % b), b));
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
		delegate bool I4_I1_I1(sbyte expect, sbyte a, sbyte b);
		delegate int I4_I1_I1_Return(sbyte expect, sbyte a, sbyte b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(sbyte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, 1)]
		[Row(sbyte.MinValue, 17)]
		[Row(sbyte.MinValue, 123)]
		[Row(sbyte.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, -1)]
		[Row(sbyte.MinValue, -17)]
		[Row(sbyte.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(sbyte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MaxValue, 1)]
		[Row(sbyte.MaxValue, 17)]
		[Row(sbyte.MaxValue, 123)]
		[Row(sbyte.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemI1(sbyte a, sbyte b)
		{
			CodeSource = CreateTestCodeWithReturn("RemI1", "sbyte", "int");
			Assert.AreEqual(a % b, Run<I4_I1_I1_Return>("", "Test", "RemI1", (sbyte)(a % b), a, b));
		}

		delegate bool I1_Constant_I1(sbyte expect, sbyte x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI1Right(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI1Right", "sbyte", "sbyte", null, b.ToString());
			Assert.IsTrue((bool)Run<I1_Constant_I1>("", "Test", "RemConstantI1Right", (sbyte)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI1Left(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI1Left", "sbyte", "sbyte", a.ToString(), null);
			Assert.IsTrue((bool)Run<I1_Constant_I1>("", "Test", "RemConstantI1Left", (sbyte)(a % b), b));
		}
		#endregion

		#region U1
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool U4_U1_U1(byte expect, byte a, byte b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(byte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, 1)]
		[Row(byte.MinValue, 17)]
		[Row(byte.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, byte.MaxValue)]
		[Row(1, byte.MaxValue)]
		[Row(17, byte.MaxValue)]
		[Row(123, byte.MaxValue)]
		// Extremvaluecases
		[Row(byte.MinValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemU1(byte a, byte b)
		{
			CodeSource = CreateTestCode("RemU1", "byte", "byte");
			Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "RemU1", (byte)(a % b), a, b));
		}

		delegate bool U1_Constant_U1(byte expect, byte x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		//[Row(byte.MinValue, byte.MaxValue)] FIXME: Uncommenting this lets the test runner freeze
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU1Right(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU1Right", "byte", "byte", null, b.ToString());
			Assert.IsTrue((bool)Run<U1_Constant_U1>("", "Test", "RemConstantU1Right", (byte)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU1Left(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU1Left", "byte", "byte", a.ToString(), null);
			Assert.IsTrue((bool)Run<U1_Constant_U1>("", "Test", "RemConstantU1Left", (byte)(a % b), b));
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
		delegate bool I4_I2_I2(short expect, short a, short b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		[Row(short.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, -1)]
		[Row(short.MinValue, -17)]
		[Row(short.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		[Row(short.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemI2(short a, short b)
		{
			CodeSource = CreateTestCode("RemI2", "short", "short");
			Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "RemI2", (short)(a % b), a, b));
		}

		delegate bool I2_Constant_I2(short expect, short x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI2Right(short a, short b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI2Right", "short", "short", null, b.ToString());
			Assert.IsTrue((bool)Run<I2_Constant_I2>("", "Test", "RemConstantI2Right", (short)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI2Left(short a, short b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI2Left", "short", "short", a.ToString(), null);
			Assert.IsTrue((bool)Run<I2_Constant_I2>("", "Test", "RemConstantI2Left", (short)(a % b), b));
		}
		#endregion

		#region U2
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool U4_U2_U2(ushort expect, ushort a, ushort b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Row(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemU2(ushort a, ushort b)
		{
			CodeSource = CreateTestCode("RemU2", "ushort", "ushort");
			Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "RemU2", (ushort)(a % b), a, b));
		}

		delegate bool U2_Constant_U2(ushort expect, ushort x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU2Right(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU2Right", "ushort", "ushort", null, b.ToString());
			Assert.IsTrue((bool)Run<U2_Constant_U2>("", "Test", "RemConstantU2Right", (ushort)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU2Left(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU2Left", "ushort", "ushort", a.ToString(), null);
			Assert.IsTrue((bool)Run<U2_Constant_U2>("", "Test", "RemConstantU2Left", (ushort)(a % b), b));
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
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, 1)]
		[Row(int.MinValue, 17)]
		[Row(int.MinValue, 123)]
		[Row(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
		[Row(int.MinValue, -17)]
		[Row(int.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MaxValue, 1)]
		[Row(int.MaxValue, 17)]
		[Row(int.MaxValue, 123)]
		[Row(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemI4(int a, int b)
		{
			CodeSource = CreateTestCode("RemI4", "int", "int");
			Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "RemI4", (int)(a % b), a, b));
		}

		delegate bool I4_Constant_I4(int expect, int x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "RemConstantI4Right", (a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "RemConstantI4Left", (a % b), b));
		}
		#endregion

		#region U4
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool U4_U4_U4(uint expect, uint a, uint b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(uint.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(uint.MinValue, 1)]
		[Row(uint.MinValue, 17)]
		[Row(uint.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(uint.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(uint.MaxValue, 1)]
		[Row(uint.MaxValue, 17)]
		[Row(uint.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, uint.MaxValue)]
		[Row(1, uint.MaxValue)]
		[Row(17, uint.MaxValue)]
		[Row(123, uint.MaxValue)]
		// Extremvaluecases
		[Row(uint.MinValue, uint.MaxValue)]
		[Row(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemU4(uint a, uint b)
		{
			CodeSource = CreateTestCode("RemU4", "uint", "uint");
			Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "RemU4", (uint)(a % b), a, b));
		}

		delegate bool U4_Constant_U4(uint expect, uint x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU4Right(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "RemConstantU4Right", (uint)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU4Left(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "RemConstantU4Left", (uint)(a % b), b));
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
		delegate long I8_I8_I8_Return(long expect, long a, long b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, 1)]
		[Row(long.MinValue, 17)]
		[Row(long.MinValue, 123)]
		[Row(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
		[Row(long.MinValue, -17)]
		[Row(long.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MaxValue, 1)]
		[Row(long.MaxValue, 17)]
		[Row(long.MaxValue, 123)]
		[Row(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
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
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemI8(long a, long b)
		{
			CodeSource = CreateTestCodeWithReturn("RemI8", "long", "long");
			Assert.AreEqual((long)(a % b), (long)Run<I8_I8_I8_Return>("", "Test", "RemI8", (long)(a % b), a, b));
		}

		delegate bool I8_Constant_I8(long expect, long x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI8Right(long a, long b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "RemConstantI8Right", (a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantI8Left(long a, long b)
		{
			CodeSource = CreateConstantTestCode("RemConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "RemConstantI8Left", (a % b), b));
		}
		#endregion

		#region U8
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expect"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		delegate bool U8_U8_U8(ulong expect, ulong a, ulong b);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ulong.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MinValue, 1)]
		[Row(ulong.MinValue, 17)]
		[Row(ulong.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ulong.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MaxValue, 1)]
		[Row(ulong.MaxValue, 17)]
		[Row(ulong.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, ulong.MaxValue)]
		[Row(1, ulong.MaxValue)]
		[Row(17, ulong.MaxValue)]
		[Row(123, ulong.MaxValue)]
		// Extremvaluecases
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Row(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void RemU8(ulong a, ulong b)
		{
			CodeSource = CreateTestCode("RemU8", "ulong", "ulong");
			Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "RemU8", (ulong)(a % b), a, b));
		}

		delegate bool U8_Constant_U8(ulong expect, ulong x);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU8Right(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU8Right", "ulong", "ulong", null, b.ToString());
			Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "RemConstantU8Right", (ulong)(a % b), a));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		[Row(23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void RemConstantU8Left(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("RemConstantU8Left", "ulong", "ulong", a.ToString(), null);
			Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "RemConstantU8Left", (ulong)(a % b), b));
		}
		#endregion
	}
}
