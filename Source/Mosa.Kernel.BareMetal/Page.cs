// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	internal static class Page
	{
		public static uint Shift { get { return Platform.GetPageShift(); } }

		public static uint Size { get { return (uint)(1 << (int)Shift); } }

		public static ulong Mask { get { return (~(Size - 1)); } }

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
