// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

//https://github.com/nifanfa/MOOS/blob/master/Kernel/Driver/RTC.cs
namespace Mosa.Kernel.BareMetal.x86;

public static class RTC
{
	public static byte Second
	{
		get
		{
			var b = Get(0);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static byte Minute
	{
		get
		{
			var b = Get(2);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static byte Hour
	{
		get
		{
			var b = Get(4);
			return (byte)(((b & 0x0F) + (b & 0x70) / 16 * 10) | (b & 0x80));
		}
	}

	public static byte Century
	{
		get
		{
			var b = Get(0x32);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static byte Year
	{
		get
		{
			var b = Get(9);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static byte Month
	{
		get
		{
			var b = Get(8);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static byte Day
	{
		get
		{
			var b = Get(7);
			return (byte)((b & 0x0F) + b / 16 * 10);
		}
	}

	public static bool BCD => (Get(0x0B) & 0x04) == 0x00;

	public static byte Get(byte index)
	{
		Native.Out8(0x70, index);

		return Native.In8(0x71);
	}

	public static void Set(byte index, byte value)
	{
		Native.Out8(0x70, index);
		Native.Out8(0x71, value);
	}

	private static void Delay()
	{
		Native.In8(0x80);
		Native.Out8(0x80, 0);
	}
}
