/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  
 */

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public class CallWithConstant : TestCompilerAdapter
	{

		private static string CreateConstantTestCode(string name, string type, string constant)
		{
			return @"
				static class Test {
					static bool " + name + "(" + type + " value) { return value == " + name + "_Target(" + constant + @"); } 
					static " + type + " " + name + "_Target(" + type + @" value) { return value; }
				}";
		}

		#region B
		
		[Row(true)]
		[Row(false)]
		[Test]
		public void CallConstantB(bool value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantB", "bool", value.ToString().ToLower());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantB", value));
		}
		#endregion

		#region C
		
		[Row('a')]
		[Row('Z')]
		[Row('-')]
		[Row('.')]
		[Test]
		public void CallConstantC(char value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantC", "char", "'" + value.ToString() + "'");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantC", value));
		}
		#endregion

		#region I1
		
		[Row(0)]
		[Row(-48)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test]
		public void CallConstantI1(sbyte value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantI1", "sbyte", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantI1", value));
		}
		#endregion

		#region U1
		
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(127)]
		[Row(128)]
		[Row(byte.MinValue)]
		[Row(byte.MaxValue)]
		[Test]
		public void CallConstantU1(byte value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantU1", "byte", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantU1", value));
		}
		#endregion

		#region I2
		
		[Row(0)]
		[Row(-48)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test]
		public void CallConstantI2(short value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantI2", "short", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantI2", value));
		}
		#endregion

		#region U2
		
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test]
		public void CallConstantU2(ushort value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantU2", "ushort", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantU2", value));
		}
		#endregion

		#region I4
	
		[Row(0)]
		[Row(-48)]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Test]
		public void CallConstantI4(int value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantI4", "int", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantI4", value));
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
		public void CallConstantU4(uint value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantU4", "uint", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantU4", value));
		}
		#endregion

		#region I8
		
		[Row(0)]
		[Row(-48)]
		[Row(long.MinValue)]
		[Row(long.MaxValue)]
		[Test]
		public void CallConstantI8(long value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantI8", "long", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantI8", value));
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
		public void CallConstantU8(ulong value)
		{
			settings.CodeSource = CreateConstantTestCode("CallConstantU8", "ulong", value.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "CallConstantU8", value));
		}
		#endregion

	}
}
