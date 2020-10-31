﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86
{
	public static class PlatformPlug
	{
		private const uint BootReservedAddress = 0x00007E00; // Size=Undefined
		private const uint InitialGCMemoryPoolAddress = 0x03000000;  // @ 48MB
		private const uint InitialGCMemoryPoolSize = 16 * 1024 * 1024;  // [Size=16MB]

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

			Multiboot.Setup(new Pointer(eax), ebx);

			SSE.Setup();
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetBootReservedRegion")]
		public static AddressRange GetBootReservedRegion()
		{
			return new AddressRange(BootReservedAddress, (Page.Size * (32 + 32)));  // 4GB = 32 (on x86), 2GB = 16 (on x64), 64GB = 16 * 32
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetPlatformReservedMemory")]
		public static AddressRange GetPlatformReservedMemory(int slot)
		{
			switch (slot)
			{
				case 0: return new AddressRange(new Pointer(0), 1024 * 1024);
				default: return new AddressRange(new Pointer(0), 0);
			}
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetInitialGCMemoryPool")]
		public static AddressRange GetInitialGCMemoryPool()
		{
			return new AddressRange(InitialGCMemoryPoolAddress, InitialGCMemoryPoolSize);
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
		public static Pointer PageTableGetPhysicalAddressFromVirtual(Pointer virtualAddress)
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
