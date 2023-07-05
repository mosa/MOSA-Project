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

	public override void DisableAllInterrupts()
	{
		//Native.Cli();
	}

	public override ConstrainedPointer GetPhysicalMemory(Pointer address, uint size)
	{
		// Map physical memory space to virtual memory space
		for (var at = address; at < address + size; at += PageSize)
		{
			PageTable.MapVirtualAddressToPhysical(at, at);
		}

		return new ConstrainedPointer(address, size);
	}

	public override void EnableAllInterrupts()
	{
		//Native.Sti();
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

	public override Pointer TranslateVirtualToPhysicalAddress(Pointer virtualAddress)
	{
		return PageTable.GetPhysicalAddressFromVirtual(virtualAddress);
	}

	/// <summary>
	/// Requests an IO read/write port interface from the kernel
	/// </summary>
	/// <param name="port">The port number.</param>
	/// <returns></returns>
	public override BaseIOPortReadWrite GetReadWriteIOPort(ushort port)
	{
		return new IOPortReadWrite(port);
	}

	/// <summary>
	/// Requests an IO read/write port interface from the kernel
	/// </summary>
	/// <param name="port">The port number.</param>
	/// <returns></returns>
	public override BaseIOPortRead GetReadIOPort(ushort port)
	{
		return new IOPortRead(port);
	}

	/// <summary>
	/// Requests an IO write port interface from the kernel
	/// </summary>
	/// <param name="port">The port number.</param>
	/// <returns></returns>
	public override BaseIOPortWrite GetWriteIOPort(ushort port)
	{
		return new IOPortWrite(port);
	}

	/// <summary>
	/// Debugs the write.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWrite(string message)
	{
		Console.Write(message);
	}

	/// <summary>
	/// Debugs the write line.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWriteLine(string message)
	{
		Console.WriteLine(message);
	}

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void Abort(string message)
	{
		Debug.Fatal(message);
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
		return Platform.In8(address);
	}

	public override ushort In16(ushort address)
	{
		return Platform.In16(address);
	}

	public override uint In32(ushort address)
	{
		return Platform.In32(address);
	}

	public override void Out8(ushort address, byte data)
	{
		Platform.Out8(address, data);
	}

	public override void Out16(ushort address, ushort data)
	{
		Platform.Out16(address, data);
	}

	public override void Out32(ushort address, uint data)
	{
		Platform.Out32(address, data);
	}

	#endregion IO Port Operations
}
