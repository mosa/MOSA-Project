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
	public class NotFixture : TestCompilerAdapter
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + @" a)
					{
						return expect == (~a);
					}
				}" + Code.AllTestCode;
		}

		#region C

		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row('a')]
		[Row('Z')]
		[Row(100)]
		[Row(char.MaxValue)]
		[Test]
		public void NotC(char a)
		{
			compiler.CodeSource = CreateTestCode("NotC", "char", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotC", (int)~a, a));
		}
		#endregion

		#region I1
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test]
		public void NotI1(sbyte a)
		{
			compiler.CodeSource = CreateTestCode("NotI1", "sbyte", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotI1", (sbyte)~a, (sbyte)a));
		}
		#endregion

		#region U1
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(byte.MinValue)]
		[Row(byte.MaxValue)]
		[Test]
		public void NotU1(byte a)
		{
			compiler.CodeSource = CreateTestCode("NotU1", "byte", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotU1", (~a), a));
		}
		#endregion

		#region I2
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test]
		public void NotI2(short a)
		{
			compiler.CodeSource = CreateTestCode("NotI2", "short", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotI2", (short)~a, (short)a));
		}
		#endregion

		#region U2
	
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test]
		public void NotU2(ushort a)
		{
			compiler.CodeSource = CreateTestCode("NotU2", "ushort", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotU2", (~a), a));
		}
		#endregion

		#region I4
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Test]
		public void NotI4(int a)
		{
			compiler.CodeSource = CreateTestCode("NotI4", "int", "int");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotI4", (int)~a, (int)a));
		}
		#endregion

		#region U4
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(uint.MinValue)]
		[Row(uint.MaxValue)]
		[Test]
		public void NotU4(uint a)
		{
			compiler.CodeSource = CreateTestCode("NotU4", "uint", "uint");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotU4", ~(uint)a, a));
		}
		#endregion

		#region I8
		
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(-0)]
		[Row(-1)]
		[Row(-2)]
		[Row(-5)]
		[Row(-10)]
		[Row(-11)]
		[Row(-100)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test]
		public void NotI8(long a)
		{
			compiler.CodeSource = CreateTestCode("NotI8", "long", "long");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotI8", (long)~a, (long)a));
		}
		#endregion

		#region U8
	
		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ulong.MinValue)]
		[Row(ulong.MaxValue)]
		[Test]
		public void NotU8(ulong a)
		{
			compiler.CodeSource = CreateTestCode("NotU8", "ulong", "ulong");
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "NotU8", ~(ulong)a, a));
		}
		#endregion
	}
}
