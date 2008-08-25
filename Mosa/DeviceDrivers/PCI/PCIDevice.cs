/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.Kernel;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.PCI
{
	public class PCIDevice : Device, IDevice
	{
		protected uint bus;
		protected uint slot;
		protected uint function;
		protected ushort vendorID;
		protected ushort deviceID;
		protected byte revisionID;
		protected ushort classCode;
		protected byte subClassCode;
		protected ushort subVendorID;
		protected ushort subDeviceID;
		protected byte progIF;
		protected byte irq;
		protected PCIBaseAddress[] addresses;

		protected IPCIController pciController;

		public PCIBaseAddress[] BaseAddresses { get { return addresses; } }
		public uint Bus { get { return bus; } }
		public uint Slot { get { return slot; } }
		public uint Function { get { return function; } }
		public ushort VendorID { get { return vendorID; } }
		public ushort DeviceID { get { return deviceID; } }
		public byte RevisionID { get { return revisionID; } }
		public ushort ClassCode { get { return classCode; } }
		public byte ProgIF { get { return progIF; } }
		public byte SubClassCode { get { return subClassCode; } }
		public ushort SubVendorID { get { return subVendorID; } }
		public ushort SubDeviceID { get { return subDeviceID; } }
		public byte IRQ { get { return irq; } }

		/// <summary>
		/// Create a new PCIDevice instance at the selected PCI address
		/// </summary>
		public PCIDevice(IPCIController pciController, uint bus, uint slot, uint fun)
		{
			base.parent = pciController as Device;
			base.name = base.parent.Name + "/" + bus.ToString() + "/" + slot.ToString() + "/" + fun.ToString();
			base.deviceStatus = DeviceStatus.Available;

			this.pciController = pciController;
			this.bus = bus;
			this.slot = slot;
			this.function = fun;

			this.addresses = new PCIBaseAddress[6];

			uint data = pciController.ReadConfig(bus, slot, fun, 0);
			this.vendorID = (ushort)(data & 0xFFFF);
			this.deviceID = (ushort)((data >> 16) & 0xFFFF);

			data = pciController.ReadConfig(bus, slot, fun, 0x08);
			this.revisionID = (byte)(data & 0xFF);
			this.progIF = (byte)((data >> 8) & 0xFF);
			this.classCode = (ushort)((data >> 16) & 0xFFFF);
			this.subClassCode = (byte)((data >> 16) & 0xFF);

			data = pciController.ReadConfig(bus, slot, fun, 0x0c);
			this.subVendorID = (ushort)(data & 0xFFFF);
			this.subDeviceID = (ushort)((data >> 16) & 0xFFFF);

			data = pciController.ReadConfig(bus, slot, fun, 0x3c);

			if ((data & 0xFF00) != 0)
				this.irq = (byte)(data & 0xFF);

			for (uint i = 0; i < 6; i++) {
				uint baseAddress = pciController.ReadConfig(bus, slot, fun, 16 + (i * 4));

				if (baseAddress != 0) {
					HAL.DisableAllInterrupts();

					pciController.WriteConfig(bus, slot, fun, 16 + (i * 4), 0xFFFFFFFF);
					uint mask = pciController.ReadConfig(bus, slot, fun, 16 + (i * 4));
					pciController.WriteConfig(bus, slot, fun, 16 + (i * 4), baseAddress);

					HAL.EnableAllInterrupts();

					if (baseAddress % 2 == 1)
						addresses[i] = new PCIBaseAddress(AddressRegion.IO, baseAddress & 0x0000FFF8, (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
					else
						addresses[i] = new PCIBaseAddress(AddressRegion.Memory, baseAddress & 0xFFFFFFF0, ~(mask & 0xFFFFFFF0) + 1, ((baseAddress & 0x08) == 1));
				}
			}
		}

		public bool Start(IDeviceManager deviceManager, IResourceManager resourceManager, PCIHardwareDevice pciHardwareDevice)
		{

			//pciHardwareDevice.AssignBusResources(); // TODO!

			pciHardwareDevice.Activate(deviceManager);

			base.deviceStatus = pciHardwareDevice.Status;

			return (base.deviceStatus == DeviceStatus.Online);
		}

		public void SetNoDriverFound()
		{
			base.deviceStatus = DeviceStatus.NotFound;
		}

	}
}