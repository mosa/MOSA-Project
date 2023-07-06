// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

public static class PlatformPlug
{
	private const uint BootReservedAddress = 0x00007E00; // Size=Undefined
	private const uint InitialGCMemoryPoolAddress = 0x03000000;  // @ 48MB
	private const uint InitialGCMemoryPoolSize = 16 * 1024 * 1024;  // [Size=16MB]

	[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
	public static uint GetPageShift() => 12;

	[Plug("Mosa.Kernel.BareMetal.Platform::EntryPoint")]
	public static void EntryPoint()
	{
		var eax = Native.GetMultibootEAX();
		var ebx = Native.GetMultibootEBX();

		Multiboot.Setup(new Pointer(ebx), eax);

		SSE.Setup();
		SerialDebug.Setup();
		PIC.Setup();
	}

	[Plug("Mosa.Kernel.BareMetal.Platform::GetBootReservedRegion")]
	public static AddressRange GetBootReservedRegion() => new AddressRange(BootReservedAddress, Page.Size);

	[Plug("Mosa.Kernel.BareMetal.Platform::GetInitialGCMemoryPool")]
	public static AddressRange GetInitialGCMemoryPool() => new AddressRange(InitialGCMemoryPoolAddress, InitialGCMemoryPoolSize);

	[Plug("Mosa.Kernel.BareMetal.Platform::PageTableSetup")]
	public static void PageTableSetup() => PageTable.Setup();

	[Plug("Mosa.Kernel.BareMetal.Platform::PageTableInitialize")]
	public static void PageTableInitialize() => PageTable.Initialize();

	[Plug("Mosa.Kernel.BareMetal.Platform::PageTableEnable")]
	public static void PageTableEnable() => PageTable.Enable();

	[Plug("Mosa.Kernel.BareMetal.Platform::MapVirtualAddressToPhysical")]
	public static void MapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true) => PageTable.MapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);

	[Plug("Mosa.Kernel.BareMetal.Platform::GetPhysicalAddressFromVirtual")]
	public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress) => PageTable.GetPhysicalAddressFromVirtual(virtualAddress);

	[Plug("Mosa.Kernel.BareMetal.Platform::ConsoleWrite")]
	public static void ConsoleWrite(byte c) => VGAConsole.Write(c);

	[Plug("Mosa.Kernel.BareMetal.Platform::DebugWrite")]
	public static void DebugWrite(byte c) => SerialDebug.Write(c);

	[Plug("Mosa.Kernel.BareMetal.Platform::InterruptHandlerSetup")]
	public static void InterruptHandlerSetup() => IDT.Setup();

	[Plug("Mosa.Kernel.BareMetal.Platform::InterruptHandlerSet")]
	public static void InterruptHandlerSet(InterruptHandler handler) => IDT.SetInterruptHandler(handler);

	#region IO Port Operations

	[Plug("Mosa.Kernel.BareMetal.Platform::In8")]
	public static byte In8(ushort address) => Native.In8(address);

	[Plug("Mosa.Kernel.BareMetal.Platform::In16")]
	public static ushort In16(ushort address) => Native.In16(address);

	[Plug("Mosa.Kernel.BareMetal.Platform::In32")]
	public static uint In32(ushort address) => Native.In32(address);

	[Plug("Mosa.Kernel.BareMetal.Platform::Out8")]
	public static void Out8(ushort address, byte data) => Native.Out8(address, data);

	[Plug("Mosa.Kernel.BareMetal.Platform::Out16")]
	public static void Out16(ushort address, ushort data) => Native.Out16(address, data);

	[Plug("Mosa.Kernel.BareMetal.Platform::Out32")]
	public static void Out32(ushort address, uint data) => Native.Out32(address, data);

	#endregion IO Port Operations

	public static void EnableInterrupts() => Native.Sti();

	public static void DisableInterrupts() => Native.Cli();
}
