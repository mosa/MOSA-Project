// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	internal static class Page
	{
		public static uint Shift = Platform.GetPageShift();

		public static uint Size = (uint)(1 << (int)Shift);

		public static ulong Mask = (~(Size - 1));

		public static void ZeroOutPage(IntPtr page)
		{
			uint writes = Size / 8;

			for (uint i = 0; i < writes; i += 8)
			{
				page.Store64(i, 0);
			}
		}
	}
}
