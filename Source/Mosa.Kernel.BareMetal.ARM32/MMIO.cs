// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.ARM32;

/// <summary>
/// MMIO
/// </summary>
public static class MMIO
{
	internal static Pointer MMIOBas;

	public static void Setup()
	{
		MMIOBas = GetMMIOBase(SystemBoard.BoardType);
	}

	private static Pointer GetMMIOBase(BoardType boardType)
	{
		switch (boardType)
		{
			case BoardType.RaspberryPi1: return new Pointer(0x20000000);
			case BoardType.RaspberryPi3: return new Pointer(0x3F000000);
			case BoardType.RaspberryPi4: return new Pointer(0x3F000000);
			case BoardType.RaspberryPi5: return new Pointer(0xFE000000);
			case BoardType.Unknown: return new Pointer(0x20000000); // default
			default: return Pointer.Zero;
		}
	}
}
