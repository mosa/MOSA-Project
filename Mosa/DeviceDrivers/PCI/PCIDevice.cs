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
		protected PCIBaseAddress[] pciBaseAddresses;
		protected byte memoryRegionCount;
		protected byte ioPortRegionCount;

		protected IPCIController pciController;

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

		public PCIBaseAddress[] BaseAddresses { get { return pciBaseAddresses; } }

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

			this.pciBaseAddresses = new PCIBaseAddress[6];

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

					if (baseAddress % 2 == 1) {
						pciBaseAddresses[i] = new PCIBaseAddress(AddressRegion.IO, baseAddress & 0x0000FFF8, (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
						ioPortRegionCount++;
					}
					else {
						pciBaseAddresses[i] = new PCIBaseAddress(AddressRegion.Memory, baseAddress & 0xFFFFFFF0, ~(mask & 0xFFFFFFF0) + 1, ((baseAddress & 0x08) == 1));
						memoryRegionCount++;
					}
				}
			}
		}

		public bool Start(IDeviceManager deviceManager, IResourceManager resourceManager, PCIHardwareDevice pciHardwareDevice)
		{
			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioPortRegionCount];
			IMemoryRegion[] memoryRegion = new IMemoryRegion[memoryRegionCount];

			int ioRegions = 0;
			int memoryRegions = 0;

			foreach (PCIBaseAddress pciBaseAddress in pciBaseAddresses)
				switch (pciBaseAddress.Region) {
					case AddressRegion.IO: ioPortRegions[ioRegions++] = new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size); break;
					case AddressRegion.Memory: memoryRegion[memoryRegions++] = new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size); break;
					default: break;
				}

			IBusResources busResources = new BusResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, IRQ, pciHardwareDevice));

			pciHardwareDevice.AssignBusResources(busResources);

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