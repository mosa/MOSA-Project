// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using System;

namespace Mosa.Workspace.Kernel
{
	internal static class Program
	{
		private static void Main()
		{
			Emulate.Multiboot.Setup(128 * 1024 * 1024); // 128 MB

			Boot.EntryPoint();

			var page1 = PhysicalPageAllocator.ReservePages(1, 0);
			System.Console.WriteLine($"Page: {page1.ToInt32()}");

			var page2 = PhysicalPageAllocator.ReservePages(65, 0);
			System.Console.WriteLine($"Page: {page2.ToInt32()}");

			var page3 = PhysicalPageAllocator.ReservePages(1, 0);
			System.Console.WriteLine($"Page: {page3.ToInt32()}");

			return;
		}
	}
}
