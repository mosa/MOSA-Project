﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Page
{
	public static uint Shift => Platform.GetPageShift();

	public static uint Size => (uint)(1 << (int)Shift);

	public static ulong Mask => (~(Size - 1));

	public static Pointer ClearPage(Pointer page)
	{
		var writes = Size / 4;

		for (uint i = 0; i < writes; i += 4)
		{
			page.Store32(i, 0);
		}

		return page;
	}
}
