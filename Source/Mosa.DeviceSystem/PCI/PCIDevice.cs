// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// The base class for all PCI devices in the device driver framework. It provides a generic way of initializing and enabling/disabling
/// such devices.
/// </summary>
public class PCIDevice : BaseDeviceDriver
{
	private IPCIController pciController;

	#region Properties

	public byte Bus { get; }

	public byte Slot { get; }

	public byte Function { get; }

	public PCICapability[] Capabilities { get; private set; }

	public ushort VendorID => pciController.ReadConfig16(this, PCIConfigurationHeader.VendorID);

	public ushort DeviceID => pciController.ReadConfig16(this, PCIConfigurationHeader.DeviceID);

	public byte RevisionID => pciController.ReadConfig8(this, PCIConfigurationHeader.RevisionID);

	public byte ClassCode => pciController.ReadConfig8(this, PCIConfigurationHeader.ClassCode);

	public byte ProgIF => pciController.ReadConfig8(this, PCIConfigurationHeader.ProgrammingInterface);

	public byte SubClassCode => pciController.ReadConfig8(this, PCIConfigurationHeader.SubClassCode);

	public ushort SubSystemVendorID => pciController.ReadConfig16(this, PCIConfigurationHeader.SubSystemVendorID);

	public ushort SubSystemID => pciController.ReadConfig16(this, PCIConfigurationHeader.SubSystemID);

	public byte IRQ => pciController.ReadConfig8(this, PCIConfigurationHeader.InterruptLineRegister);

	public ushort StatusRegister
	{
		get => pciController.ReadConfig16(this, PCIConfigurationHeader.StatusRegister);
		set => pciController.WriteConfig16(this, PCIConfigurationHeader.StatusRegister, value);
	}

	public ushort CommandRegister
	{
		get => pciController.ReadConfig16(this, PCIConfigurationHeader.CommandRegister);
		set => pciController.WriteConfig16(this, PCIConfigurationHeader.CommandRegister, value);
	}

	public BaseAddress[] BaseAddresses { get; private set; }

	#endregion Properties

	public PCIDevice(byte bus, byte slot, byte function)
	{
		Bus = bus;
		Slot = slot;
		Function = function;
	}

	public override void Initialize()
	{
		pciController = Device.Parent.DeviceDriver as IPCIController;
		if (pciController == null)
			return;

		Device.Name = $"{Device.Parent.Name}/{Bus}.{Slot}.{Function}";
		BaseAddresses = new BaseAddress[8];

		for (byte i = 0; i < 6; i++)
		{
			var bar = (byte)(PCIConfigurationHeader.BaseAddressRegisterBase + i * 4);

			var address = pciController.ReadConfig32(this, bar);
			if (address == 0)
				continue;

			HAL.DisableAllInterrupts();

			pciController.WriteConfig32(this, bar, 0xFFFFFFFF);
			var mask = pciController.ReadConfig32(this, bar);
			pciController.WriteConfig32(this, bar, address);

			HAL.EnableAllInterrupts();

			if (address % 2 == 1)
			{
				BaseAddresses[i] = new BaseAddress(AddressType.PortIO, new Pointer(address & 0x0000FFF8), (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
			}
			else
			{
				BaseAddresses[i] = new BaseAddress(AddressType.Memory, new Pointer(address & 0xFFFFFFF0), ~(mask & 0xFFFFFFF0) + 1, (address & 0x08) == 1);
			}
		}

		// FIXME: Special case for generic VGA
		if (ClassCode == 0x03 && SubClassCode == 0x00 && ProgIF == 0x00)
		{
			BaseAddresses[6] = new BaseAddress(AddressType.Memory, new Pointer(0xA0000), 0x1FFFF, false);
			BaseAddresses[7] = new BaseAddress(AddressType.PortIO, new Pointer(0x3B0), 0x0F, false);
		}

		if ((StatusRegister & (byte)PCIStatus.Capability) != 0)
		{
			var capabilities = new List<PCICapability>();
			var ptr = pciController.ReadConfig8(this, PCIConfigurationHeader.CapabilitiesPointer);

			while (ptr != 0)
			{
				var capability = pciController.ReadConfig8(this, ptr);
				capabilities.Add(new PCICapability(capability, ptr));

				ptr = pciController.ReadConfig8(this, (byte)(ptr + 1));
			}

			Capabilities = capabilities.ToArray();
		}

		EnableDevice();
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start() => Device.Status = DeviceStatus.Online;

	public override bool OnInterrupt()
	{
		// TODO
		return true;
	}

	public void EnableDevice()
		=> CommandRegister = (ushort)(CommandRegister | PCICommand.IOSpaceEnable | PCICommand.BusMasterFunctionEnable | PCICommand.MemorySpaceEnable);

	public void DisableDevice()
		=> CommandRegister = (ushort)(CommandRegister & ~PCICommand.IOSpaceEnable & ~PCICommand.BusMasterFunctionEnable & PCICommand.MemorySpaceEnable);

	public void SetNoDriverFound() => Device.Status = DeviceStatus.NotFound;

	public void SetDeviceOnline() => Device.Status = DeviceStatus.Online;
}
