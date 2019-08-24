// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests
{
	public static class SpecificTests
	{
		[MosaUnitTest]
		public static uint SetBits32()
		{
			uint self = 0x00004000;
			return self.SetBits(12, 52, 0x00000007, 12);
		}

		[MosaUnitTest]
		public static ulong SetBits64()
		{
			ulong self = 0x00004000;
			return self.SetBits(12, 52, 0x00000007, 12);
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftRight(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU >> (64 - count);
		}

		[MosaUnitTest((byte)0)]
		[MosaUnitTest((byte)1)]
		[MosaUnitTest((byte)2)]
		[MosaUnitTest((byte)3)]
		[MosaUnitTest((byte)4)]
		[MosaUnitTest((byte)8)]
		[MosaUnitTest((byte)16)]
		[MosaUnitTest((byte)31)]
		[MosaUnitTest((byte)32)]
		[MosaUnitTest((byte)33)]
		[MosaUnitTest((byte)63)]
		[MosaUnitTest((byte)64)]
		public static ulong ShiftLeft(byte count)
		{
			return 0xFFFFFFFFFFFFFFFFU << (64 - count);
		}

		public static uint SetBits(this uint self, byte index, byte count, uint value, byte sourceIndex)
		{
			value = value >> sourceIndex;
			uint mask = 0xFFFFFFFFU >> (32 - count);
			uint bits = (value & mask) << index;
			return (self & ~(mask << index)) | bits;
		}

		public static ulong SetBits(this ulong self, byte index, byte count, ulong value, byte sourceIndex)
		{
			value = value >> sourceIndex;
			ulong mask = 0xFFFFFFFFFFFFFFFFU >> (64 - count);
			ulong bits = (value & mask) << index;
			return (self & ~(mask << index)) | bits;
		}

		[MosaUnitTest]
		public static ulong SetBits2()
		{
			ulong self = 0x00004000;
			return self.SetBits2(12, 52, 0x00000007, 12);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong SetBits2(this ulong self, byte index, byte count, ulong value, byte sourceIndex)
		{
			value = value >> sourceIndex;
			return value;
		}
	}
}
