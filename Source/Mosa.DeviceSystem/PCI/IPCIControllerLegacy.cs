// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// An interface used for reading/writing to the PCI configuration space of a PCI device. Unlike <see cref="IPCIController"/>, this
/// interface allows writing to any bus, slot and function.
/// </summary>
public interface IPCIControllerLegacy
{
	uint ReadConfig32(byte bus, byte slot, byte function, byte register);

	ushort ReadConfig16(byte bus, byte slot, byte function, byte register);

	byte ReadConfig8(byte bus, byte slot, byte function, byte register);

	void WriteConfig32(byte bus, byte slot, byte function, byte register, uint value);

	void WriteConfig16(byte bus, byte slot, byte function, byte register, ushort value);

	void WriteConfig8(byte bus, byte slot, byte function, byte register, byte value);
}
