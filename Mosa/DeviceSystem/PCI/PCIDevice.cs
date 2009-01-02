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
	public class PCIDevice : Device, IDevice, IPCIDevice, IPCIDeviceResource
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
		public PCIBaseAddress[] PCIBaseAddresses { get { return pciBaseAddresses; } }

		/// <summary>
		/// Create a new PCIDevice instance
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
			this.pciBaseAddresses = new PCIBaseAddress[8];

			for (byte i = 0; i < 6; i++) {
				uint address = pciController.ReadConfig32(bus, slot, fun, (byte)(16 + (i * 4)));

				if (address != 0) {
					HAL.DisableAllInterrupts();

					pciController.WriteConfig32(bus, slot, fun, (byte)(16 + (i * 4)), 0xFFFFFFFF);
					uint mask = pciController.ReadConfig32(bus, slot, fun, (byte)(16 + (i * 4)));
					pciController.WriteConfig32(bus, slot, fun, (byte)(16 + (i * 4)), address);

					HAL.EnableAllInterrupts();

					if (address % 2 == 1)
						pciBaseAddresses[i] = new PCIBaseAddress(PCIAddressType.IO, address & 0x0000FFF8, (~(mask & 0xFFF8) + 1) & 0xFFFF, false);
					else
						pciBaseAddresses[i] = new PCIBaseAddress(PCIAddressType.Memory, address & 0xFFFFFFF0, ~(mask & 0xFFFFFFF0) + 1, ((address & 0x08) == 1));
				}
			}

			if ((ClassCode == 0x03) && (SubClassCode == 0x00) && (ProgIF == 0x00)) {
				// Special case for generic VGA
				pciBaseAddresses[6] = new PCIBaseAddress(PCIAddressType.Memory, 0xA0000, 0x1FFFF, false);
				pciBaseAddresses[7] = new PCIBaseAddress(PCIAddressType.IO, 0x3B0, 0x0F, false);
			}

			foreach (PCIBaseAddress baseAddress in pciBaseAddresses)
				if (baseAddress != null)
					switch (baseAddress.Region) {
						case PCIAddressType.IO: ioPortRegionCount++; break;
						case PCIAddressType.Memory: memoryRegionCount++; break;
					}
		}

		/// <summary>
		/// Gets or sets the status register.
		/// </summary>
		/// <value>The status.</value>
		public ushort StatusRegister
		{
			get { return pciController.ReadConfig16(bus, slot, function, 0x04); }
			set { pciController.WriteConfig16(bus, slot, function, 0x04, value); }
		}

		/// <summary>
		/// Gets or sets the command register.
		/// </summary>
		/// <value>The status.</value>
		public ushort CommandRegister
		{
			get { return pciController.ReadConfig16(bus, slot, function, 0x06); }
			set { pciController.WriteConfig16(bus, slot, function, 0x06, value); }
		}

		/// <summary>
		/// Sets the no driver found.
		/// </summary>
		public void SetNoDriverFound()
		{
			deviceStatus = DeviceStatus.NotFound;
		}

		/// <summary>
		/// Sets the device online.
		/// </summary>
		public void SetDeviceOnline()
		{
			deviceStatus = DeviceStatus.Online;
		}

	}
}