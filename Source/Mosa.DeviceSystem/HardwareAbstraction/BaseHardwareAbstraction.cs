// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;
using Mosa.DeviceSystem.Misc;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// The base class for the MOSA hardware abstraction layer. See the HardwareAbstractionLayer class of Mosa.Kernel.BareMetal for an
/// implementation.
/// </summary>
public abstract class BaseHardwareAbstraction
{
	public abstract uint PageSize { get; }

	public abstract PlatformArchitecture PlatformArchitecture { get; }

	public abstract ConstrainedPointer GetPhysicalMemory(Pointer address, uint size);

	public abstract void DisableInterrupts();

	public abstract void EnableInterrupts();

	public abstract void Sleep(uint milliseconds);

	public abstract void ProcessInterrupt(byte irq);

	public abstract ConstrainedPointer AllocateVirtualMemory(uint size, uint alignment);

	public abstract void DebugWrite(string message);

	public abstract void DebugWriteLine(string message);

	[DoesNotReturn]
	public abstract void Abort(string message);

	public abstract void Yield();

	public abstract byte In8(ushort address);

	public abstract ushort In16(ushort address);

	public abstract uint In32(ushort address);

	public abstract void Out8(ushort address, byte data);

	public abstract void Out16(ushort address, ushort data);

	public abstract void Out32(ushort address, uint data);
}
