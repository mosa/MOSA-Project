// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public struct Time
{
	public byte Second { get; }

	public byte Minute { get; }

	public byte Hour { get; }

	public byte Day { get; }

	public byte Month { get; }

	public ushort Year { get; }

	public Time(byte second, byte minute, byte hour, byte day, byte month, ushort year)
	{
		Second = second;
		Minute = minute;
		Hour = hour;
		Day = day;
		Month = month;
		Year = year;
	}
}
