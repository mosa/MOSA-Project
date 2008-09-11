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
    /// <summary>
    /// 
    /// </summary>
	public class PCIDevice : Device, IDevice
	{
        /// <summary>
        /// 
        /// </summary>
		protected byte bus;
        /// <summary>
        /// 
        /// </summary>
		protected byte slot;
        /// <summary>
        /// 
        /// </summary>
		protected byte function;
        /// <summary>
        /// 
        /// </summary>
		protected ushort vendorID;
        /// <summary>
        /// 
        /// </summary>
		protected ushort deviceID;
        /// <summary>
        /// 
        /// </summary>
		protected byte revisionID;
        /// <summary>
        /// 
        /// </summary>
		protected ushort classCode;
        /// <summary>
        /// 
        /// </summary>
		protected byte subClassCode;
        /// <summary>
        /// 
        /// </summary>
		protected ushort subVendorID;
        /// <summary>
        /// 
        /// </summary>
		protected ushort subDeviceID;
        /// <summary>
        /// 
        /// </summary>
		protected byte progIF;
        /// <summary>
        /// 
        /// </summary>
		protected byte irq;
        /// <summary>
        /// 
        /// </summary>
		protected PCIBaseAddress[] pciBaseAddresses;
        /// <summary>
        /// 
        /// </summary>
		protected byte memoryRegionCount;
        /// <summary>
        /// 
        /// </summary>
		protected byte ioPortRegionCount;

        /// <summary>
        /// 
        /// </summary>
		protected IPCIController pciController;

        /// <summary>
        /// Gets the bus.
        /// </summary>
        /// <value>The bus.</value>
		public byte Bus { get { return bus; } }
        /// <summary>
        /// Gets the slot.
        /// </summary>
        /// <value>The slot.</value>
		public byte Slot { get { return slot; } }
        /// <summary>
        /// Gets the function.
        /// </summary>
        /// <value>The function.</value>
		public byte Function { get { return function; } }
        /// <summary>
        /// Gets the vendor ID.
        /// </summary>
        /// <value>The vendor ID.</value>
		public ushort VendorID { get { return vendorID; } }
        /// <summary>
        /// Gets the device ID.
        /// </summary>
        /// <value>The device ID.</value>
		public ushort DeviceID { get { return deviceID; } }
        /// <summary>
        /// Gets the revision ID.
        /// </summary>
        /// <value>The revision ID.</value>
		public byte RevisionID { get { return revisionID; } }
        /// <summary>
        /// Gets the class code.
        /// </summary>
        /// <value>The class code.</value>
		public ushort ClassCode { get { return classCode; } }
        /// <summary>
        /// Gets the prog IF.
        /// </summary>
        /// <value>The prog IF.</value>
		public byte ProgIF { get { return progIF; } }
        /// <summary>
        /// Gets the sub class code.
        /// </summary>
        /// <value>The sub class code.</value>
		public byte SubClassCode { get { return subClassCode; } }
        /// <summary>
        /// Gets the sub vendor ID.
        /// </summary>
        /// <value>The sub vendor ID.</value>
		public ushort SubVendorID { get { return subVendorID; } }
        /// <summary>
        /// Gets the sub device ID.
        /// </summary>
        /// <value>The sub device ID.</value>
		public ushort SubDeviceID { get { return subDeviceID; } }
        /// <summary>
        /// Gets the IRQ.
        /// </summary>
        /// <value>The IRQ.</value>
		public byte IRQ { get { return irq; } }

        /// <summary>
        /// Gets the base addresses.
        /// </summary>
        /// <value>The base addresses.</value>
		public PCIBaseAddress[] BaseAddresses { get { return pciBaseAddresses; } }

		/// <summary>
		/// Create a new PCIDevice instance at the selected PCI address
		/// </summary>
		public PCIDevice(IPCIController pciController, byte bus, byte slot, byte fun)
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

			for (byte i = 0; i < 6; i++) {
				uint baseAddress = pciController.ReadConfig(bus, slot, fun, (byte)(16 + (i * 4)));

				if (baseAddress != 0) {
					HAL.DisableAllInterrupts();

					pciController.WriteConfig(bus, slot, fun, (byte)(16 + (i * 4)), 0xFFFFFFFF);
					uint mask = pciController.ReadConfig(bus, slot, fun, (byte)(16 + (i * 4)));
					pciController.WriteConfig(bus, slot, fun, (byte)(16 + (i * 4)), baseAddress);

					HAL.EnableAllInterrupts();

					if (baseAddress % 2 == 1) {
						pciBaseAddresses[i] = new PCIBaseAddress(PCIAddressRegion.IO, baseAddress & 0x0000FFF8, (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
						ioPortRegionCount++;
					}
					else {
						pciBaseAddresses[i] = new PCIBaseAddress(PCIAddressRegion.Memory, baseAddress & 0xFFFFFFF0, ~(mask & 0xFFFFFFF0) + 1, ((baseAddress & 0x08) == 1));
						memoryRegionCount++;
					}
				}
			}
		}

        /// <summary>
        /// Starts the specified device manager.
        /// </summary>
        /// <param name="deviceManager">The device manager.</param>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="pciHardwareDevice">The pci hardware device.</param>
        /// <returns></returns>
		public bool Start(IDeviceManager deviceManager, IResourceManager resourceManager, PCIHardwareDevice pciHardwareDevice)
		{
			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioPortRegionCount];
			IMemoryRegion[] memoryRegion = new IMemoryRegion[memoryRegionCount];

			int ioRegions = 0;
			int memoryRegions = 0;

			foreach (PCIBaseAddress pciBaseAddress in pciBaseAddresses)
				switch (pciBaseAddress.Region) {
					case PCIAddressRegion.IO: ioPortRegions[ioRegions++] = new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size); break;
					case PCIAddressRegion.Memory: memoryRegion[memoryRegions++] = new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size); break;
					default: break;
				}

			IBusResources busResources = new BusResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, IRQ, pciHardwareDevice));

			pciHardwareDevice.AssignBusResources(busResources);

			pciHardwareDevice.Activate(deviceManager);

			base.deviceStatus = pciHardwareDevice.Status;

			return (base.deviceStatus == DeviceStatus.Online);
		}

        /// <summary>
        /// Sets the no driver found.
        /// </summary>
		public void SetNoDriverFound()
		{
			base.deviceStatus = DeviceStatus.NotFound;
		}

	}
}