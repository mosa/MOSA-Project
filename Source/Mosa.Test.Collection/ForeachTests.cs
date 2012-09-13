
 
using System;

namespace Mosa.Test.Collection
{

	public static class ForeachTests 
	{
		
		public static byte ForeachU1()
		{
			byte[] a = new byte[5];
			for (int i = 0; i < 5; i++)
				a[i] = (byte)i;

			byte total = 0;

			foreach (byte v in a)
				total += v;

			return total;
		}
		
		public static ushort ForeachU2()
		{
			ushort[] a = new ushort[5];
			for (int i = 0; i < 5; i++)
				a[i] = (ushort)i;

			ushort total = 0;

			foreach (ushort v in a)
				total += v;

			return total;
		}
		
		public static uint ForeachU4()
		{
			uint[] a = new uint[5];
			for (int i = 0; i < 5; i++)
				a[i] = (uint)i;

			uint total = 0;

			foreach (uint v in a)
				total += v;

			return total;
		}
		
		public static ulong ForeachU8()
		{
			ulong[] a = new ulong[5];
			for (int i = 0; i < 5; i++)
				a[i] = (ulong)i;

			ulong total = 0;

			foreach (ulong v in a)
				total += v;

			return total;
		}
		
		public static sbyte ForeachI1()
		{
			sbyte[] a = new sbyte[5];
			for (int i = 0; i < 5; i++)
				a[i] = (sbyte)i;

			sbyte total = 0;

			foreach (sbyte v in a)
				total += v;

			return total;
		}
		
		public static short ForeachI2()
		{
			short[] a = new short[5];
			for (int i = 0; i < 5; i++)
				a[i] = (short)i;

			short total = 0;

			foreach (short v in a)
				total += v;

			return total;
		}
		
		public static int ForeachI4()
		{
			int[] a = new int[5];
			for (int i = 0; i < 5; i++)
				a[i] = (int)i;

			int total = 0;

			foreach (int v in a)
				total += v;

			return total;
		}
		
		public static long ForeachI8()
		{
			long[] a = new long[5];
			for (int i = 0; i < 5; i++)
				a[i] = (long)i;

			long total = 0;

			foreach (long v in a)
				total += v;

			return total;
		}
		}
}

