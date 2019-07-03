// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal
{
	public static class PhysicalMemoryManager
	{
		public static void Start()
		{
			//TODO
		}

		public static IntPtr ReservePages(int count, int alignment)
		{
			//TODO
			return IntPtr.Zero;
		}

		public static IntPtr ReserveAnyPage()
		{
			//TODO
			return IntPtr.Zero; // new AddressRange(0, 1);
		}

		public static void ReleasePages(IntPtr page, int count)
		{
			//TODO
		}
	}
}
