// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// CMOS Device Driver
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.X86)]
public class CMOS : BaseDeviceDriver, IDateTime
{
	/// <summary>
	/// The command port
	/// </summary>
	protected IOPortReadWrite commandPort;

	/// <summary>
	/// The data port
	/// </summary>
	protected IOPortReadWrite dataPort;

	public override void Initialize()
	{
		Device.Name = "CMOS";

		commandPort = Device.Resources.GetIOPortReadWrite(0, 0);
		dataPort = Device.Resources.GetIOPortReadWrite(0, 4);
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		Device.Status = DeviceStatus.Online;
	}

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public override bool OnInterrupt() => true;

	public DateTime GetDateTime()
	{
		var bcd = (Read(0x0B) & 0x04) == 0x00;

		var century = BCDToBinary(bcd, Read(0x32));
		var second = BCDToBinary(bcd, Read(0));
		var minute = BCDToBinary(bcd, Read(2));
		var hour = BCDToBinary(bcd, Read(4));
		var year = BCDToBinary(bcd, Read(9));
		var month = BCDToBinary(bcd, Read(8));
		var day = BCDToBinary(bcd, Read(7));

		if (century is 19 or 21)
		{
			return new DateTime(century * 100 + year, month, day, hour, minute, second);
		}
		else
		{
			return new DateTime(2000 + year, month, day, hour, minute, second);
		}
	}

	/// <summary>
	/// Reads the specified address.
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns></returns>
	protected byte Read(byte address)
	{
		lock (_lock)
		{
			commandPort.Write8(address);
			return dataPort.Read8();
		}
	}

	private static byte BCDToBinary(bool bcd, byte value)
	{
		if (bcd)
			return BCDToBinary(value);
		else
			return value;
	}

	private static byte BCDToBinary(byte bcd)
	{
		return (byte)(bcd / 16 * 10 + (bcd & 0xF));
	}
}
