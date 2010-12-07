/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
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
	public class Clt : CodeDomTestRunner
	{
		private static string s_testCode = @"
			static class Test {
				public static bool Clt(#t1 a, #t2 b) {
					return (a < b);
				}
			}
		" + Code.AllTestCode;

		private static string CreateConstantTestCode(string typeIn, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool CltConstant(" + typeIn + @" x)
						{
							return (" + constLeft + @" < x);
						}
					}" + Code.AllTestCode;
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static bool CltConstant(" + typeIn + @" x)
						{
							return (x < " + constRight + @");
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
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CltC(char a, char b)
		{
			CodeSource = s_testCode.Replace("#t1", "char").Replace("#t2", "char");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CltConstantCRight(char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CltConstantCLeft(char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I1
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(1, 2)]
		[Row(SByte.MinValue, SByte.MinValue + 10)]
		[Row(SByte.MaxValue, SByte.MaxValue)]
		[Row(0, SByte.MinValue)]
		[Row(0, SByte.MaxValue)]
		[Row(0, 1)]
		[Row(SByte.MinValue, 0)]
		[Row(SByte.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltI1(sbyte a, sbyte b)
		{
			CodeSource = s_testCode.Replace("#t1", "sbyte").Replace("#t2", "sbyte");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI1Right(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

	
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI1Left(sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I2
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(Int16.MinValue, Int16.MinValue + 10)]
		[Row(Int16.MaxValue, Int16.MaxValue)]
		[Row(0, Int16.MinValue)]
		[Row(0, Int16.MaxValue)]
		[Row(0, 1)]
		[Row(Int16.MinValue, 0)]
		[Row(Int16.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltI2(short a, short b)
		{
			CodeSource = s_testCode.Replace("#t1", "short").Replace("#t2", "short");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI2Right(short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI2Left(short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I4
		
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(Int32.MinValue, Int32.MinValue + 10)]
		[Row(Int32.MaxValue, Int32.MaxValue)]
		[Row(0, Int32.MinValue)]
		[Row(0, Int32.MaxValue)]
		[Row(0, 1)]
		[Row(Int32.MinValue, 0)]
		[Row(Int32.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltI4(int a, int b)
		{
			CodeSource = s_testCode.Replace("#t1", "int").Replace("#t2", "int");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI4Right(int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI4Left(int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
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
		[Row(-1L, -2L)]
		[Row(-2L, -1L)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltI8(long a, long b)
		{
			CodeSource = s_testCode.Replace("#t1", "long").Replace("#t2", "long");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI8Right(long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantI8Left(long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
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
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltU1(byte a, byte b)
		{
			CodeSource = s_testCode.Replace("#t1", "byte").Replace("#t2", "byte");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU1Right(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU1Left(byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
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
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltU2(ushort a, ushort b)
		{
			CodeSource = s_testCode.Replace("#t1", "ushort").Replace("#t2", "ushort");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU2Right(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU2Left(ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U4
	
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(UInt32.MinValue, UInt32.MinValue + 10)]
		[Row(UInt32.MaxValue, UInt32.MaxValue)]
		[Row(1, UInt32.MinValue)]
		[Row(0, UInt32.MaxValue)]
		[Row(0, 1)]
		[Row(UInt32.MinValue, 1)]
		[Row(UInt32.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltU4(uint a, uint b)
		{
			CodeSource = s_testCode.Replace("#t1", "uint").Replace("#t2", "uint");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU4Right(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU4Left(uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U8
	
		[Row(0, 0)]
		[Row(1, 1)]
		[Row(UInt64.MinValue, UInt64.MinValue + 10)]
		[Row(UInt64.MaxValue, UInt64.MaxValue)]
		[Row(1, UInt64.MinValue)]
		[Row(0, UInt64.MaxValue)]
		[Row(0, 1)]
		[Row(UInt64.MinValue, 1)]
		[Row(UInt64.MaxValue, 0)]
		[Row(1, 0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltU8(ulong a, ulong b)
		{
			CodeSource = s_testCode.Replace("#t1", "ulong").Replace("#t2", "ulong");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU8Right(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CltConstantU8Left(ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region R4
	
		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MinValue, Single.MinValue + 10.0f)]
		[Row(Single.MaxValue - 0.5f, Single.MaxValue)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(1.0f, 3.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(0.0f, 1.0f)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltR4(float a, float b)
		{
			CodeSource = s_testCode.Replace("#t1", "float").Replace("#t2", "float");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MaxValue - 10.5f, Single.MaxValue)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(1.0f, 3.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(0.0f, 1.0f)]
		[Test, Author("boddlnagg")]
		public void CltConstantR4Right(float a, float b)
		{
			CodeSource = CreateConstantTestCode("float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MinValue, Single.MinValue + 10.0f)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(1.0f, 3.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(0.0f, 1.0f)]
		[Test, Author("boddlnagg")]
		public void CltConstantR4Left(float a, float b)
		{
			CodeSource = CreateConstantTestCode("float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region R8
		
		[Row(0.0, 0.5)]
		[Row(1.0, 2.0)]
		[Row(Double.MinValue, Double.MinValue + 1)]
		[Row(Double.MaxValue - 1, Double.MaxValue)]
		[Row(0.0, Double.MinValue)]
		[Row(0.0, Double.MaxValue)]
		[Row(0.0, 1.0)]
		[Row(Double.MinValue, 0.0)]
		[Row(Double.MaxValue, 0.0)]
		[Row(1.0, 0.0)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CltR8(double a, double b)
		{
			CodeSource = s_testCode.Replace("#t1", "double").Replace("#t2", "double");
			bool res = Run<bool>(@"", @"Test", @"Clt", a, b);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CltConstantR8Right(double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			bool res = Run<bool>(@"", @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CltConstantR8Left(double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			bool res = Run<bool>(@"", @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion
	}
}
