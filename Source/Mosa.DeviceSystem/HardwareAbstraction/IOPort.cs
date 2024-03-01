// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// A structure used for interacting with port-mapped I/O devices at a specific (obviously not memory) address.
/// This structure only provides read access to the port.
/// </summary>
public struct IOPortRead
{
	public ushort Address { get; }

	public IOPortRead(ushort address) => Address = address;

	public byte Read8() => HAL.In8(Address);

	public ushort Read16() => HAL.In16(Address);

	public uint Read32() => HAL.In32(Address);
}

/// <summary>
/// A structure used for interacting with port-mapped I/O devices at a specific (obviously not memory) address.
/// This structure only provides write access to the port.
/// </summary>
public struct IOPortWrite
{
	public ushort Address { get; }

	public IOPortWrite(ushort address) => Address = address;

	public void Write8(byte data) => HAL.Out8(Address, data);

	public void Write16(ushort data) => HAL.Out16(Address, data);

	public void Write32(uint data) => HAL.Out32(Address, data);
}

/// <summary>
/// A structure used for interacting with port-mapped I/O devices at a specific (obviously not memory) address.
/// This structure provides both read and write access to the port.
/// </summary>
public struct IOPortReadWrite
{
	public ushort Address { get; }

	public IOPortReadWrite(ushort address) => Address = address;

	public byte Read8() => HAL.In8(Address);

	public ushort Read16() => HAL.In16(Address);

	public uint Read32() => HAL.In32(Address);

	public void Write8(byte data) => HAL.Out8(Address, data);

	public void Write16(ushort data) => HAL.Out16(Address, data);

	public void Write32(uint data) => HAL.Out32(Address, data);
}
