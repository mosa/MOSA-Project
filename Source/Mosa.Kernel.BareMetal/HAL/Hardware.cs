// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.HAL;

/// <summary>
/// Hardware
/// </summary>
public sealed class Hardware : BaseHardwareAbstraction
{
	public override uint PageSize => Page.Size;

	public override ConstrainedPointer GetPhysicalMemory(Pointer address, uint size)
	{
		var pointer = VirtualPageAllocator.MapPhysical(address, size, false);

		return new ConstrainedPointer(pointer, size);
	}

	public override void EnableInterrupts()
	{
		Platform.Interrupt.Enable();
	}

	public override void DisableInterrupts()
	{
		Platform.Interrupt.Disable();
	}

	public override void ProcessInterrupt(byte irq)
	{
		DeviceSystem.HAL.ProcessInterrupt(irq);
	}

	public override void Sleep(uint milliseconds)
	{
	}

	public override ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment)
	{
		//var address = KernelMemory.AllocateVirtualMemory(size);

		//return new ConstrainedPointer(address, size);
		return new ConstrainedPointer(Pointer.Zero, 0);
	}

	/// <summary>
	/// Debugs the write.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWrite(string message)
	{
		Debug.Write(message);
	}

	/// <summary>
	/// Debugs the write line.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWriteLine(string message)
	{
		Debug.WriteLine(message);
	}

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void Abort(string message)
	{
		Debug.Fatal(message ?? "Abort");
	}

	/// <summary>
	/// Pause
	/// </summary>
	public override void Yield()
	{
		//Native.Hlt();
	}

	#region IO Port Operations

	public override byte In8(ushort address)
	{
		return Platform.IO.In8(address);
	}

	public override ushort In16(ushort address)
	{
		return Platform.IO.In16(address);
	}

	public override uint In32(ushort address)
	{
		return Platform.IO.In32(address);
	}

	public override void Out8(ushort address, byte data)
	{
		Platform.IO.Out8(address, data);
	}

	public override void Out16(ushort address, ushort data)
	{
		Platform.IO.Out16(address, data);
	}

	public override void Out32(ushort address, uint data)
	{
		Platform.IO.Out32(address, data);
	}

	#endregion IO Port Operations
}
