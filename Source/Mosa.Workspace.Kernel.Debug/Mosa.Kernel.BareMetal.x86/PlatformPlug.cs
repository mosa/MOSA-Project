// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	public static class PlatformPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
		public static uint GetPageShift()
		{
			return 12;
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::EntryPoint")]
		public static void EntryPoint()
		{
			var eax = Native.GetMultibootEAX();
			var ebx = Native.GetMultibootEBX();

			Multiboot.Setup(new IntPtr(eax), ebx);

			SSE.Setup();
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetMemoryMapAddress")]
		public static IntPtr GetMemoryMapAddress()
		{
			return new IntPtr(Address.GCInitialMemory);
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::UpdateBootMemoryMap")]
		public static void UpdateBootMemoryMap()
		{
			// Reserve the first 1MB
			BootMemoryMap.SetMemoryMap(new IntPtr(0), 1024 * 1024, BootMemoryMapType.Reserved);
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetInitialGCMemoryPool")]
		public static AddressRange GetInitialGCMemoryPool()
		{
			return new AddressRange(Address.GCInitialMemory, 16 * 1024 * 1024); // 16MB @ 48MB
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::PageTableSetup")]
		public static void PageTableSetup()
		{
			PageTable.Setup();
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::PageTableInitialize")]
		public static void PageTableInitialize()
		{
			PageTable.Initialize();
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::PageTableMapVirtualAddressToPhysical")]
		public static void PageTableMapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
		{
			PageTable.MapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::PageTableGetPhysicalAddressFromVirtual")]
		public static IntPtr PageTableGetPhysicalAddressFromVirtual(IntPtr virtualAddress)
		{
			return PageTable.GetPhysicalAddressFromVirtual(virtualAddress);
		}
	}
}
