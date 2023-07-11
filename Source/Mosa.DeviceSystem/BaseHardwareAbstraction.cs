// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem;

/// <summary>
/// Hardware
/// </summary>
public abstract class BaseHardwareAbstraction
{
	/// <summary>
	/// Gets the size of the page.
	/// </summary>
	public abstract uint PageSize { get; }

	public abstract PlatformArchitecture PlatformArchitecture { get; }

	/// <summary>
	/// Gets a block of memory from the kernel
	/// </summary>
	/// <param name="address">The address.</param>
	/// <param name="size">The size.</param>
	/// <returns></returns>
	public abstract ConstrainedPointer GetPhysicalMemory(Pointer address, uint size);

	/// Disables all interrupts.
	/// </summary>
	public abstract void DisableInterrupts();

	/// <summary>
	/// Enables all interrupts.
	/// </summary>
	public abstract void EnableInterrupts();

	/// <summary>
	/// Sleeps the specified milliseconds.
	/// </summary>
	/// <param name="milliseconds">The milliseconds.</param>
	public abstract void Sleep(uint milliseconds);

	/// <summary>
	/// Processes the interrupt.
	/// </summary>
	/// <param name="irq">The irq.</param>
	public abstract void ProcessInterrupt(byte irq);

	/// <summary>
	/// Allocates the virtual memory.
	/// </summary>
	/// <param name="size">The size.</param>
	/// <param name="alignment">The alignment.</param>
	/// <returns></returns>
	public abstract ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment);

	/// <summary>
	/// Debugs the write.
	/// </summary>
	/// <param name="message">The message.</param>
	public abstract void DebugWrite(string message);

	/// <summary>
	/// Debugs the write line.
	/// </summary>
	/// <param name="message">The message.</param>
	public abstract void DebugWriteLine(string message);

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public abstract void Abort(string message);

	/// <summary>
	/// Pause
	/// </summary>
	public abstract void Yield();

	public abstract byte In8(ushort address);

	public abstract ushort In16(ushort address);

	public abstract uint In32(ushort address);

	public abstract void Out8(ushort address, byte data);

	public abstract void Out16(ushort address, ushort data);

	public abstract void Out32(ushort address, uint data);
}
