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

namespace Mosa.Test.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class Ceq : CodeDomTestRunner
	{
		private static string testCode = @"
			static class Test {
				public static bool Ceq(#t1 a, #t2 b) {
					return (a == b);
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
						static bool CeqConstant(" + typeIn + @" x)
						{
							return (" + constLeft + @" == x);
						}
					}" + Code.AllTestCode;
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
					}" + Code.AllTestCode;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region B
		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author(@"Michael Ruck"), Importance(Importance.Critical)]
		public void CeqB(bool result, bool a, bool b)
		{
			CodeSource = testCode.Replace("#t1", "bool").Replace("#t2", "bool");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author("boddlnagg")]
		public void CeqConstantBRight(bool result, bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("bool", null, b.ToString().ToLower());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test, Author("boddlnagg")]
		public void CeqConstantBLeft(bool result, bool a, bool b)
		{
			CodeSource = CreateConstantTestCode("bool", a.ToString().ToLower(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region C
	
		[Row(true, 0, 0)]
		[Row(false, 'a', 'Z')]
		[Row(true, 'a', 'a')]
		[Row(false, 0, 128)]
		[Test, Author("boddlnagg")]
		public void CeqC(bool result, char a, char b)
		{
			CodeSource = testCode.Replace("#t1", "char").Replace("#t2", "char");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(false, 0, 'a')]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test, Author("boddlnagg")]
		public void CeqConstantCRight(bool result, char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(false, 'a', 0)]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test, Author("boddlnagg")]
		public void CeqConstantCLeft(bool result, char a, char b)
		{
			CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I1
		
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
			CodeSource = testCode.Replace("#t1", "sbyte").Replace("#t2", "sbyte");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI1Right(bool result, sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI1Left(bool result, sbyte a, sbyte b)
		{
			CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I2
		
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
			CodeSource = testCode.Replace("#t1", "short").Replace("#t2", "short");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI2Right(bool result, short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI2Left(bool result, short a, short b)
		{
			CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I4
		
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
			CodeSource = testCode.Replace("#t1", "int").Replace("#t2", "int");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI4Right(bool result, int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI4Left(bool result, int a, int b)
		{
			CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I8
		
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
			CodeSource = testCode.Replace("#t1", "long").Replace("#t2", "long");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI8Right(bool result, long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantI8Left(bool result, long a, long b)
		{
			CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U1
		
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
			CodeSource = testCode.Replace("#t1", "byte").Replace("#t2", "byte");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU1Right(bool result, byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU1Left(bool result, byte a, byte b)
		{
			CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U2
	
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
			CodeSource = testCode.Replace("#t1", "ushort").Replace("#t2", "ushort");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU2Right(bool result, ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

	
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU2Left(bool result, ushort a, ushort b)
		{
			CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U4
		
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
			CodeSource = testCode.Replace("#t1", "uint").Replace("#t2", "uint");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU4Right(bool result, uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU4Left(bool result, uint a, uint b)
		{
			CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U8
		
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
			CodeSource = testCode.Replace("#t1", "ulong").Replace("#t2", "ulong");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU8Right(bool result, ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test, Author("boddlnagg")]
		public void CeqConstantU8Left(bool result, ulong a, ulong b)
		{
			CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R4
		
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
			CodeSource = testCode.Replace("#t1", "float").Replace("#t2", "float");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

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
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

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
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R8
		
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
			CodeSource = testCode.Replace("#t1", "double").Replace("#t2", "double");
			bool res = Run<bool>(@"", @"Test", @"Ceq", a, b);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR8Right(bool result, double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test, Author("boddlnagg")]
		public void CeqConstantR8Left(bool result, double a, double b)
		{
			CodeSource = CreateConstantTestCode("double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			bool res = Run<bool>(@"", @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion
	}
}
