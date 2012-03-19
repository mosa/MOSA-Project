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
	public struct InitobjTestsStruct
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

	public static class InitobjTests
	{
		public static byte InitobjTestU1()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueU1;
		}

		public static ushort InitobjTestU2()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueU2;
		}

		public static uint InitobjTestU4()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueU4;
		}

		public static ulong InitobjTestU8()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueU8;
		}

		public static sbyte InitobjTestI1()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueI1;
		}

		public static short InitobjTestI2()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueI2;
		}

		public static int InitobjTestI4()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueI4;
		}

		public static long InitobjTestI8()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueI8;
		}

		public static float InitobjTestR4()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueR4;
		}

		public static double InitobjTestR8()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueR8;
		}

		public static bool InitobjTestB()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueB;
		}

		public static char InitobjTestC()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueC;
		}

		public static object InitobjTestO()
		{
			InitobjTestsStruct d = new InitobjTestsStruct();
			return d.ValueO;
		}

	}
}
