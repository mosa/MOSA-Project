/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
	[TestFixture]
	class ConditionalOperator : CodeDomTestRunner
	{
		private static string CreateTestCode(string name, string condition, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b, " + typeIn + @" c, " + typeIn + @" d)
					{
						return expect == ((a " + condition + @" b) ? c : d);
					}
				}" + Code.AllTestCode;
		}

		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Row((byte)255, (byte)0, (byte)255, (byte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U1(byte a, byte b, byte c, byte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U1", "==", "byte", "byte");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_U1", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Row((byte)255, (byte)0, (byte)255, (byte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U1(byte a, byte b, byte c, byte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U1", "!=", "byte", "byte");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_U1", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Row((sbyte)-2, (sbyte)-2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)-1, (sbyte)0, (sbyte)-1, (sbyte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I1", "==", "sbyte", "sbyte");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_I1", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Row((sbyte)-2, (sbyte)-2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)-1, (sbyte)0, (sbyte)-1, (sbyte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I1", "!=", "sbyte", "sbyte");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_I1", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U2", "==", "ushort", "ushort");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_U2", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U2", "!=", "ushort", "ushort");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_U2", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I2(short a, short b, short c, short d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I2", "==", "short", "short");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_I2", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I2(short a, short b, short c, short d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I2", "!=", "short", "short");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_I2", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I4(int a, int b, int c, int d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I4", "==", "int", "int");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_I4", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I4(int a, int b, int c, int d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I4", "!=", "int", "int");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_I4", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((uint)2, (uint)2, (uint)0, (uint)1)]
		[Row((uint)1, (uint)0, (uint)1, (uint)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U4(uint a, uint b, uint c, uint d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U4", "==", "uint", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_U4", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((uint)2, (uint)2, (uint)0, (uint)1)]
		[Row((uint)1, (uint)0, (uint)1, (uint)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U4(uint a, uint b, uint c, uint d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U4", "!=", "uint", "uint");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_U4", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((long)2, (long)2, (long)0, (long)1)]
		[Row((long)1, (long)0, (long)1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I8(long a, long b, long c, long d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I8", "==", "long", "long");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_I8", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((long)2, (long)2, (long)0, (long)1)]
		[Row((long)1, (long)0, (long)1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I8(long a, long b, long c, long d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I8", "!=", "long", "long");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_I8", ((a != b) ? c : d), a, b, c, d));
		}

		[Row((ulong)2, (ulong)2, (ulong)0, (ulong)1)]
		[Row((ulong)1, (ulong)0, (ulong)1, (ulong)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U8", "==", "ulong", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_EQ_U8", ((a == b) ? c : d), a, b, c, d));
		}

		[Row((ulong)2, (ulong)2, (ulong)0, (ulong)1)]
		[Row((ulong)1, (ulong)0, (ulong)1, (ulong)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U8", "!=", "ulong", "ulong");
			Assert.IsTrue(Run<bool>("", "Test", "ConditionalOperator_NEQ_U8", ((a != b) ? c : d), a, b, c, d));
		}
	}
}
