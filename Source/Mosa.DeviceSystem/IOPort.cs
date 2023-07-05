// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public sealed class IOPortRead : BaseIOPortRead
{
	public IOPortRead(ushort address)
	{
		Address = address;
	}

	public override byte Read8()
	{
		return HAL.In8(Address);
	}

	public override ushort Read16()
	{
		return HAL.In16(Address);
	}

	public override uint Read32()
	{
		return HAL.In32(Address);
	}
}

public sealed class IOPortWrite : BaseIOPortWrite
{
	public IOPortWrite(ushort address)
	{
		Address = address;
	}

	public override void Write8(byte data)
	{
		HAL.Out8(Address, data);
	}

	public override void Write16(ushort data)
	{
		HAL.Out16(Address, data);
	}

	public override void Write32(uint data)
	{
		HAL.Out32(Address, data);
	}
}

public sealed class IOPortReadWrite : BaseIOPortReadWrite
{
	public IOPortReadWrite(ushort address)
	{
		Address = address;
	}

	public override byte Read8()
	{
		return HAL.In8(Address);
	}

	public override ushort Read16()
	{
		return HAL.In16(Address);
	}

	public override uint Read32()
	{
		return HAL.In32(Address);
	}

	public override void Write8(byte data)
	{
		HAL.Out8(Address, data);
	}

	public override void Write16(ushort data)
	{
		HAL.Out16(Address, data);
	}

	public override void Write32(uint data)
	{
		HAL.Out32(Address, data);
	}
}
