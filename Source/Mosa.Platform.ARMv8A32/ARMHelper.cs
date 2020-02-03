// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.ARMv8A32
{
	public static class ARMHelper
	{
		public static bool CalculateRotatedImmediateValue(uint value, out uint immediate, out byte rotation4, out byte imm8)
		{
			if ((value & 0xFF) == value)
			{
				rotation4 = 0;
				imm8 = (byte)value;
				immediate = value;

				return true;
			}

			int shift = 0;

			if ((value & 0xFFFF) == 0)
			{
				shift = 16;
				value >>= 16;
			}

			do
			{
				var r = (value & 0b1) << 31;
				value >>= 1;
				value |= r;

				if (((value & 0xFF) == value) && (shift % 2 == 0))
				{
					rotation4 = (byte)(shift / 2);
					imm8 = (byte)value;
					immediate = value | (uint)(rotation4 << 8);

					return true;
				}
				shift++;
			}
			while (shift < 32);

			rotation4 = 0;
			imm8 = 0;
			immediate = 0;

			return false;
		}
	}
}
