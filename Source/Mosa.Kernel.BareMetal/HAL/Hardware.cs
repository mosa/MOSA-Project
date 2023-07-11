// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.HAL;

/// <summary>
/// Hardware Interface
/// </summary>
public sealed class Hardware : BaseHardwareAbstraction
{
	public override uint PageSize => Page.Size;

	public override PlatformArchitecture PlatformArchitecture => Platform.GetPlatformArchitecture();

	public override ConstrainedPointer GetPhysicalMemory(Pointer address, uint size)
		=> new ConstrainedPointer(VirtualPageAllocator.MapPhysical(address, size, false), size);

	public override void EnableInterrupts() => Platform.Interrupt.Enable();

	public override void DisableInterrupts() => Platform.Interrupt.Disable();

	public override void ProcessInterrupt(byte irq) => DeviceSystem.HAL.ProcessInterrupt(irq);

	public override void Sleep(uint milliseconds)
	{
		// TODO
	}

	public override ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment)
	{
		var address = VirtualMemoryAllocator.AllocateMemory(size);

		//return new ConstrainedPointer(address, size);
		return new ConstrainedPointer(Pointer.Zero, 0);
	}

	public override void DebugWrite(string message) => Debug.Write(message);

	public override void DebugWriteLine(string message) => Debug.WriteLine(message);

	public override void Abort(string message)
	{
		Debug.Write("***HAL Abort***");
		Debug.WriteLine(message ?? "Abort");
		Debug.Fatal();
	}

	public override void Yield() => Platform.Scheduler.Yield();

	public override byte In8(ushort address) => Platform.IO.In8(address);

	public override ushort In16(ushort address) => Platform.IO.In16(address);

	public override uint In32(ushort address) => Platform.IO.In32(address);

	public override void Out8(ushort address, byte data) => Platform.IO.Out8(address, data);

	public override void Out16(ushort address, ushort data) => Platform.IO.Out16(address, data);

	public override void Out32(ushort address, uint data) => Platform.IO.Out32(address, data);
}
