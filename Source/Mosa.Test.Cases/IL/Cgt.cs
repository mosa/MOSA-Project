/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class Cgt : TestCompilerAdapter
	{
		private static string testCode = @"
			static class Test {
				public static bool Cgt(#t1 a, #t2 b) {
					return (a > b);
				}
			}
		";

		private static string CreateConstantTestCode(string typeIn, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool CgtConstant(" + typeIn + @" x)
						{
							return (" + constLeft + @" > x);
						}
					}";
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static bool CgtConstant(" + typeIn + @" x)
						{
							return (x > " + constRight + @");
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
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test]
		public void CgtC(char a, char b)
		{
			settings.CodeSource = testCode.Replace("#t1", "char").Replace("#t2", "char");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void CgtConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void CgtConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region I1
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(2, 1)]
		[Row(SByte.MinValue, SByte.MinValue + 10)]
		[Row(SByte.MaxValue, SByte.MaxValue)]
		[Row(0, SByte.MinValue)]
		[Row(0, SByte.MaxValue)]
		[Row(0, 1)]
		[Row(SByte.MinValue, 0)]
		[Row(SByte.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtI1(sbyte a, sbyte b)
		{
			settings.CodeSource = testCode.Replace("#t1", "sbyte").Replace("#t2", "sbyte");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CgtConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CgtConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region I2
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(2, 1)]
		[Row(Int16.MinValue, Int16.MinValue + 10)]
		[Row(Int16.MaxValue, Int16.MaxValue)]
		[Row(0, Int16.MinValue)]
		[Row(0, Int16.MaxValue)]
		[Row(0, 1)]
		[Row(Int16.MinValue, 0)]
		[Row(Int16.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtI2(short a, short b)
		{
			settings.CodeSource = testCode.Replace("#t1", "short").Replace("#t2", "short");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void CgtConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void CgtConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region I4
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(2, 1)]
		[Row(Int32.MinValue, Int32.MinValue + 10)]
		[Row(Int32.MaxValue, Int32.MaxValue)]
		[Row(0, Int32.MinValue)]
		[Row(0, Int32.MaxValue)]
		[Row(0, 1)]
		[Row(Int32.MinValue, 0)]
		[Row(Int32.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtI4(int a, int b)
		{
			settings.CodeSource = testCode.Replace("#t1", "int").Replace("#t2", "int");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void CgtConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void CgtConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region I8
		
		[Row(0L, 0L)]
		[Row(1L, 1L)]
		[Row(Int64.MinValue, Int64.MinValue + 10)]
		[Row(Int64.MaxValue, Int64.MaxValue)]
		[Row(0L, Int64.MinValue)]
		[Row(0L, Int64.MaxValue)]
		[Row(0L, 1L)]
		[Row(Int64.MinValue, 0L)]
		[Row(Int64.MaxValue, 0L)]
		[Row(1L, 0L)]
		[Test, Importance(Importance.Critical)]
		public void CgtI8(long a, long b)
		{
			settings.CodeSource = testCode.Replace("#t1", "long").Replace("#t2", "long");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue + 1, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Row(12377513, 1237751)]
		[Row(42, 17)]
		[Row(long.MaxValue, long.MinValue)]
		[Test]
		public void CgtConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue + 1, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Row(12377513, 1237751)]
		[Row(42, 17)]
		[Row(long.MaxValue, long.MinValue)]
		[Test]
		public void CgtConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region U1
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(Byte.MinValue, Byte.MinValue + 10)]
		[Row(Byte.MaxValue, Byte.MaxValue)]
		[Row(1, Byte.MinValue)]
		[Row(0, Byte.MaxValue)]
		[Row(0, 1)]
		[Row(Byte.MinValue, 1)]
		[Row(Byte.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtU1(byte a, byte b)
		{
			settings.CodeSource = testCode.Replace("#t1", "byte").Replace("#t2", "byte");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void CgtConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void CgtConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region U2
	
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(UInt16.MinValue, UInt16.MinValue + 10)]
		[Row(UInt16.MaxValue, UInt16.MaxValue)]
		[Row(1, UInt16.MinValue)]
		[Row(0, UInt16.MaxValue)]
		[Row(0, 1)]
		[Row(UInt16.MinValue, 2)]
		[Row(UInt16.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtU2(ushort a, ushort b)
		{
			settings.CodeSource = testCode.Replace("#t1", "ushort").Replace("#t2", "ushort");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CgtConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CgtConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region U4
	
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(UInt32.MinValue, UInt32.MinValue + 10)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(1, UInt32.MinValue)]
		[Row(0, UInt32.MaxValue)]
		[Row(3, 1)]
		[Row(UInt32.MinValue, 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtU4(uint a, uint b)
		{
			settings.CodeSource = testCode.Replace("#t1", "uint").Replace("#t2", "uint");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void CgtConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void CgtConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region U8
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(UInt64.MinValue, UInt64.MinValue + 10)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(1, UInt64.MinValue)]
		[Row(0, UInt64.MaxValue)]
		[Row(3, 2)]
		[Row(UInt64.MinValue, 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Importance(Importance.Critical)]
		public void CgtU8(ulong a, ulong b)
		{
			settings.CodeSource = testCode.Replace("#t1", "ulong").Replace("#t2", "ulong");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CgtConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CgtConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region R4
	
		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MinValue, Single.MinValue + 10)]
		[Row(Single.MaxValue, Single.MaxValue)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(3.0f, 1.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(1.0f, 0.0f)]
		[Test, Importance(Importance.Critical)]
		public void CgtR4(float a, float b)
		{
			settings.CodeSource = testCode.Replace("#t1", "float").Replace("#t2", "float");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MinValue, Single.MinValue + 10)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(3.0f, 1.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(1.0f, 0.0f)]
		[Test]
		public void CgtConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0f, 0f)]
		[Row(-17f, 42.5f)]
		[Row(float.MaxValue, float.MaxValue)]
		[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void CgtConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion

		#region R8
	
		[Row(0.0, 0.5)]
		[Row(1.0, 2.0)]
		[Row(Double.MinValue, Double.MinValue + 1)]
		[Row(Double.MaxValue - 1, Double.MaxValue)]
		[Row(0.0, Double.MinValue)]
		[Row(0.0, Double.MaxValue)]
		[Row(3.0, 1.0)]
		[Row(Double.MinValue, 0.0)]
		[Row(Double.MaxValue, 0.0)]
		[Row(1.0, 0.0)]
		[Test, Importance(Importance.Critical)]
		public void CgtR8(double a, double b)
		{
			settings.CodeSource = testCode.Replace("#t1", "double").Replace("#t2", "double");
			bool res = Run<bool>(string.Empty, @"Test", @"Cgt", a, b);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CgtConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", a);
			Assert.IsTrue((a > b) == res);
		}

		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CgtConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CgtConstant", b);
			Assert.IsTrue((a > b) == res);
		}
		#endregion
	}
}
