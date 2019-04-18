// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	internal static class Page
	{
		public static uint Shift = Platform.GetPageShift();

		public static uint Size = (uint)(1 << (int)Shift);

		public static ulong Mask = (~(Size - 1));
	}
}
