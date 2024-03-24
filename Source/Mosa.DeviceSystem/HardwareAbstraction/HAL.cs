// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;
using Mosa.DeviceSystem.Misc;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// A static class used throughout the code base to call the hardware abstraction layer. It internally sets an instance of a
/// <see cref="BaseHardwareAbstraction"/> class, and effectively does nothing more than that, except from providing a
/// <see cref="HandleInterrupt"/> delegate method to handle interrupts coming from anywhere.
/// </summary>
public static class HAL
{
	public delegate void HandleInterrupt(byte irq);

	public static PlatformArchitecture PlatformArchitecture => hardwareAbstraction.PlatformArchitecture;

	private static BaseHardwareAbstraction hardwareAbstraction;
	private static HandleInterrupt handleInterrupt;

	public static void Set(BaseHardwareAbstraction abstraction) => hardwareAbstraction = abstraction;

	public static void SetInterruptHandler(HandleInterrupt method) => handleInterrupt = method;

	public static void ProcessInterrupt(byte irq) => handleInterrupt?.Invoke(irq);

	public static ConstrainedPointer GetPhysicalMemory(Pointer address, uint size) => hardwareAbstraction.GetPhysicalMemory(address, size);

	internal static void DisableAllInterrupts() => hardwareAbstraction.DisableInterrupts();

	internal static void EnableAllInterrupts() => hardwareAbstraction.EnableInterrupts();

	public static void Sleep(uint milliseconds) => hardwareAbstraction.Sleep(milliseconds);

	public static ConstrainedPointer AllocateMemory(uint size, uint alignment) => hardwareAbstraction.AllocateVirtualMemory(size, alignment);

	public static void DebugWrite(string message) => hardwareAbstraction.DebugWrite(message);

	public static void DebugWriteLine(string message) => hardwareAbstraction.DebugWriteLine(message);

	[DoesNotReturn]
	public static void Abort(string message) => hardwareAbstraction.Abort(message);

	public static void Yield() => hardwareAbstraction.Yield();

	public static byte In8(ushort address) => hardwareAbstraction.In8(address);

	public static ushort In16(ushort address) => hardwareAbstraction.In16(address);

	public static uint In32(ushort address) => hardwareAbstraction.In32(address);

	public static void Out8(ushort address, byte data) => hardwareAbstraction.Out8(address, data);

	public static void Out16(ushort address, ushort data) => hardwareAbstraction.Out16(address, data);

	public static void Out32(ushort address, uint data) => hardwareAbstraction.Out32(address, data);
}
