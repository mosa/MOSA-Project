// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.ARM32;

namespace Mosa.Kernel.BareMetal.ARM32;

/// <summary>
/// MMIO
/// </summary>
public static class SystemBoard
{
	internal static BoardType BoardType;

	public static void Setup()
	{
		BoardType = GetBoardType();
	}

	private static BoardType GetBoardType()
	{
		var value = Native.Mrc(15, 0, 0, 0, 0);

		return GetBoardType(value);
	}

	private static BoardType GetBoardType(uint register)
	{
		switch (register)
		{
			case 0xB76: return BoardType.RaspberryPi1;
			case 0xC07: return BoardType.RaspberryPi2;
			case 0xD03: return BoardType.RaspberryPi3;
			case 0xD08: return BoardType.RaspberryPi4;
			default: return BoardType.Unknown;
		}
	}
}
