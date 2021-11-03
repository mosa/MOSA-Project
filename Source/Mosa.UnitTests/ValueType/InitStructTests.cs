﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.


// No run-time tests

namespace Mosa.UnitTests.ValueType
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
	}

	public static class InitStructTests
	{
		[MosaUnitTest]
		public static byte InitStructU1()
		{
			InitStruct d = new InitStruct();
			return d.ValueU1;
		}

		[MosaUnitTest]
		public static ushort InitStructU2()
		{
			InitStruct d = new InitStruct();
			return d.ValueU2;
		}

		[MosaUnitTest]
		public static uint InitStructU4()
		{
			InitStruct d = new InitStruct();
			return d.ValueU4;
		}

		[MosaUnitTest]
		public static ulong InitStructU8()
		{
			InitStruct d = new InitStruct();
			return d.ValueU8;
		}

		[MosaUnitTest]
		public static sbyte InitStructI1()
		{
			InitStruct d = new InitStruct();
			return d.ValueI1;
		}

		[MosaUnitTest]
		public static short InitStructI2()
		{
			InitStruct d = new InitStruct();
			return d.ValueI2;
		}

		[MosaUnitTest]
		public static int InitStructI4()
		{
			InitStruct d = new InitStruct();
			return d.ValueI4;
		}

		[MosaUnitTest]
		public static long InitStructI8()
		{
			InitStruct d = new InitStruct();
			return d.ValueI8;
		}

		[MosaUnitTest]
		public static float InitStructR4()
		{
			InitStruct d = new InitStruct();
			return d.ValueR4;
		}

		[MosaUnitTest]
		public static double InitStructR8()
		{
			InitStruct d = new InitStruct();
			return d.ValueR8;
		}

		[MosaUnitTest]
		public static bool InitStructB()
		{
			InitStruct d = new InitStruct();
			return d.ValueB;
		}

		[MosaUnitTest]
		public static char InitStructC()
		{
			InitStruct d = new InitStruct();
			return d.ValueC;
		}

	}
}
