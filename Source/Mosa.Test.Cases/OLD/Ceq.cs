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
using System.Globalization;

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Ceq : TestCompilerAdapter
	{

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
					}";
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
					}";
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
		[Test]
		public void CeqConstantBRight(bool result, bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("bool", null, b.ToString().ToLower());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, true, true)]
		[Row(true, false, false)]
		[Row(false, true, false)]
		[Row(false, false, true)]
		[Test]
		public void CeqConstantBLeft(bool result, bool a, bool b)
		{
			settings.CodeSource = CreateConstantTestCode("bool", a.ToString().ToLower(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region C
	
		[Row(false, 0, 'a')]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test]
		public void CeqConstantCRight(bool result, char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(false, 'a', 0)]
		[Row(false, '-', '.')]
		[Row(true, 'a', 'a')]
		[Test]
		public void CeqConstantCLeft(bool result, char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I1
		
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CeqConstantI1Right(bool result, sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, sbyte.MinValue, sbyte.MinValue)]
		[Row(false, sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CeqConstantI1Left(bool result, sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I2
		
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test]
		public void CeqConstantI2Right(bool result, short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, short.MinValue, short.MinValue)]
		[Row(false, short.MinValue, short.MaxValue)]
		[Test]
		public void CeqConstantI2Left(bool result, short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I4
		
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test]
		public void CeqConstantI4Right(bool result, int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, int.MinValue, int.MinValue)]
		[Row(false, int.MinValue, int.MaxValue)]
		[Test]
		public void CeqConstantI4Left(bool result, int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region I8
		
		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test]
		public void CeqConstantI8Right(bool result, long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, -17, 42)]
		[Row(true, long.MinValue, long.MinValue)]
		[Row(false, long.MinValue, long.MaxValue)]
		[Test]
		public void CeqConstantI8Left(bool result, long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U1
		
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test]
		public void CeqConstantU1Right(bool result, byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, byte.MaxValue, byte.MaxValue)]
		[Row(false, byte.MinValue, byte.MaxValue)]
		[Test]
		public void CeqConstantU1Left(bool result, byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U2
	
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CeqConstantU2Right(bool result, ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

	
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ushort.MaxValue, ushort.MaxValue)]
		[Row(false, ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CeqConstantU2Left(bool result, ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U4
		
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test]
		public void CeqConstantU4Right(bool result, uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, uint.MaxValue, uint.MaxValue)]
		[Row(false, uint.MinValue, uint.MaxValue)]
		[Test]
		public void CeqConstantU4Left(bool result, uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region U8
		
		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CeqConstantU8Right(bool result, ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0, 0)]
		[Row(false, 17, 142)]
		[Row(true, ulong.MaxValue, ulong.MaxValue)]
		[Row(false, ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CeqConstantU8Left(bool result, ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R4
		
		[Row(true, 0f, 0f)]
		[Row(true, 13.9f, 13.9f)]
		[Row(true, 11.91262f, 11.91262f)]
		[Row(false, 11.91262f, 11.91263f)]
		[Row(false, -17f, 42f)]
		[Row(false, Single.MinValue, Single.MaxValue)]
		[Test]
		public void CeqConstantR4Right(bool result, float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0f, 0f)]
		[Row(true, 13.9f, 13.9f)]
		[Row(true, 11.91262f, 11.91262f)]
		[Row(false, 11.91262f, 11.91263f)]
		[Row(false, -17f, 42f)]
		[Row(false, Single.MinValue, Single.MaxValue)]
		[Test]
		public void CeqConstantR4Left(bool result, float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion

		#region R8
		
		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CeqConstantR8Right(bool result, double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", null, b.ToString(CultureInfo.InvariantCulture));
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", a);
			Assert.IsTrue(result == res);
		}

		[Row(true, 0.0, 0.0)]
		[Row(false, -17.0, 42.5)]
		[Row(true, 1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(false, -1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CeqConstantR8Left(bool result, double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", a.ToString(CultureInfo.InvariantCulture), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CeqConstant", b);
			Assert.IsTrue(result == res);
		}
		#endregion
	}
}
