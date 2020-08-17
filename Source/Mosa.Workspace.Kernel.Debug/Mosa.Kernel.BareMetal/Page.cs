// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal
{
	public static class Page
	{
		public static uint Shift { get { return Platform.GetPageShift(); } }

		public static uint Size { get { return (uint)(1 << (int)Shift); } }

		public static ulong Mask { get { return (~(Size - 1)); } }

		public static Pointer ClearPage(Pointer page)
		{
			uint writes = Size / 8;

			for (uint i = 0; i < writes; i += 8)
			{
				page.Store64(i, 0);
			}

			return page;
		}
	}
}
