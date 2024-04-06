// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// A device driver which queries the PCI controller in the system. It implements the <see cref="IPCIController"/> interface.
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
public sealed class PCIController : BaseDeviceDriver, IPCIController
{
	private const uint BaseValue = 0x80000000;

	private IOPortReadWrite configAddress, configData;

	public override void Initialize()
	{
		Device.Name = "PCIController_0x" + Device.Resources.IOPortRegions[0].BaseIOPort.ToString("X");

		configAddress = Device.Resources.GetIOPortReadWrite(0, 0);
		configData = Device.Resources.GetIOPortReadWrite(0, 4);
	}

	public override void Probe()
	{
		configAddress.Write32(BaseValue);
		var found = configAddress.Read32() == BaseValue;

		Device.Status = found ? DeviceStatus.Available : DeviceStatus.NotFound;
	}

	public override void Start()
	{
		if (Device.Status == DeviceStatus.Available)
			Device.Status = DeviceStatus.Online;
	}

	public override bool OnInterrupt() => false;

	private static uint GetIndex(byte bus, byte slot, byte function, byte register)
	{
		return BaseValue
			   | (uint)((bus & 0xFF) << 16)
			   | (uint)((slot & 0x0F) << 11)
			   | (uint)((function & 0x07) << 8)
			   | (uint)(register & 0xFC);
	}

	#region IPCIController

	public uint ReadConfig32(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return configData.Read32();
	}

	public ushort ReadConfig16(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return (ushort)((configData.Read32() >> (register % 4 * 8)) & 0xFFFF);
	}

	public byte ReadConfig8(PCIDevice pciDevice, byte register)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		return (byte)((configData.Read32() >> (register % 4 * 8)) & 0xFF);
	}

	public void WriteConfig32(PCIDevice pciDevice, byte register, uint value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write32(value);
	}

	public void WriteConfig16(PCIDevice pciDevice, byte register, ushort value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write16(value);
	}

	public void WriteConfig8(PCIDevice pciDevice, byte register, byte value)
	{
		configAddress.Write32(GetIndex(pciDevice.Bus, pciDevice.Slot, pciDevice.Function, register));
		configData.Write8(value);
	}

	#endregion IPCIController
}
