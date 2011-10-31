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
using System.Globalization;

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class Clt : TestCompilerAdapter
	{

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
					}";
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
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C

		[Row(0, 'a')]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void CltConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", null, "'" + b.ToString() + "'");
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row('a', 0)]
		[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void CltConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCode("char", "'" + a.ToString() + "'", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I1
		
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CltConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

	
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(sbyte.MinValue, sbyte.MinValue)]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void CltConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("sbyte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I2
		
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void CltConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(short.MinValue, short.MinValue)]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void CltConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("short", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I4
		
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void CltConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(int.MinValue, int.MinValue)]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void CltConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("int", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region I8
		
		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void CltConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(-17, 42)]
		[Row(long.MinValue, long.MinValue)]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void CltConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("long", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U1
	
		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void CltConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(byte.MaxValue, byte.MaxValue)]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void CltConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("byte", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U2
		
		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CltConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ushort.MaxValue, ushort.MaxValue)]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void CltConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("ushort", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U4
	
		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void CltConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(uint.MaxValue, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void CltConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("uint", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region U8
	
		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CltConstantU8Right(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", null, b.ToString());
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0, 0)]
		[Row(17, 142)]
		[Row(ulong.MaxValue, ulong.MaxValue)]
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Test]
		public void CltConstantU8Left(ulong a, ulong b)
		{
			settings.CodeSource = CreateConstantTestCode("ulong", a.ToString(), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region R4
	
		[Row(0.0f, 0.0f)]
		[Row(1.0f, 1.0f)]
		[Row(Single.MaxValue - 10.5f, Single.MaxValue)]
		[Row(0.0f, Single.MinValue)]
		[Row(0.0f, Single.MaxValue)]
		[Row(1.0f, 3.0f)]
		[Row(Single.MinValue, 0.0f)]
		[Row(Single.MaxValue, 0.0f)]
		[Row(0.0f, 1.0f)]
		[Test]
		public void CltConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", null, b.ToString(CultureInfo.InvariantCulture) + "f");
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
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
		[Test]
		public void CltConstantR4Left(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("float", a.ToString(CultureInfo.InvariantCulture) + "f", null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion

		#region R8
		
		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CltConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", null, b.ToString(CultureInfo.InvariantCulture));
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", a);
			Assert.IsTrue((a < b) == res);
		}

		[Row(0.0, 0.0)]
		[Row(-17.0, 42.5)]
		[Row(1.79769313486231E+308, 1.79769313486231E+308)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void CltConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("double", a.ToString(CultureInfo.InvariantCulture), null);
			bool res = Run<bool>(string.Empty, @"Test", @"CltConstant", b);
			Assert.IsTrue((a < b) == res);
		}
		#endregion
	}
}
