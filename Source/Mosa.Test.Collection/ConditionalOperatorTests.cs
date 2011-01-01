/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

namespace Mosa.Test.Collection
{

	public static class ConditionalOperatorTests
	{

		public static sbyte EQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			return ((a == b) ? c : d);
		}

		public static short EQ_I2(short a, short b, short c, short d)
		{
			return ((a == b) ? c : d);
		}

		public static int EQ_I4(int a, int b, int c, int d)
		{
			return ((a == b) ? c : d);
		}

		public static long EQ_I8(long a, long b, long c, long d)
		{
			return ((a == b) ? c : d);
		}

		public static byte EQ_U1(byte a, byte b, byte c, byte d)
		{
			return ((a == b) ? c : d);
		}

		public static ushort EQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			return ((a == b) ? c : d);
		}

		public static uint EQ_U4(uint a, uint b, uint c, uint d)
		{
			return ((a == b) ? c : d);
		}

		public static ulong EQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			return ((a == b) ? c : d);
		}

		public static sbyte NEQ_I1(sbyte a, sbyte b, sbyte c, sbyte d)
		{
			return ((a != b) ? c : d);
		}

		public static short NEQ_I2(short a, short b, short c, short d)
		{
			return ((a != b) ? c : d);
		}

		public static int NEQ_I4(int a, int b, int c, int d)
		{
			return ((a != b) ? c : d);
		}

		public static long NEQ_I8(long a, long b, long c, long d)
		{
			return ((a != b) ? c : d);
		}

		public static byte NEQ_U1(byte a, byte b, byte c, byte d)
		{
			return ((a != b) ? c : d);
		}

		public static ushort NEQ_U2(ushort a, ushort b, ushort c, ushort d)
		{
			return ((a != b) ? c : d);
		}

		public static uint NEQ_U4(uint a, uint b, uint c, uint d)
		{
			return ((a != b) ? c : d);
		}

		public static ulong NEQ_U8(ulong a, ulong b, ulong c, ulong d)
		{
			return ((a != b) ? c : d);
		}

	}
}
