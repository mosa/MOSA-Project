// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Runtime;

namespace Mosa.Workspace.Kernel
{
	internal static class Program
	{
		private static void Main()
		{
			Emulate.Multiboot.Setup(128 * 1024 * 1024); // 128 MB

			Boot.PlatformInitialization();

			Boot.GarbageCollectionInitialization();

			Boot.EntryPoint();

			Console.WriteLine();

			Pointer page;

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(64, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page:  0x{page.ToInt32():X8} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(2, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(4, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(8, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(16, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(32, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			page = PhysicalPageAllocator.ReservePages(1, 0);
			Console.WriteLine($"Page: {page.ToInt32()} {page.ToInt32() / Page.Size}");

			return;
		}
	}
}
