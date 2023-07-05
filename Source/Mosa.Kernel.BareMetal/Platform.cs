// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Platform
{
	// These methods will be plugged and implemented elsewhere in the platform specific implementation

	public static uint GetPageShift()
	{
		return 0;
	}

	public static void EntryPoint()
	{
	}

	public static AddressRange GetBootReservedRegion()
	{
		return new AddressRange(0, 0);
	}

	public static AddressRange GetInitialGCMemoryPool()
	{
		return new AddressRange(0, 0);
	}

	public static void PageTableSetup()
	{ }

	public static void PageTableInitialize()
	{ }

	public static void PageTableEnable()
	{ }

	public static void PageTableMapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true)
	{ }

	public static Pointer PageTableGetPhysicalAddressFromVirtual(Pointer virtualAddress)
	{
		return Pointer.Zero;
	}

	public static void ConsoleWrite(byte c)
	{ }

	public static void DebugWrite(byte c)
	{ }

	public static void InterruptHandlerSetup()
	{ }

	public static void InterruptHandlerSet(InterruptHandler handler)
	{ }

	#region IO Port Operations

	public static byte In8(ushort address)
	{
		return 0;
	}

	public static ushort In16(ushort address)
	{
		return 0;
	}

	public static uint In32(ushort address)
	{
		return 0;
	}

	public static void Out8(ushort address, byte data)
	{ }

	public static void Out16(ushort address, ushort data)
	{ }

	public static void Out32(ushort address, uint data)
	{ }

	#endregion IO Port Operations
}
