// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class SpecificTests
	{
		[MosaUnitTest]
		public static uint SetBits32()
		{
			uint self = 0x00004000;
			uint mask = 0x00000007;
			return self.SetBits(12, 52, mask, 12);
		}

		[MosaUnitTest]
		public static ulong SetBits64()
		{
			ulong self = 0x00004000;
			ulong mask = 0x00000007;
			return self.SetBits(12, 52, mask, 12);
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
	}
}
