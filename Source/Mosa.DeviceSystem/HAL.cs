// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem;

/// <summary>
/// HAL
/// </summary>
public static class HAL
{
	/// <summary>
	/// The hardware abstraction
	/// </summary>
	private static BaseHardwareAbstraction hardwareAbstraction;

	public static PlatformArchitecture PlatformArchitecture => hardwareAbstraction.PlatformArchitecture;

	/// <summary>
	/// Sets the hardware abstraction.
	/// </summary>
	/// <param name="hardwareAbstraction">The hardware abstraction.</param>
	public static void Set(BaseHardwareAbstraction hardwareAbstraction) => HAL.hardwareAbstraction = hardwareAbstraction;

	/// <summary>
	/// Interrupt Delegate
	/// </summary>
	/// <param name="irq">The irq.</param>
	public delegate void HandleInterrupt(byte irq);

	private static HandleInterrupt handleInterrupt;

	/// <summary>
	/// Sets the interrupt handler.
	/// </summary>
	/// <param name="handleInterrupt">The handle interrupt.</param>
	public static void SetInterruptHandler(HandleInterrupt handleInterrupt) => HAL.handleInterrupt = handleInterrupt;

	/// <summary>
	/// Processes the interrupt.
	/// </summary>
	/// <param name="irq">The irq.</param>
	public static void ProcessInterrupt(byte irq) => handleInterrupt?.Invoke(irq);

	/// <summary>

	/// <summary>
	/// Requests a block of memory from the kernel
	/// </summary>
	/// <param name="address">The address.</param>
	/// <param name="size">The size.</param>
	/// <returns></returns>
	public static ConstrainedPointer GetPhysicalMemory(Pointer address, uint size) => hardwareAbstraction.GetPhysicalMemory(address, size);

	/// Disables all interrupts.
	/// </summary>
	internal static void DisableAllInterrupts() => hardwareAbstraction.DisableInterrupts();

	/// <summary>
	/// Enables all interrupts.
	/// </summary>
	internal static void EnableAllInterrupts() => hardwareAbstraction.EnableInterrupts();

	/// <summary>
	/// Sleeps the specified milliseconds.
	/// </summary>
	/// <param name="milliseconds">The milliseconds.</param>
	public static void Sleep(uint milliseconds) => hardwareAbstraction.Sleep(milliseconds);

	/// <summary>
	/// Allocates the memory.
	/// </summary>
	/// <param name="size">The size.</param>
	/// <param name="alignment">The alignment.</param>
	/// <returns></returns>
	public static ConstrainedPointer AllocateMemory(uint size, uint alignment) => hardwareAbstraction.AllocateVirtualMemory(size, alignment);

	/// <summary>
	/// Debugs the write.
	/// </summary>
	/// <param name="message">The message.</param>
	public static void DebugWrite(string message) => hardwareAbstraction.DebugWrite(message);

	/// <summary>
	/// Debugs the write line.
	/// </summary>
	/// <param name="message">The message.</param>
	public static void DebugWriteLine(string message) => hardwareAbstraction.DebugWriteLine(message);

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public static void Abort(string message) => hardwareAbstraction.Abort(message);

	/// <summary>
	/// Aborts with the specified message.
	/// </summary>
	/// <param name="message">The message.</param>
	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			hardwareAbstraction.Abort(message);
		}
	}

	/// <summary>
	/// Pause
	/// </summary>
	public static void Yield() => hardwareAbstraction.Yield();

	public static byte In8(ushort address) => hardwareAbstraction.In8(address);

	public static ushort In16(ushort address) => hardwareAbstraction.In16(address);

	public static uint In32(ushort address) => hardwareAbstraction.In32(address);

	public static void Out8(ushort address, byte data) => hardwareAbstraction.Out8(address, data);

	public static void Out16(ushort address, ushort data) => hardwareAbstraction.Out16(address, data);

	public static void Out32(ushort address, uint data) => hardwareAbstraction.Out32(address, data);
}
