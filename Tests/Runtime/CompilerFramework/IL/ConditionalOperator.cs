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
	/// <summary>
	/// Provides test cases for the conditional operator IL operation.
	/// </summary>
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
                }" + Code.ObjectClassDefinition;
		}

		delegate bool I1(sbyte expect, sbyte a, sbyte b, sbyte c, sbyte d);
		delegate bool I2(short expect, short a, short b, short c, short d);
		delegate bool I4(int expect, int a, int b, int c, int d);
		delegate bool I8(long expect, long a, long b, long c, long d);

		delegate bool U1(byte expect, byte a, byte b, byte c, byte d);
		delegate bool U2(ushort expect, ushort a, ushort b, ushort c, ushort d);
		delegate bool U4(uint expect, uint a, uint b, uint c, uint d);
		delegate bool U8(ulong expect, ulong a, ulong b, ulong c, ulong d);

		/// <summary>
		/// Tests the ConditionalOperator_EQ_U1 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U1(byte a, byte b, byte c, byte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U1", "==", "byte", "byte");
			Assert.IsTrue((bool)Run<U1>("", "Test", "ConditionalOperator_EQ_U1", ((a == b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_NEQ_U1 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U1(byte a, byte b, byte c, byte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U1", "!=", "byte", "byte");
			Assert.IsTrue((bool)Run<U1>("", "Test", "ConditionalOperator_NEQ_U1", ((a != b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_EQ_I1 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I1", "==", "sbyte", "sbyte");
			Assert.IsTrue((bool)Run<I1>("", "Test", "ConditionalOperator_EQ_I1", ((a == b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_NEQ_I1 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I1", "!=", "sbyte", "sbyte");
			Assert.IsTrue((bool)Run<I1>("", "Test", "ConditionalOperator_NEQ_I1", ((a != b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_EQ_U2 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_U2", "==", "ushort", "ushort");
			Assert.IsTrue((bool)Run<U2>("", "Test", "ConditionalOperator_EQ_U2", ((a == b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_NEQ_U2 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_U2", "!=", "ushort", "ushort");
			Assert.IsTrue((bool)Run<U2>("", "Test", "ConditionalOperator_NEQ_U2", ((a != b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_EQ_I2 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I2(short a, short b, short c, short d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I2", "==", "short", "short");
			Assert.IsTrue((bool)Run<I2>("", "Test", "ConditionalOperator_EQ_I2", ((a == b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_NEQ_I2 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I2(short a, short b, short c, short d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I2", "!=", "short", "short");
			Assert.IsTrue((bool)Run<I2>("", "Test", "ConditionalOperator_NEQ_I2", ((a != b) ? c : d), a, b, c, d));
		}


		/// <summary>
		/// Tests the ConditionalOperator_EQ_I4 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_EQ_I4(int a, int b, int c, int d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_EQ_I4", "==", "int", "int");
			Assert.IsTrue((bool)Run<I4>("", "Test", "ConditionalOperator_EQ_I4", ((a == b) ? c : d), a, b, c, d));
		}

		/// <summary>
		/// Tests the ConditionalOperator_NEQ_I4 for the sbyte type.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Test, Author(@"Phil Garcia", @"phil@thinkedge.com")]
		public void ConditionalOperator_NEQ_I4(int a, int b, int c, int d)
		{
			CodeSource = CreateTestCode("ConditionalOperator_NEQ_I4", "!=", "int", "int");
			Assert.IsTrue((bool)Run<I4>("", "Test", "ConditionalOperator_NEQ_I4", ((a != b) ? c : d), a, b, c, d));
		}
	}
}
