// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public static class VirtualMemoryManager
	{
		// TODO - implementation:
		// list of page pools (representing physical pages)
		// page pool consist of bitmap + # of entries + # of free entries
		//

		public static void Start()
		{
		}

		public static IntPtr GetMemoryPages(int count)
		{
			return IntPtr.Zero;
		}

		public static void AllocatePage(IntPtr virtualPage)
		{
			return;
		}

		public static void Map(IntPtr virtualPage, IntPtr physicalPage)
		{
			return;
		}
	}
}
