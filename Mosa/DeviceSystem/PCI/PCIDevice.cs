/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public class PCIDevice : Device, IDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected IPCIController pciController;

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
		protected BaseAddress[] baseAddresses;
		/// <summary>
		/// 
		/// </summary>
		protected byte memoryRegionCount;
		/// <summary>
		/// 
		/// </summary>
		protected byte ioPortRegionCount;

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
		public ushort VendorID { get { return pciController.ReadConfig16(Bus, Slot, Function, 0x00); } }
		/// <summary>
		/// Gets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		public ushort DeviceID { get { return pciController.ReadConfig16(Bus, Slot, Function, 0x02); } }
		/// <summary>
		/// Gets the revision ID.
		/// </summary>
		/// <value>The revision ID.</value>
		public byte RevisionID { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x08); } }
		/// <summary>
		/// Gets the class code.
		/// </summary>
		/// <value>The class code.</value>
		public byte ClassCode { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x0A); } }
		/// <summary>
		/// Gets the prog IF.
		/// </summary>
		/// <value>The prog IF.</value>
		public byte ProgIF { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x0B); } }
		/// <summary>
		/// Gets the sub class code.
		/// </summary>
		/// <value>The sub class code.</value>
		public byte SubClassCode { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x09); } }
		/// <summary>
		/// Gets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		public ushort SubVendorID { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x0C); } }
		/// <summary>
		/// Gets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		public ushort SubDeviceID { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x0E); } }
		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ { get { return pciController.ReadConfig8(Bus, Slot, Function, 0x3c); } }

		/// <summary>
		/// Gets the base addresses.
		/// </summary>
		/// <value>The base addresses.</value>
		public BaseAddress[] BaseAddresses { get { return baseAddresses; } }

		/// <summary>
		/// Create a new PCIDevice instance at the selected PCI address
		/// </summary>
		/// <param name="pciController">The pci controller.</param>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="fun">The fun.</param>
		public PCIDevice(IPCIController pciController, byte bus, byte slot, byte fun)
		{
			base.parent = pciController as Device;
			base.name = base.parent.Name + "/" + bus.ToString() + "." + slot.ToString() + "." + fun.ToString();
			base.deviceStatus = DeviceStatus.Available;

			this.pciController = pciController;
			this.bus = bus;
			this.slot = slot;
			this.function = fun;

			ioPortRegionCount = memoryRegionCount = 0;
			this.baseAddresses = new BaseAddress[8];

			for (byte i = 0; i < 6; i++) {
				uint address = pciController.ReadConfig32(bus, slot, fun, (byte)(16 + (i * 4)));

				if (address != 0) {
					HAL.DisableAllInterrupts();

					pciController.WriteConfig32(bus, slot, fun, (byte)(16 + (i * 4)), 0xFFFFFFFF);
					uint mask = pciController.ReadConfig32(bus, slot, fun, (byte)(16 + (i * 4)));
					pciController.WriteConfig32(bus, slot, fun, (byte)(16 + (i * 4)), address);

					HAL.EnableAllInterrupts();

					if (address % 2 == 1)
						baseAddresses[i] = new BaseAddress(AddressRegion.IO, address & 0x0000FFF8, (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
					else
						baseAddresses[i] = new BaseAddress(AddressRegion.Memory, address & 0xFFFFFFF0, ~(mask & 0xFFFFFFF0) + 1, ((address & 0x08) == 1));
				}
			}

			if ((ClassCode == 0x03) && (SubClassCode == 0x00) && (ProgIF == 0x00)) {
				// Special case for generic VGA
				baseAddresses[6] = new BaseAddress(AddressRegion.Memory, 0xA0000, 0x1FFFF, false);
				baseAddresses[7] = new BaseAddress(AddressRegion.IO, 0x3B0, 0x0F, false);
			}

			foreach (BaseAddress baseAddress in baseAddresses)
				if (baseAddress != null)
					switch (baseAddress.Region) {
						case AddressRegion.IO: ioPortRegionCount++; break;
						case AddressRegion.Memory: memoryRegionCount++; break;
					}
		}

		/// <summary>
		/// Gets the resources.
		/// </summary>
		/// <param name="hardwareDevice">The hardware device.</param>
		/// <param name="deviceManager">The device manager.</param>
		/// <param name="resourceManager">The resource manager.</param>
		/// <returns></returns>
		public IHardwareResources GetResources(IHardwareDevice hardwareDevice, IDeviceManager deviceManager, IResourceManager resourceManager)
		{
			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioPortRegionCount];
			IMemoryRegion[] memoryRegion = new IMemoryRegion[memoryRegionCount];

			int ioRegions = 0;
			int memoryRegions = 0;

			foreach (BaseAddress pciBaseAddress in baseAddresses)
				switch (pciBaseAddress.Region) {
					case AddressRegion.IO: ioPortRegions[ioRegions++] = new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size); break;
					case AddressRegion.Memory: memoryRegion[memoryRegions++] = new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size); break;
					default: break;
				}

			return new HardwareResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, IRQ, hardwareDevice));
		}

		/// <summary>
		/// Starts the specified hardware device.
		/// </summary>
		/// <param name="hardwareDevice">The hardware device.</param>
		/// <param name="deviceManager">The device manager.</param>
		/// <param name="resourceManager">The resource manager.</param>
		/// <returns></returns>
		public bool Start(IHardwareDevice hardwareDevice, IDeviceManager deviceManager, IResourceManager resourceManager)
		{
			IHardwareResources hardwareResources = GetResources(hardwareDevice, deviceManager, resourceManager);

			if (resourceManager.ClaimResources(hardwareResources)) {
				hardwareResources.EnableIRQ();
				if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
					base.deviceStatus = hardwareDevice.Status;
				else {
					hardwareResources.DisableIRQ();
					resourceManager.ReleaseResources(hardwareResources);
				}
			}
			else
				base.deviceStatus = DeviceStatus.Error;

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