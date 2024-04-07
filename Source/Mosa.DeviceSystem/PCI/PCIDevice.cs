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
	#region Properties

	public byte Bus { get; }

	public byte Slot { get; }

	public byte Function { get; }

	public IPCIController Controller { get; private set; }

	public PCICapability[] Capabilities { get; private set; }

	public BaseAddress[] BaseAddresses { get; } = new BaseAddress[8];

	public ushort VendorID { get; private set; }

	public ushort DeviceID { get; private set; }

	public byte RevisionID { get; private set; }

	public byte ClassCode { get; private set; }

	public byte ProgIF { get; private set; }

	public byte SubClassCode { get; private set; }

	public ushort SubSystemVendorID { get; private set; }

	public ushort SubSystemID { get; private set; }

	public byte IRQ { get; private set; }

	public ushort StatusRegister
	{
		get => Controller.ReadConfig16(this, PCIConfigurationHeader.StatusRegister);
		set => Controller.WriteConfig16(this, PCIConfigurationHeader.StatusRegister, value);
	}

	public ushort CommandRegister
	{
		get => Controller.ReadConfig16(this, PCIConfigurationHeader.CommandRegister);
		set => Controller.WriteConfig16(this, PCIConfigurationHeader.CommandRegister, value);
	}

	#endregion Properties

	public PCIDevice(byte bus, byte slot, byte function)
	{
		Bus = bus;
		Slot = slot;
		Function = function;
	}

	public override void Initialize()
	{
		Controller = Device.Parent.DeviceDriver as IPCIController;
		if (Controller == null)
			return;

		VendorID = Controller.ReadConfig16(this, PCIConfigurationHeader.VendorID);
		DeviceID = Controller.ReadConfig16(this, PCIConfigurationHeader.DeviceID);
		RevisionID = Controller.ReadConfig8(this, PCIConfigurationHeader.RevisionID);
		ClassCode = Controller.ReadConfig8(this, PCIConfigurationHeader.ClassCode);
		ProgIF = Controller.ReadConfig8(this, PCIConfigurationHeader.ProgrammingInterface);
		SubClassCode = Controller.ReadConfig8(this, PCIConfigurationHeader.SubClassCode);
		SubSystemVendorID = Controller.ReadConfig16(this, PCIConfigurationHeader.SubSystemVendorID);
		SubSystemID = Controller.ReadConfig16(this, PCIConfigurationHeader.SubSystemID);
		IRQ = Controller.ReadConfig8(this, PCIConfigurationHeader.InterruptLineRegister);

		Device.Name = $"{Device.Parent.Name}/{Bus}.{Slot}.{Function}";

		for (byte i = 0; i < 6; i++)
		{
			var bar = (byte)(PCIConfigurationHeader.BaseAddressRegisterBase + i * 4);

			var address = Controller.ReadConfig32(this, bar);
			if (address == 0)
				continue;

			HAL.DisableAllInterrupts();

			Controller.WriteConfig32(this, bar, 0xFFFFFFFF);
			var mask = Controller.ReadConfig32(this, bar);
			Controller.WriteConfig32(this, bar, address);

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
			var ptr = Controller.ReadConfig8(this, PCIConfigurationHeader.CapabilitiesPointer);

			while (ptr != 0)
			{
				var capability = Controller.ReadConfig8(this, ptr);
				capabilities.Add(new PCICapability(capability, ptr));

				ptr = Controller.ReadConfig8(this, (byte)(ptr + 1));
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
