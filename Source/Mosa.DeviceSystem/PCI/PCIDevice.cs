// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// PCI Device
	/// </summary>
	public class PCIDevice : BaseDeviceDriver, IPCIDevice, IPCIDeviceResource
	{
		#region PCICommand

		internal struct PCIConfigurationHeader
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

		internal struct PCICommand
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

		/// <summary>
		/// The pci controller
		/// </summary>
		protected IPCIController pciController;

		/// <summary>
		/// The memory region count
		/// </summary>
		protected byte memoryRegionCount;

		/// <summary>
		/// The io port region count
		/// </summary>
		protected byte ioPortRegionCount;

		#region Properties

		/// <summary>
		/// Gets the bus.
		/// </summary>
		/// <value>The bus.</value>
		public byte Bus { get; protected set; }

		/// <summary>
		/// Gets the slot.
		/// </summary>
		/// <value>The slot.</value>
		public byte Slot { get; protected set; }

		/// <summary>
		/// Gets the function.
		/// </summary>
		/// <value>The function.</value>
		public byte Function { get; protected set; }

		/// <summary>
		/// Gets the vendor ID.
		/// </summary>
		/// <value>The vendor ID.</value>
		public ushort VendorID { get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.VendorID); } }

		/// <summary>
		/// Gets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		public ushort DeviceID { get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.DeviceID); } }

		/// <summary>
		/// Gets the revision ID.
		/// </summary>
		/// <value>The revision ID.</value>
		public byte RevisionID { get { return pciController.ReadConfig8(Bus, Slot, Function, PCIConfigurationHeader.RevisionID); } }

		/// <summary>
		/// Gets the class code.
		/// </summary>
		/// <value>The class code.</value>
		public byte ClassCode { get { return pciController.ReadConfig8(Bus, Slot, Function, PCIConfigurationHeader.ClassCode); } }

		/// <summary>
		/// Gets the prog IF.
		/// </summary>
		/// <value>The prog IF.</value>
		public byte ProgIF { get { return pciController.ReadConfig8(Bus, Slot, Function, PCIConfigurationHeader.ProgrammingInterface); } }

		/// <summary>
		/// Gets the sub class code.
		/// </summary>
		/// <value>The sub class code.</value>
		public byte SubClassCode { get { return pciController.ReadConfig8(Bus, Slot, Function, PCIConfigurationHeader.SubClassCode); } }

		/// <summary>
		/// Gets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		public ushort SubSystemVendorID { get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.SubSystemVendorID); } }

		/// <summary>
		/// Gets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		public ushort SubSystemID { get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.SubSystemID); } }

		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ { get { return pciController.ReadConfig8(Bus, Slot, Function, PCIConfigurationHeader.InterruptLineRegister); } }

		/// <summary>
		/// Gets or sets the status register.
		/// </summary>
		/// <value>The status.</value>
		public ushort StatusRegister
		{
			get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.StatusRegister); }
			set { pciController.WriteConfig16(Bus, Slot, Function, PCIConfigurationHeader.StatusRegister, value); }
		}

		/// <summary>
		/// Gets or sets the command register.
		/// </summary>
		/// <value>The status.</value>
		public ushort CommandRegister
		{
			get { return pciController.ReadConfig16(Bus, Slot, Function, PCIConfigurationHeader.CommandRegister); }
			set { pciController.WriteConfig16(Bus, Slot, Function, PCIConfigurationHeader.CommandRegister, value); }
		}

		/// <summary>
		/// Gets the base addresses.
		/// </summary>
		/// <value>The base addresses.</value>
		public BaseAddress[] BaseAddresses { get; private set; }

		#endregion Properties

		public override void Initialize()
		{
			pciController = Device.Parent.DeviceDriver as IPCIController;

			var configuration = Device.Configuration as PCIDeviceConfiguration;

			Bus = configuration.Bus;
			Slot = configuration.Slot;
			Function = configuration.Function;

			Device.Name = Device.Parent.Name + '/' + Bus.ToString() + '.' + Slot.ToString() + '.' + Function.ToString();

			ioPortRegionCount = memoryRegionCount = 0;
			BaseAddresses = new BaseAddress[8];

			for (byte i = 0; i < 6; i++)
			{
				byte barr = (byte)(PCIConfigurationHeader.BaseAddressRegisterBase + (i * 4));

				uint address = pciController.ReadConfig32(Bus, Slot, Function, barr);

				if (address == 0)
					continue;

				HAL.DisableAllInterrupts();

				pciController.WriteConfig32(Bus, Slot, Function, barr, 0xFFFFFFFF);
				uint mask = pciController.ReadConfig32(Bus, Slot, Function, barr);
				pciController.WriteConfig32(Bus, Slot, Function, barr, address);

				HAL.EnableAllInterrupts();

				if (address % 2 == 1)
					BaseAddresses[i] = new BaseAddress(AddressType.IO, new IntPtr(address & 0x0000FFF8), (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
				else
					BaseAddresses[i] = new BaseAddress(AddressType.Memory, new IntPtr(address & 0xFFFFFFF0), ~(mask & 0xFFFFFFF0) + 1, (address & 0x08) == 1);
			}

			// FIXME: Special case for generic VGA
			if (ClassCode == 0x03 && SubClassCode == 0x00 && ProgIF == 0x00)
			{
				BaseAddresses[6] = new BaseAddress(AddressType.Memory, new IntPtr(0xA0000), 0x1FFFF, false);
				BaseAddresses[7] = new BaseAddress(AddressType.IO, new IntPtr(0x3B0), 0x0F, false);
			}

			foreach (var baseAddress in BaseAddresses)
			{
				if (baseAddress == null)
					continue;

				if (baseAddress.Region == AddressType.Undefined)
					continue;

				switch (baseAddress.Region)
				{
					case AddressType.IO: ioPortRegionCount++; break;
					case AddressType.Memory: memoryRegionCount++; break;
				}
			}
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
		public override bool OnInterrupt()
		{
			// TODO
			return true;
		}

		/// <summary>
		/// Enables the device.
		/// </summary>
		public void EnableDevice()
		{
			CommandRegister = (ushort)(CommandRegister | PCICommand.IOSpaceEnable | PCICommand.BusMasterFunctionEnable | PCICommand.MemorySpaceEnable);
		}

		/// <summary>
		/// Disables the device.
		/// </summary>
		public void DisableDevice()
		{
			CommandRegister = (ushort)(CommandRegister & (~PCICommand.IOSpaceEnable & ~PCICommand.BusMasterFunctionEnable & PCICommand.MemorySpaceEnable));
		}

		/// <summary>
		/// Sets the no driver found.
		/// </summary>
		public void SetNoDriverFound()
		{
			Device.Status = DeviceStatus.NotFound;
		}

		/// <summary>
		/// Sets the device online.
		/// </summary>
		public void SetDeviceOnline()
		{
			Device.Status = DeviceStatus.Online;
		}
	}
}
