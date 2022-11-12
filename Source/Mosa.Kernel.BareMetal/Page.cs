// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal
{
	public static class Page
	{
		public static uint Shift => Platform.GetPageShift();

		public static uint Size => (uint)(1 << (int)Shift);

		public static ulong Mask => (~(Size - 1));

		public static Pointer ClearPage(Pointer page)
		{
			//Console.WriteLine("Mosa.Kernel.BareMetal.Page.ClearPage:Enter");

			var writes = Size / 8;

			for (uint i = 0; i < writes; i += 8)
			{
				//Console.WriteValue(i);
				//Console.WriteLine();

				page.Store64(i, 0);
			}

			//Console.WriteLine("Mosa.Kernel.BareMetal.Page.ClearPage:Exit");

			return page;
		}
	}
}
