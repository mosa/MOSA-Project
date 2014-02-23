/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Xunit;
using Xunit.Extensions;

namespace Mosa.Platform.Internal.x86.xUnit
{
	public class DivisionTests
	{
		[Theory]
		//[InlineData((uint)0, (int)32)]	// does not matter
		[InlineData((uint)1, (int)31)]
		[InlineData((uint)2, (int)30)]
		[InlineData((uint)4, (int)29)]
		[InlineData((uint)8, (int)28)]
		[InlineData((uint)16, (int)27)]
		[InlineData((uint)32, (int)26)]
		[InlineData((uint)64, (int)25)]
		[InlineData((uint)128, (int)24)]
		[InlineData((uint)256, (int)23)]
		[InlineData((uint)512, (int)22)]
		[InlineData((uint)0xFFFFFFFF, (int)0)]
		public void nlz(uint value, int expected)
		{
			Assert.Equal(expected, Division.nlz(value));
		}

		[Theory]
		[InlineData((ulong)0x1, (ulong)0x1)]
		[InlineData((ulong)0x2, (ulong)0x1)]
		[InlineData((ulong)0x2, (ulong)0x100)]
		[InlineData((ulong)0x100, (ulong)0x2)]
		[InlineData((ulong)0xFFFFFFFFFFFFFFFF, (ulong)0x1)]
		[InlineData((ulong)0xFFFFFFFFFFFFFFFF, (ulong)0x2)]
		[InlineData((ulong)0xFFFFFFFFFFFFFFFF, (ulong)0xFF)]
		[InlineData((ulong)0x0000FFFFFFFFFFFF, (ulong)0x1)]
		[InlineData((ulong)0x0000FFFFFFFFFFFF, (ulong)0x2)]
		[InlineData((ulong)0x0000FFFFFFFFFFFF, (ulong)0xFF)]
		[InlineData((ulong)0x0000FFFFFFFFFFFF, (ulong)0xFFFFFFFF)]
		[InlineData((ulong)0x00F0FFFFFFFFFFFF, (ulong)0xFFFF0FFF)]
		[InlineData((ulong)0x00F0FFFFFFFFFFFF, (ulong)0xFFFF0FFF)]
		[InlineData((ulong)0x10F0FFFFFFFFFFFF, (ulong)0x12FFFF0FFF)]
		[InlineData((ulong)0x02F0FFFFFFFFFFFF, (ulong)0x12FFFF0FFF)]
		[InlineData((ulong)0x00F0FF32FFFFFFFF, (ulong)0x12FFFF0FFF)]
		[InlineData((ulong)0x00F0FFFF43FFFFFF, (ulong)0x12FFFF0FFF)]
		public void udiv64(ulong n, ulong d)
		{
			ulong expected = n / d;

			Assert.Equal(expected, Division.udiv64(n, d));
		}

		[Theory]
		[InlineData((long)0x1, (long)0x1)]
		[InlineData((long)0x2, (long)0x1)]
		[InlineData((long)0x2, (long)0x100)]
		[InlineData((long)0x100, (long)0x2)]
		[InlineData((long)0xFFFFFFFFFFFFFFF, (long)0x1)]
		[InlineData((long)0xFFFFFFFFFFFFFFF, (long)0x2)]
		[InlineData((long)0xFFFFFFFFFFFFFFF, (long)0xFF)]
		[InlineData((long)0x0000FFFFFFFFFFFF, (long)0x1)]
		[InlineData((long)0x0000FFFFFFFFFFFF, (long)0x2)]
		[InlineData((long)0x0000FFFFFFFFFFFF, (long)0xFF)]
		[InlineData((long)0x0000FFFFFFFFFFFF, (long)0xFFFFFFFF)]
		[InlineData((long)0x00F0FFFFFFFFFFFF, (long)0xFFFF0FFF)]
		[InlineData((long)0x00F0FFFFFFFFFFFF, (long)0xFFFF0FFF)]
		[InlineData((long)0x10F0FFFFFFFFFFFF, (long)0x12FFFF0FFF)]
		[InlineData((long)0x02F0FFFFFFFFFFFF, (long)0x12FFFF0FFF)]
		[InlineData((long)0x00F0FF32FFFFFFFF, (long)0x12FFFF0FFF)]
		[InlineData((long)0x00F0FFFF43FFFFFF, (long)0x12FFFF0FFF)]
		public void sdiv64(long n, long d)
		{
			long expected = n / d;

			Assert.Equal(expected, Division.sdiv64(n, d));
		}

		[Theory]
		[InlineData((ulong)0x0, (ulong)0x1)]
		[InlineData((ulong)0x1, (ulong)0x1)]
		[InlineData((ulong)0x4, (ulong)0x2)]
		[InlineData((ulong)0x8, (ulong)0x2)]
		[InlineData((ulong)0x4, (ulong)0x4)]
		[InlineData((ulong)0x8, (ulong)0x8)]
		[InlineData((ulong)0xFFFF, (ulong)0xF)]
		[InlineData((ulong)0xFFFFFF, (ulong)0xF)]
		[InlineData((ulong)0xFFFFFF, (ulong)0xFF)]
		public void umod64(ulong n, ulong d)
		{
			ulong expected = n % d;

			Assert.Equal(expected, Division.umod64(n, d));
		}

		[Theory]
		[InlineData((long)0x1, (long)0x1)]
		[InlineData((long)0x2, (long)0x1)]
		[InlineData((long)0x4, (long)0x2)]
		[InlineData((long)-0x4, (long)0x2)]
		[InlineData((long)-0x4, (long)-0x2)]
		public void smod64(long n, long d)
		{
			long expected = n % d;

			Assert.Equal(expected, Division.smod64(n, d));
		}
	}
}