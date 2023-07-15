// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

public static class PlatformPlug
{
	private const uint BootReservedAddress = 0x00007E00; // Size=Undefined
	private const uint InitialGCMemoryPoolAddress = 0x03000000;  // @ 48MB
	private const uint InitialGCMemoryPoolSize = 16 * 1024 * 1024;  // [Size=16MB]

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

	public static PlatformArchitecture GetPlatformArchitecture() => PlatformArchitecture.X86;

	[Plug("Mosa.Kernel.BareMetal.Platform::ConsoleWrite")]
	public static void ConsoleWrite(byte c) => x86.VGAConsole.Write(c);

	[Plug("Mosa.Kernel.BareMetal.Platform::DebugWrite")]
	public static void DebugWrite(byte c) => x86.SerialDebug.Write(c);

	public static class PageTablePlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::Setup")]
		public static void Setup() => x86.PageTable.Setup();

		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::GetPageShift")]
		public static uint GetPageShift() => 12;

		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::Initialize")]
		public static void Initialize() => x86.PageTable.Initialize();

		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::Enable")]
		public static void Enable() => x86.PageTable.Enable();

		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::MapVirtualAddressToPhysical")]
		public static void MapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true) => x86.PageTable.MapVirtualAddressToPhysical(virtualAddress, physicalAddress, present);

		[Plug("Mosa.Kernel.BareMetal.Platform+PageTable::GetPhysicalAddressFromVirtual")]
		public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress) => x86.PageTable.GetPhysicalAddressFromVirtual(virtualAddress);
	}

	public static class InterruptPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform+Interrupt::Setup")]
		public static void Setup() => IDT.Setup();

		[Plug("Mosa.Kernel.BareMetal.Platform+Interrupt::SetHandler")]
		public static void SetHandler(InterruptHandler handler) => IDT.SetInterruptHandler(handler);

		[Plug("Mosa.Kernel.BareMetal.Platform+Interrupt::Enable")]
		public static void Enable() => Native.Sti();

		[Plug("Mosa.Kernel.BareMetal.Platform+Interrupt::Disable")]
		public static void Disable() => Native.Cli();
	}

	public static class IOPlugPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform+IO::In8")]
		public static byte In8(ushort address) => Native.In8(address);

		[Plug("Mosa.Kernel.BareMetal.Platform+IO::In16")]
		public static ushort In16(ushort address) => Native.In16(address);

		[Plug("Mosa.Kernel.BareMetal.Platform+IO::In32")]
		public static uint In32(ushort address) => Native.In32(address);

		[Plug("Mosa.Kernel.BareMetal.Platform+IO::Out8")]
		public static void Out8(ushort address, byte data) => Native.Out8(address, data);

		[Plug("Mosa.Kernel.BareMetal.Platform+IO::Out16")]
		public static void Out16(ushort address, ushort data) => Native.Out16(address, data);

		[Plug("Mosa.Kernel.BareMetal.Platform+IO::Out32")]
		public static void Out32(ushort address, uint data) => Native.Out32(address, data);
	}

	public static class SchedulerPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform+Scheduler::Start")]
		public static void Start() => x86.Scheduler.Start();

		[Plug("Mosa.Kernel.BareMetal.Platform+Scheduler::Yield")]
		public static void Yield() => x86.Scheduler.Yield();

		[Plug("Mosa.Kernel.BareMetal.Platform+Scheduler::SignalTermination")]
		public static void SignalTermination() => x86.Scheduler.SignalTermination();

		[Plug("Mosa.Kernel.BareMetal.Platform+Scheduler::SwitchToThread")]
		public static void SwitchToThread(Thread thread) => x86.Scheduler.SwitchToThread(thread);

		[Plug("Mosa.Kernel.BareMetal.Platform+Scheduler::SetupThreadStack")]
		public static Pointer SetupThreadStack(Pointer stackTop, Pointer methodAddress, Pointer termAddress) => x86.Scheduler.SetupThreadStack(stackTop, methodAddress, termAddress);
	}

	public static class SerialPlug
	{
		public static void Setup(int serial) => x86.Serial.Setup((ushort)serial);

		public static void Write(int serial, byte data) => x86.Serial.Write((ushort)serial, data);

		public static byte Read(int serial) => x86.Serial.Read((ushort)serial);

		public static bool IsDataReady(int serial) => x86.Serial.IsDataReady((ushort)serial);
	}
}
