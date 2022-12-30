// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Kernel.BareMetal.x86;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal
{
	public static class Platform
	{
		// These methods will be plugged and implemented elsewhere in the platform specific implementation

		public static uint GetPageShift()
		{
			return 12;
		}

		public static void EntryPoint()
		{
			Multiboot.Setup(new Pointer(Mosa.Workspace.Kernel.Emulate.Multiboot.MultibootStructure), MultibootV1.MultibootMagic);
		}

		public static AddressRange GetPlatformReservedMemory(int slot)
		{
			return PlatformPlug.GetPlatformReservedMemory(slot);
		}

		public static AddressRange GetBootReservedRegion()
		{
			return PlatformPlug.GetBootReservedRegion();
		}

		public static AddressRange GetInitialGCMemoryPool()
		{
			return PlatformPlug.GetInitialGCMemoryPool();
		}

		public static void PageTableSetup()
		{
			PlatformPlug.PageTableSetup();
		}

		public static void PageTableInitialize()
		{
			//PlatformPlug.PageTableInitialize();
		}

		public static void PageTableEnable()
		{
			// TODO
		}

		public static void PageTableMapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
		{
			//PlatformPlug.PageTableMapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);
		}

		public static Pointer PageTableGetPhysicalAddressFromVirtual(IntPtr virtualAddress)
		{
			return Pointer.Zero;

			//return PlatformPlug.PageTableGetPhysicalAddressFromVirtual(virtualAddress);
		}

		public static void ConsoleWrite(byte c)
		{
			System.Console.Write((char)c);

			//VGAConsole.Write(c);
		}
	}
}
