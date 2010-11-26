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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	[TestFixture]
	public class Call : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string type)
		{
			return @"
				static class Test {
					static bool " + name + "(" + type + " value) { return value == " + name + @"_Target(value); } 
					static " + type + " " + name + "_Target(" + type + @" value) { return value; }
				}" + Code.AllTestCode;
		}

		private static string CreateConstantTestCode(string name, string type, string constant)
		{
			return @"
				static class Test {
					static bool " + name + "(" + type + " value) { return value == " + name + "_Target(" + constant + @"); } 
					static " + type + " " + name + "_Target(" + type + @" value) { return value; }
				}" + Code.AllTestCode;
		}

		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallEmpty()
		{
			CodeSource = @"
				static class Test { 
					static void CallEmpty() { CallEmpty_Target(); } 
					static void CallEmpty_Target() { }
				}" + Code.AllTestCode;
			Run<object>("", "Test", "CallEmpty");
		}

		#region B
		
		[Row(true)]
		[Row(false)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallB(bool value)
		{
			CodeSource = CreateTestCode("CallB", "bool");
			Assert.IsTrue(Run<bool>("", "Test", "CallB", value));
		}

		[Row(true)]
		[Row(false)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantB(bool value)
		{
			CodeSource = CreateConstantTestCode("CallConstantB", "bool", value.ToString().ToLower());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantB", value));
		}
		#endregion

		#region C
		
		[Row(0)]
		[Row('a')]
		[Row('Z')]
		[Row(char.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallC(char value)
		{
			CodeSource = CreateTestCode("CallC", "char");
			Assert.IsTrue(Run<bool>("", "Test", "CallC", value));
		}

		[Row('a')]
		[Row('Z')]
		[Row('-')]
		[Row('.')]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantC(char value)
		{
			CodeSource = CreateConstantTestCode("CallConstantC", "char", "'" + value.ToString() + "'");
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantC", value));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI1(sbyte value)
		{
			CodeSource = CreateTestCode("CallI1", "sbyte");
			Assert.IsTrue(Run<bool>("", "Test", "CallI1", value));
		}

		[Row(0)]
		[Row(-48)]
		[Row(sbyte.MinValue)]
		[Row(sbyte.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI1(sbyte value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI1", "sbyte", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantI1", value));
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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU1(byte value)
		{
			CodeSource = CreateTestCode("CallU1", "byte");
			Assert.IsTrue(Run<bool>("", "Test", "CallU1", value));
		}

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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU1(byte value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU1", "byte", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantU1", value));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI2(short value)
		{
			CodeSource = CreateTestCode("CallI2", "short");
			Assert.IsTrue(Run<bool>("", "Test", "CallI2", value));
		}

		[Row(0)]
		[Row(-48)]
		[Row(short.MinValue)]
		[Row(short.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI2(short value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI2", "short", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantI2", value));
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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU2(ushort value)
		{
			CodeSource = CreateTestCode("CallU2", "ushort");
			Assert.IsTrue(Run<bool>("", "Test", "CallU2", value));
		}

		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ushort.MinValue)]
		[Row(ushort.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU2(ushort value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU2", "ushort", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantU2", value));
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
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI4(int value)
		{
			CodeSource = CreateTestCode("CallI4", "int");
			Assert.IsTrue(Run<bool>("", "Test", "CallI4", value));
		}

		[Row(0)]
		[Row(-48)]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI4(int value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI4", "int", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantI4", value));
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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU4(uint value)
		{
			CodeSource = CreateTestCode("CallU4", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "CallU4", value));
		}

		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(uint.MinValue)]
		[Row(uint.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU4(uint value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU4", "uint", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantU4", value));
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
		[Row(long.MinValue)]
		[Row(long.MaxValue)]
		[Test, Author("alyman", "mail.alex.lyman@gmail.com")]
		public void CallI8(long value)
		{
			CodeSource = CreateTestCode("CallI8", "long");
			Assert.IsTrue(Run<bool>("", "Test", "CallI8", value));
		}

		[Row(0)]
		[Row(-48)]
		[Row(long.MinValue)]
		[Row(long.MaxValue)]
		[Test, Author("boddlnagg", "kpreisert@googlemail.com")]
		public void CallConstantI8(long value)
		{
			CodeSource = CreateConstantTestCode("CallConstantI8", "long", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantI8", value));
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
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallU8(ulong value)
		{
			CodeSource = CreateTestCode("CallU8", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "CallU8", value));
		}

		[Row(0)]
		[Row(1)]
		[Row(2)]
		[Row(5)]
		[Row(10)]
		[Row(11)]
		[Row(100)]
		[Row(ulong.MinValue)]
		[Row(ulong.MaxValue)]
		[Test, Author("tgiphil", "phil@thinkedge.com")]
		public void CallConstantU8(ulong value)
		{
			CodeSource = CreateConstantTestCode("CallConstantU8", "ulong", value.ToString());
			Assert.IsTrue(Run<bool>("", "Test", "CallConstantU8", value));
		}
		#endregion

		[Test, Author(@"grover", @"sharpos@michaelruck.de")]
		public void CallOrderI4()
		{
			CodeSource = @"
				static class Test {
					static bool CallOrderI4(int a, int b, int c, int d) {
						return (a == 1 && b == 2 && c == 3 && d == 4);
					}
				}
			" + Code.AllTestCode;

			Assert.IsTrue(Run<bool>(@"", @"Test", @"CallOrderI4", 1, 2, 3, 4));
		}

		[Test, Author(@"tgiphil", @"phil@thinkedge.com")]
		public void CallOrderU8()
		{
			CodeSource = @"
				static class Test {
					static bool CallOrderU8(ulong a, ulong b, ulong c, ulong d) {
						return (a == 1 && b == 2 && c == 3 && d == 4);
					}
				}
			" + Code.AllTestCode;

			Assert.IsTrue(Run<bool>(@"", @"Test", @"CallOrderU8", (ulong)1, (ulong)2, (ulong)3, (ulong)4));
		}

		[Test, Author(@"tgiphil", @"phil@thinkedge.com")]
		public void CallOrderU4_U8_U8_U8()
		{
			CodeSource = @"
				static class Test {
					static bool CallOrderU4_U8_U8_U8(uint a, ulong b, ulong c, ulong d) {
						return (a == 1 && b == 2 && c == 3 && d == 4);
					}
				}
			" + Code.AllTestCode;

			Assert.IsTrue(Run<bool>(@"", @"Test", @"CallOrderU4_U8_U8_U8", (uint)1, (ulong)2, (ulong)3, (ulong)4));
		}

	}
}
