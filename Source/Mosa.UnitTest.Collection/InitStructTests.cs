// Copyright (c) MOSA Project. Licensed under the New BSD License.


// No run-time tests

namespace Mosa.UnitTest.Collection
{
	internal struct InitStruct
	{
		public byte ValueU1;
		public ushort ValueU2;
		public uint ValueU4;
		public ulong ValueU8;
		public sbyte ValueI1;
		public short ValueI2;
		public int ValueI4;
		public long ValueI8;
		public float ValueR4;
		public double ValueR8;
		public bool ValueB;
		public char ValueC;
		public object ValueO;
	}

	public static class InitStructTests
	{
		[MosaUnitTest(Series = "U1")]
		public static byte InitStructU1()
		{
			InitStruct d = new InitStruct();
			return d.ValueU1;
		}

		[MosaUnitTest(Series = "U2")]
		public static ushort InitStructU2()
		{
			InitStruct d = new InitStruct();
			return d.ValueU2;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint InitStructU4()
		{
			InitStruct d = new InitStruct();
			return d.ValueU4;
		}

		[MosaUnitTest(Series = "U8")]
		public static ulong InitStructU8()
		{
			InitStruct d = new InitStruct();
			return d.ValueU8;
		}

		[MosaUnitTest(Series = "I1")]
		public static sbyte InitStructI1()
		{
			InitStruct d = new InitStruct();
			return d.ValueI1;
		}

		[MosaUnitTest(Series = "I2")]
		public static short InitStructI2()
		{
			InitStruct d = new InitStruct();
			return d.ValueI2;
		}

		[MosaUnitTest(Series = "I4")]
		public static int InitStructI4()
		{
			InitStruct d = new InitStruct();
			return d.ValueI4;
		}

		[MosaUnitTest(Series = "I8")]
		public static long InitStructI8()
		{
			InitStruct d = new InitStruct();
			return d.ValueI8;
		}

		[MosaUnitTest(Series = "R4")]
		public static float InitStructR4()
		{
			InitStruct d = new InitStruct();
			return d.ValueR4;
		}

		[MosaUnitTest(Series = "R8")]
		public static double InitStructR8()
		{
			InitStruct d = new InitStruct();
			return d.ValueR8;
		}

		[MosaUnitTest(Series = "B")]
		public static bool InitStructB()
		{
			InitStruct d = new InitStruct();
			return d.ValueB;
		}

		[MosaUnitTest(Series = "C")]
		public static char InitStructC()
		{
			InitStruct d = new InitStruct();
			return d.ValueC;
		}

		[MosaUnitTest(Series = "O")]
		public static object InitStructO()
		{
			InitStruct d = new InitStruct();
			return d.ValueO;
		}

	}
}
