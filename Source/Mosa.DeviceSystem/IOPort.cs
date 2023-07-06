// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public struct IOPortRead
{
	public ushort Address { get; private set; }

	public IOPortRead(ushort address) => Address = address;

	public byte Read8() => HAL.In8(Address);

	public ushort Read16() => HAL.In16(Address);

	public uint Read32() => HAL.In32(Address);
}

public struct IOPortWrite
{
	public ushort Address { get; private set; }

	public IOPortWrite(ushort address) => Address = address;

	public void Write8(byte data) => HAL.Out8(Address, data);

	public void Write16(ushort data) => HAL.Out16(Address, data);

	public void Write32(uint data) => HAL.Out32(Address, data);
}

public struct IOPortReadWrite
{
	public ushort Address { get; private set; }

	public IOPortReadWrite(ushort address)
	{
		Address = address;
	}

	public byte Read8() => HAL.In8(Address);

	public ushort Read16() => HAL.In16(Address);

	public uint Read32() => HAL.In32(Address);

	public void Write8(byte data) => HAL.Out8(Address, data);

	public void Write16(ushort data) => HAL.Out16(Address, data);

	public void Write32(uint data) => HAL.Out32(Address, data);
}
