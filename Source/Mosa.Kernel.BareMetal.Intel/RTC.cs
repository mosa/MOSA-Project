// Copyright (c) MOSA Project. Licensed under the New BSD License.

//https://github.com/nifanfa/MOOS/blob/master/Kernel/Driver/RTC.cs

namespace Mosa.Kernel.BareMetal.Intel;

public static class RTC
{
	public static bool BCD { get; private set; }

	public static byte Second => Get(0);

	public static byte Minute => Get(2);

	public static byte Hour => Get(4);

	public static byte Day => Get(7);

	public static byte Month => Get(8);

	public static byte Year => Get(9);

	public static byte Century => Get(0x32);

	public static void Setup() => BCD = (Get(0x0B) & 0x04) == 0x00;

	public static byte BCDToBinary(byte value) => BCD ? (byte)(value / 16 * 10 + (value & 0xF)) : value;

	public static byte Get(byte index)
	{
		Platform.IO.Out8(0x70, index);

		return Platform.IO.In8(0x71);
	}

	public static void Set(byte index, byte value)
	{
		Platform.IO.Out8(0x70, index);
		Platform.IO.Out8(0x71, value);
	}
}
