/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
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
	/// Tests support for the IL ceq operation with various operands.
	/// </summary>
	[TestFixture]
	public class Ceq : CodeDomTestRunner
	{
		private static string s_testCode = @"
			static class Test {
				public static bool Ceq(t1 a, t2 b) {
					return (a == b);
				}
			}
		" + Code.ObjectClassDefinition;

		private static string CreateConstantTestCode(string typeIn, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool CeqConstant(" + typeIn + @" x)
						{
							return (" + constLeft + @" == x);
						}
					}" + Code.ObjectClassDefinition;
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static bool CeqConstant(" + typeIn + @" x)
						{
							return (x == " + constRight + @");
						}
					}" + Code.ObjectClassDefinition;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		delegate bool B_B_B(bool a, bool b);
		delegate bool B_C_C([MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);
		delegate bool B_I1_I1(sbyte a, sbyte b);
		delegate bool B_I2_I2(short a, short b);
		delegate bool B_I4_I4(int a, int b);
		delegate bool B_I8_I8(long a, long b);
		delegate bool B_U1_U1(byte a, byte b);
		delegate bool B_U2_U2(ushort a, ushort b);
		delegate bool B_U4_U4(uint a, uint b);
		delegate bool B_U8_U8(ulong a, ulong b);
		delegate bool B_R4_R4(float a, float b);
		delegate bool B_R8_R8(double a, double b);

		delegate bool B_Constant_B(bool x);
		delegate bool B_Constant_C([MarshalAs(UnmanagedType.U2)]char x);
		delegate bool B_Constant_I1(sbyte x);
		delegate bool B_Constant_I2(short x);
		delegate bool B_Constant_I4(int x);
		delegate bool B_Constant_I8(long x);
		delegate bool B_Constant_U1(byte x);
		delegate bool B_Constant_U2(ushort x);
		delegate bool B_Constant_U4(uint x);
		delegate bool B_Constant_U8(ulong x);
		delegate bool B_Constant_R4(float x);
		delegate bool B_Constant_R8(double x);

		#region B
		/// <summary>
		/// Tests support for the ceq IL operation for boolean operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqB(bool result, bool a, bool b)
		{
			CodeSource = s_testCode.Replace("t1", "bool").Replace("t2", "bool");
			bool res = (bool)Run<B_B_B>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for boolean operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author("boddlnagg")]
		public void CeqConstantBRight(bool result, bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("bool", null, b.ToString().ToLower());
			bool res = (bool)Run<B_Constant_B>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for boolean operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author("boddlnagg")]
		public void CeqConstantBLeft(bool result, bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("bool", a.ToString().ToLower(), null);
			bool res = (bool)Run<B_Constant_B>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region C
		/// <summary>
		/// Tests support for the ceq IL operation for char operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 'a', 'Z')]
		[Row(true, 'a', 'a')]
		[Row(false, 0, 128)]
		[Test, Author("boddlnagg")]
		public void CeqC(bool result, char a, char b)
		{
			CodeSource = s_testCode.Replace("t1", "char").Replace("t2", "char");
			bool res = (bool)Run<B_C_C>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for char operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(false, 0, 'a')]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test, Author("boddlnagg")]
		public void CeqConstantCRight(bool result, char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for char operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(false, 'a', 0)]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test, Author("boddlnagg")]
		public void CeqConstantCLeft(bool result, char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = (bool)Run<B_Constant_C>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I1
		/// <summary>
		/// Tests support for the ceq IL operation for I1 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, SByte.MinValue, SByte.MinValue)]
		[Row(true, SByte.MaxValue, SByte.MaxValue)]
		[Row(false, 0, SByte.MinValue)]
		[Row(false, 0, SByte.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, SByte.MinValue, 0)]
		[Row(false, SByte.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqI1(bool result, sbyte a, sbyte b)
		{
			CodeSource = s_testCode.Replace("t1", "sbyte").Replace("t2", "sbyte");
			bool res = (bool)Run<B_I1_I1>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I1 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI1Right(bool result, sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I1 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI1Left(bool result, sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = (bool)Run<B_Constant_I1>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I2
		/// <summary>
		/// Tests support for the ceq IL operation for I2 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, Int16.MinValue, Int16.MinValue)]
		[Row(true, Int16.MaxValue, Int16.MaxValue)]
		[Row(false, 0, Int16.MinValue)]
		[Row(false, 0, Int16.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, Int16.MinValue, 0)]
		[Row(false, Int16.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqI2(bool result, short a, short b)
		{
			CodeSource = s_testCode.Replace("t1", "short").Replace("t2", "short");
			bool res = (bool)Run<B_I2_I2>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I2 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI2Right(bool result, short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I2 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI2Left(bool result, short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = (bool)Run<B_Constant_I2>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I4
		/// <summary>
		/// Tests support for the ceq IL operation for I4 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, Int32.MinValue, Int32.MinValue)]
		[Row(true, Int32.MaxValue, Int32.MaxValue)]
		[Row(false, 0, Int32.MinValue)]
		[Row(false, 0, Int32.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, Int32.MinValue, 0)]
		[Row(false, Int32.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqI4(bool result, int a, int b)
		{
			CodeSource = s_testCode.Replace("t1", "int").Replace("t2", "int");
			bool res = (bool)Run<B_I4_I4>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I2 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI4Right(bool result, int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I2 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI4Left(bool result, int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = (bool)Run<B_Constant_I4>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I8
		/// <summary>
		/// Tests support for the ceq IL operation for I8 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0L, 0L)]
		[Row(true, 1L, 1L)]
		[Row(true, Int64.MinValue, Int64.MinValue)]
		[Row(true, Int64.MaxValue, Int64.MaxValue)]
		[Row(false, 0L, Int64.MinValue)]
		[Row(false, 0L, Int64.MaxValue)]
		[Row(false, 0L, 1L)]
		[Row(false, Int64.MinValue, 0L)]
		[Row(false, Int64.MaxValue, 0L)]
		[Row(false, 1L, 0L)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqI8(bool result, long a, long b)
		{
			CodeSource = s_testCode.Replace("t1", "long").Replace("t2", "long");
			bool res = (bool)Run<B_I8_I8>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I8 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI8Right(bool result, long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for I8 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI8Left(bool result, long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = (bool)Run<B_Constant_I8>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U1
		/// <summary>
		/// Tests support for the ceq IL operation for U1 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, Byte.MinValue, Byte.MinValue)]
		[Row(true, Byte.MaxValue, Byte.MaxValue)]
		[Row(false, 1, Byte.MinValue)]
		[Row(false, 0, Byte.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, Byte.MinValue, 1)]
		[Row(false, Byte.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqU1(bool result, byte a, byte b)
		{
			CodeSource = s_testCode.Replace("t1", "byte").Replace("t2", "byte");
			bool res = (bool)Run<B_U1_U1>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U1 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU1Right(bool result, byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U1 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU1Left(bool result, byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = (bool)Run<B_Constant_U1>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U2
		/// <summary>
		/// Tests support for the ceq IL operation for U2 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, UInt16.MinValue, UInt16.MinValue)]
		[Row(true, UInt16.MaxValue, UInt16.MaxValue)]
		[Row(false, 1, UInt16.MinValue)]
		[Row(false, 0, UInt16.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, UInt16.MinValue, 2)]
		[Row(false, UInt16.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqU2(bool result, ushort a, ushort b)
		{
			CodeSource = s_testCode.Replace("t1", "ushort").Replace("t2", "ushort");
			bool res = (bool)Run<B_U2_U2>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U2 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU2Right(bool result, ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U2 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU2Left(bool result, ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = (bool)Run<B_Constant_U2>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U4
		/// <summary>
		/// Tests support for the ceq IL operation for U4 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, UInt32.MinValue, UInt32.MinValue)]
		[Row(true, UInt32.MaxValue, UInt32.MaxValue)]
		[Row(false, 1, UInt32.MinValue)]
		[Row(false, 0, UInt32.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, UInt32.MinValue, 1)]
		[Row(false, UInt32.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqU4(bool result, uint a, uint b)
		{
			CodeSource = s_testCode.Replace("t1", "uint").Replace("t2", "uint");
			bool res = (bool)Run<B_U4_U4>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U4 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU4Right(bool result, uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U4 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU4Left(bool result, uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = (bool)Run<B_Constant_U4>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U8
		/// <summary>
		/// Tests support for the ceq IL operation for U4 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(true, 1, 1)]
		[Row(true, UInt64.MinValue, UInt64.MinValue)]
		[Row(true, UInt64.MaxValue, UInt64.MaxValue)]
		[Row(false, 1, UInt64.MinValue)]
		[Row(false, 0, UInt64.MaxValue)]
		[Row(false, 0, 1)]
		[Row(false, UInt64.MinValue, 1)]
		[Row(false, UInt64.MaxValue, 0)]
		[Row(false, 1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqU8(bool result, ulong a, ulong b)
		{
			CodeSource = s_testCode.Replace("t1", "ulong").Replace("t2", "ulong");
			bool res = (bool)Run<B_U8_U8>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U8 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU8Right(bool result, ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for U8 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU8Left(bool result, ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = (bool)Run<B_Constant_U8>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R4
		/// <summary>
		/// Tests support for the ceq IL operation for R4 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0.0f, 0.0f)]
		[Row(true, 1.0f, 1.0f)]
		[Row(true, Single.MinValue, Single.MinValue)]
		[Row(true, Single.MaxValue, Single.MaxValue)]
		[Row(false, 0.0f, Single.MinValue)]
		[Row(false, 0.0f, Single.MaxValue)]
		[Row(false, 0.0f, 1.0f)]
		[Row(false, Single.MinValue, 0.0f)]
		[Row(false, Single.MaxValue, 0.0f)]
		[Row(false, 1.0f, 0.0f)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqR4(bool result, float a, float b)
		{
			CodeSource = s_testCode.Replace("t1", "float").Replace("t2", "float");
			bool res = (bool)Run<B_R4_R4>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for R4 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0f, 0f)]
		[Row(true, 13.9f, 13.9f)]
		[Row(true, 11.91262f, 11.91262f)]
		[Row(false, 11.91262f, 11.91263f)]
		[Row(false, -17f, 42f)]
		[Row(false, Single.MinValue, Single.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR4Right(bool result, float a, float b)
		{
			CodeSource = CreateConstantTestCode("float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
			bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for R4 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0f, 0f)]
		[Row(true, 13.9f, 13.9f)]
		[Row(true, 11.91262f, 11.91262f)]
		[Row(false, 11.91262f, 11.91263f)]
		[Row(false, -17f, 42f)]
		[Row(false, Single.MinValue, Single.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR4Left(bool result, float a, float b)
		{
			CodeSource = CreateConstantTestCode("float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			bool res = (bool)Run<B_Constant_R4>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R8
		/// <summary>
		/// Tests support for the ceq IL operation for R8 operands.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0.0, 0.0)]
		[Row(true, 1.0, 1.0)]
		[Row(true, Double.MinValue, Double.MinValue)]
		[Row(true, Double.MaxValue, Double.MaxValue)]
		[Row(false, 0.0, Double.MinValue)]
		[Row(false, 0.0, Double.MaxValue)]
		[Row(false, 0.0, 1.0)]
		[Row(false, Double.MinValue, 0.0)]
		[Row(false, Double.MaxValue, 0.0)]
		[Row(false, 1.0, 0.0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqR8(bool result, double a, double b)
		{
			CodeSource = s_testCode.Replace("t1", "double").Replace("t2", "double");
			bool res = (bool)Run<B_R8_R8>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for R8 operands with right value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR8Right(bool result, double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		/// <summary>
		/// Tests support for the ceq IL operation for R8 operands with left value constant.
		/// </summary>
		/// <param name="result">The expected return value from the compiled code.</param>
		/// <param name="a">The first value to compare.</param>
		/// <param name="b">The second value to compare.</param>
		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR8Left(bool result, double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			bool res = (bool)Run<B_Constant_R8>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion
	}
}
