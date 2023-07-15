// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.VirtIO;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Platform
{
	// These methods will be plugged and implemented elsewhere in the platform specific implementation

	public static void EntryPoint()
	{
	}

	public static AddressRange GetBootReservedRegion() => new AddressRange(0, 0);

	public static AddressRange GetInitialGCMemoryPool() => new AddressRange(0, 0);

	public static PlatformArchitecture GetPlatformArchitecture() => PlatformArchitecture.None;

	public static void ConsoleWrite(byte c)
	{ }

	public static void DebugWrite(byte c)
	{ }

	public static class PageTable
	{
		public static uint GetPageShift() => 0;

		public static void Setup()
		{ }

		public static void Initialize()
		{ }

		public static void Enable()
		{ }

		public static void MapVirtualAddressToPhysical(Pointer virtualAddress, Pointer physicalAddress, bool present = true)
		{ }

		public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress) => Pointer.Zero;
	}

	public static class Interrupt
	{
		public static void Setup()
		{ }

		public static void SetHandler(InterruptHandler handler)
		{ }

		public static void Enable()
		{ }

		public static void Disable()
		{ }
	}

	public static class IO
	{
		public static byte In8(ushort address) => 0;

		public static ushort In16(ushort address) => 0;

		public static uint In32(ushort address) => 0;

		public static void Out8(ushort address, byte data)
		{ }

		public static void Out16(ushort address, ushort data)
		{ }

		public static void Out32(ushort address, uint data)
		{ }
	}

	public static class Scheduler
	{
		public static void Start()
		{ }

		public static void Yield()
		{ }

		public static void SignalTermination()
		{ }

		public static void SwitchToThread(Thread thread)
		{ }

		public static Pointer SetupThreadStack(Pointer stackTop, Pointer methodAddress, Pointer termAddress) => Pointer.Zero;
	}

	public static class Serial
	{
		public static void Setup(int serial)
		{ }

		public static void Write(int serial, byte data)
		{ }

		public static byte Read(int serial) => 0;

		public static bool IsDataReady(int serial) => false;
	}
}
