// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Demo.SVGAWorld.x86.HAL;

/// <summary>
/// Hardware
/// </summary>
public sealed class Hardware : BaseHardwareAbstraction
{
	/// <summary>
	/// Gets the size of the page.
	/// </summary>
	public override uint PageSize => PageFrameAllocator.PageSize;

	public override PlatformArchitecture PlatformArchitecture => PlatformArchitecture.X86;

	/// <summary>
	/// Gets a block of memory from the kernel
	/// </summary>
	/// <param name="address">The address.</param>
	/// <param name="size">The size.</param>
	/// <returns></returns>
	public override ConstrainedPointer GetPhysicalMemory(Pointer address, uint size)
	{
		var start = (uint)address.ToInt32();

		// Map physical memory space to virtual memory space
		for (var at = start; at < start + size; at += PageSize)
		{
			PageTable.MapVirtualAddressToPhysical(at, at);
		}

		return new ConstrainedPointer(address, size);
	}

	/// <summary>
	/// Disables all interrupts.
	/// </summary>
	public override void DisableInterrupts()
	{
		Native.Cli();
	}

	/// <summary>
	/// Enables all interrupts.
	/// </summary>
	public override void EnableInterrupts()
	{
		Native.Sti();
	}

	/// <summary>
	/// Processes the interrupt.
	/// </summary>
	/// <param name="irq">The irq.</param>
	public override void ProcessInterrupt(byte irq)
	{
		DeviceSystem.HAL.ProcessInterrupt(irq);
	}

	/// <summary>
	/// Sleeps the specified milliseconds.
	/// </summary>
	/// <param name="milliseconds">The milliseconds.</param>
	public override void Sleep(uint milliseconds)
	{
	}

	/// <summary>
	/// Allocates the virtual memory.
	/// </summary>
	/// <param name="size">The size.</param>
	/// <param name="alignment">The alignment.</param>
	/// <returns></returns>
	public override ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment)
	{
		var address = KernelMemory.AllocateVirtualMemory(size);

		return new ConstrainedPointer(address, size);
	}

	/// <summary>
	/// Debugs the write.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWrite(string message)
	{
		//Console.Write(message);
	}

	/// <summary>
	/// Debugs the write line.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void DebugWriteLine(string message)
	{
		//Serial.Write(Serial.COM1, message);
		//Serial.Write(Serial.COM1, (byte)'\n');
	}

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public override void Abort(string message)
	{
		Environment.FailFast(message);
	}

	/// <summary>
	/// Pause
	/// </summary>
	public override void Yield()
	{
		Native.Hlt();
	}

	#region IO Port Operations

	public override byte In8(ushort address)
	{
		return Native.In8(address);
	}

	public override ushort In16(ushort address)
	{
		return Native.In16(address);
	}

	public override uint In32(ushort address)
	{
		return Native.In32(address);
	}

	public override void Out8(ushort address, byte data)
	{
		Native.Out8(address, data);
	}

	public override void Out16(ushort address, ushort data)
	{
		Native.Out16(address, data);
	}

	public override void Out32(ushort address, uint data)
	{
		Native.Out32(address, data);
	}

	#endregion IO Port Operations
}
