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
	public class NotFixture : CodeDomTestRunner
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
		[Test, Author("boddlnagg", "boddlnagg@googlemail.com")]
		public void NotC(char a)
		{
			CodeSource = CreateTestCode("NotC", "char", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotC", (int)~a, a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotI1(sbyte a)
		{
			CodeSource = CreateTestCode("NotI1", "sbyte", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotI1", (sbyte)~a, (sbyte)a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotU1(byte a)
		{
			CodeSource = CreateTestCode("NotU1", "byte", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotU1", (~a), a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotI2(short a)
		{
			CodeSource = CreateTestCode("NotI2", "short", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotI2", (short)~a, (short)a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotU2(ushort a)
		{
			CodeSource = CreateTestCode("NotU2", "ushort", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotU2", (~a), a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotI4(int a)
		{
			CodeSource = CreateTestCode("NotI4", "int", "int");
			Assert.IsTrue(Run<bool>("", "Test", "NotI4", (int)~a, (int)a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotU4(uint a)
		{
			CodeSource = CreateTestCode("NotU4", "uint", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "NotU4", ~(uint)a, a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotI8(long a)
		{
			CodeSource = CreateTestCode("NotI8", "long", "long");
			Assert.IsTrue(Run<bool>("", "Test", "NotI8", (long)~a, (long)a));
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
		[Test, Author("rootnode", "rootnode@mosa-project.org")]
		public void NotU8(ulong a)
		{
			CodeSource = CreateTestCode("NotU8", "ulong", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "NotU8", ~(ulong)a, a));
		}
		#endregion
	}
}
