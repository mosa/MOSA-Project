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
using System.Globalization;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.Collection;
using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class ConditionalOperator : TestCompilerAdapter
	{

		public ConditionalOperator()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)25, (sbyte)25, (sbyte)3, (sbyte)2)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Row((sbyte)-1, (sbyte)0, (sbyte)-1, (sbyte)0)]
		[Test]
		public void EQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_I1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_I1", a, b, c, d));
		}

		[Row((sbyte)2, (sbyte)2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)1, (sbyte)0, (sbyte)1, (sbyte)0)]
		[Row((sbyte)-2, (sbyte)-2, (sbyte)0, (sbyte)1)]
		[Row((sbyte)-1, (sbyte)0, (sbyte)-1, (sbyte)0)]
		[Test]
		public void NEQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_I1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_I1", a, b, c, d));
		}

		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Row((byte)255, (byte)0, (byte)255, (byte)0)]
		[Test]
		public void EQ_U1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_U1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_U1", a, b, c, d));
		}

		[Row((byte)2, (byte)2, (byte)0, (byte)1)]
		[Row((byte)1, (byte)0, (byte)1, (byte)0)]
		[Row((byte)255, (byte)0, (byte)255, (byte)0)]
		[Test]
		public void NEQ_U1(byte a, byte b, byte c, byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_U1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_U1", a, b, c, d));
		}

		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test]
		public void EQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_U2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_U2", a, b, c, d));
		}

		[Row((ushort)2, (ushort)2, (ushort)0, (ushort)1)]
		[Row((ushort)1, (ushort)0, (ushort)1, (ushort)0)]
		[Test]
		public void NEQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_U2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_U2", a, b, c, d));
		}

		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Test]
		public void EQ_I2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_I2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_I2", a, b, c, d));
		}

		[Row((short)2, (short)2, (short)0, (short)1)]
		[Row((short)1, (short)0, (short)1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Row((short)-1, (short)0, (short)-1, (short)0)]
		[Test]
		public void NEQ_I2(short a, short b, short c, short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_I2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_I2", a, b, c, d));
		}

		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Test]
		public void EQ_I4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_I4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_I4", a, b, c, d));
		}

		[Row((int)2, (int)2, (int)0, (int)1)]
		[Row((int)1, (int)0, (int)1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Row((int)-1, (int)0, (int)-1, (int)0)]
		[Test]
		public void NEQ_I4(int a, int b, int c, int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_I4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_I4", a, b, c, d));
		}

		[Row((uint)2, (uint)2, (uint)0, (uint)1)]
		[Row((uint)1, (uint)0, (uint)1, (uint)0)]
		[Test]
		public void EQ_U4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_U4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_U4", a, b, c, d));
		}

		[Row((uint)2, (uint)2, (uint)0, (uint)1)]
		[Row((uint)1, (uint)0, (uint)1, (uint)0)]
		[Test]
		public void NEQ_U4(uint a, uint b, uint c, uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_U4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_U4", a, b, c, d));
		}

		[Row((long)2, (long)2, (long)0, (long)1)]
		[Row((long)1, (long)0, (long)1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Test]
		public void EQ_I8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_I8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_I8", a, b, c, d));
		}

		[Row((long)2, (long)2, (long)0, (long)1)]
		[Row((long)1, (long)0, (long)1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Row((long)-1, (long)0, (long)-1, (long)0)]
		[Test]
		public void NEQ_I8(long a, long b, long c, long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_I8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_I8", a, b, c, d));
		}

		[Row((ulong)2, (ulong)2, (ulong)0, (ulong)1)]
		[Row((ulong)1, (ulong)0, (ulong)1, (ulong)0)]
		[Test]
		public void EQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.EQ_U8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "EQ_U8", a, b, c, d));
		}

		[Row((ulong)2, (ulong)2, (ulong)0, (ulong)1)]
		[Row((ulong)1, (ulong)0, (ulong)1, (ulong)0)]
		[Test]
		public void NEQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.NEQ_U8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "NEQ_U8", a, b, c, d));
		}
	}
}
