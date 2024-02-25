// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Drivers.PCI;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// The base class for all PCI devices in the device driver framework. It provides a generic way of initializing and enabling/disabling
/// such devices.
/// </summary>
public class PCIDevice : BaseDeviceDriver
{
	#region PCICommand

	private struct PCIConfigurationHeader
	{
		internal const int VendorID = 0x00;
		internal const int DeviceID = 0x02;
		internal const int CommandRegister = 0x04;
		internal const int StatusRegister = 0x06;
		internal const int RevisionID = 0x08;
		internal const int ProgrammingInterface = 0x09;
		internal const int SubClassCode = 0x0A;
		internal const int ClassCode = 0x0B;
		internal const int CacheLineSize = 0xC;
		internal const int LatencyTimer = 0xD;
		internal const int HeaderType = 0xE;
		internal const int BIST = 0xF;
		internal const int BaseAddressRegisterBase = 0x10;
		internal const int BaseAddressRegister1 = 0x10;
		internal const int BaseAddressRegister2 = 0x14;
		internal const int BaseAddressRegister3 = 0x18;
		internal const int BaseAddressRegister4 = 0x1C;
		internal const int BaseAddressRegister5 = 0x20;
		internal const int BaseAddressRegister6 = 0x24;
		internal const int CardbusCISPointer = 0x28;
		internal const int SubSystemVendorID = 0x2C;
		internal const int SubSystemID = 0x2E;
		internal const int ExpansionROMBaseAddress = 0x30;
		internal const int CapabilitiesPointer = 0x34;
		internal const int InterruptLineRegister = 0x3C;
		internal const int InterruptPinRegister = 0x3D;
		internal const int MIN_GNT = 0x3E;
		internal const int MAX_LAT = 0x3F;

		//internal const int CapabilityID = 0x80;
		//internal const int NextCapabilityPointer = 0x81;
		//internal const int PowerManagementCapabilities = 0x82;
		//internal const int PowerManagementControlStatusRegister = 0x84;
		//internal const int BridgeSupportExtension = 0x86;
		//internal const int PowerManagementDataRegister = 0x87;
		//internal const int CapabilityID = 0xA0;
		//internal const int NextCapabilityPointer = 0xA1;
		//internal const int MessageControl = 0xA2;
		//internal const int MessageAddress = 0xA4;
		//internal const int MessageData = 0xA8;
		//internal const int MaskBitsforMSI = 0xAC;
		//internal const int PendingBitsforMSI = 0xB0;
	}

	private struct PCICommand
	{
		internal const ushort IOSpaceEnable = 0x1; // Enable response in I/O space
		internal const ushort MemorySpaceEnable = 0x2; //  Enable response in memory space
		internal const ushort BusMasterFunctionEnable = 0x4; //  Enable bus mastering
		internal const ushort SpecialCycleEnable = 0x8; //  Enable response to special cycles
		internal const ushort MemoryWriteandInvalidateEnable = 0x10; //  Use memory write and invalidate

		//internal const ushort VGA_Pallete = 0x20; //  Enable palette snooping
		//internal const ushort Parity = 0x40; //  Enable parity checking
		//internal const ushort Wait = 0x80; //  Enable address/data stepping
		//internal const ushort SERR = 0x100; //  Enable SERR
		//internal const ushort Fast_Back = 0x200; //  Enable back-to-back writes
	}

	#endregion PCICommand

	private IPCIController pciController;

	#region Properties

	public byte Bus { get; private set; }

	public byte Slot { get; private set; }

	public byte Function { get; private set; }

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

	public override void Initialize()
	{
		pciController = Device.Parent.DeviceDriver as IPCIController;
		if (pciController == null)
			return;

		var configuration = Device.Configuration as PCIDeviceConfiguration;
		if (configuration == null)
			return;

		Bus = configuration.Bus;
		Slot = configuration.Slot;
		Function = configuration.Function;

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
