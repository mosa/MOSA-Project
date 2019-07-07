// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

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
			//PlatformPlug.EntryPoint();
		}

		public static IntPtr GetMemoryMapAddress()
		{
			return new IntPtr(0);

			//return PlatformPlug.GetMemoryMapAddress();
		}

		public static void UpdateBootMemoryMap()
		{
		}

		//public static AddressRange GetInitialGCMemoryPool()
		//{
		//	return PlatformPlug.GetInitialGCMemoryPool();
		//}

		public static void PageTableSetup()
		{
			//PlatformPlug.PageTableSetup();
		}

		public static void PageTableInitialize()
		{
			//PlatformPlug.PageTableInitialize();
		}

		public static void PageTableMapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
		{
			//PlatformPlug.PageTableMapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);
		}

		public static IntPtr PageTableGetPhysicalAddressFromVirtual(IntPtr virtualAddress)
		{
			return new IntPtr(0);

			//return PlatformPlug.PageTableGetPhysicalAddressFromVirtual(virtualAddress);
		}
	}
}
