/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert (Boddlnagg) <kpreisert@googlemail.com> 
 *
 */
 

namespace Mosa.Test.Collection
{
	public struct InitStruct
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
		public static byte InitStructU1()
		{
			InitStruct d = new InitStruct();
			return d.ValueU1;
		}

		public static ushort InitStructU2()
		{
			InitStruct d = new InitStruct();
			return d.ValueU2;
		}

		public static uint InitStructU4()
		{
			InitStruct d = new InitStruct();
			return d.ValueU4;
		}

		public static ulong InitStructU8()
		{
			InitStruct d = new InitStruct();
			return d.ValueU8;
		}

		public static sbyte InitStructI1()
		{
			InitStruct d = new InitStruct();
			return d.ValueI1;
		}

		public static short InitStructI2()
		{
			InitStruct d = new InitStruct();
			return d.ValueI2;
		}

		public static int InitStructI4()
		{
			InitStruct d = new InitStruct();
			return d.ValueI4;
		}

		public static long InitStructI8()
		{
			InitStruct d = new InitStruct();
			return d.ValueI8;
		}

		public static float InitStructR4()
		{
			InitStruct d = new InitStruct();
			return d.ValueR4;
		}

		public static double InitStructR8()
		{
			InitStruct d = new InitStruct();
			return d.ValueR8;
		}

		public static bool InitStructB()
		{
			InitStruct d = new InitStruct();
			return d.ValueB;
		}

		public static char InitStructC()
		{
			InitStruct d = new InitStruct();
			return d.ValueC;
		}

		public static object InitStructO()
		{
			InitStruct d = new InitStruct();
			return d.ValueO;
		}

	}
}
