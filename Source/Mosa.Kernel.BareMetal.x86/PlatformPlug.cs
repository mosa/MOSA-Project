// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	public static class PlatformPlug
	{
		private const uint BootReservedAddress = 0x00007E00; // Size=Undefined
		private const uint GCInitialAddress = 0x03000000;  // 48MB [Size=16MB]

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

		[Plug("Mosa.Kernel.BareMetal.Platform::GetBootReservedRegion")]
		public static AddressRange GetBootReservedRegion()
		{
			return new AddressRange(BootReservedAddress, Page.Size);
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
			return new AddressRange(GCInitialAddress, 16 * 1024 * 1024); // 16MB @ 48MB
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

		[Plug("Mosa.Kernel.BareMetal.Platform::PageTableEnable")]
		public static void PageTableEnable()
		{
			PageTable.Enable();
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

		[Plug("Mosa.Kernel.BareMetal.Platform::ConsoleWrite")]
		public static void ConsoleWrite(byte c)
		{
			VGAConsole.Write(c);
		}
	}
}
