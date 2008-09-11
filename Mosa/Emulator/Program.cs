/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.PCI;
using Mosa.ClassLib;

namespace Mosa.Emulator
{
	/// <summary>
	/// Program with CLR emulated devices
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		public static void Main(string[] args)
		{
			// Set Device Driver system to the emulator port and memory methods
			DeviceDrivers.Kernel.HAL.SetIOPortFactory(Mosa.EmulatedDevices.IOPortDispatch.RegisterIOPort);
			DeviceDrivers.Kernel.HAL.SetMemoryFactory(Mosa.EmulatedDevices.MemoryDispatch.RegisterMemory);

			// Start the emulated devices
			EmulatedDevices.Setup.Initialize();

			// Start driver system
			DeviceDrivers.Setup.Initialize();

			// Set the interrupt handler
			DeviceDrivers.Kernel.HAL.SetInterruptHandler(DeviceDrivers.Setup.ResourceManager.InterruptManager.ProcessInterrupt);

			// Create Emulated Keyboard device
			Mosa.EmulatedDevices.Keyboard keyboard = new Mosa.EmulatedDevices.Keyboard();

			// Added the emulated keyboard device to the device drivers
			DeviceDrivers.Setup.DeviceManager.Add(keyboard);

			// Get the Text VGA device
			LinkedList<IDevice> devices = DeviceDrivers.Setup.DeviceManager.GetDevices(new FindDevice.WithName("VGA"));

			// Create a screen interface to the Text VGA device
			ITextScreen screen = new TextScreen((ITextDevice)devices.First.value);

			// Get a list of all devices
			devices = DeviceDrivers.Setup.DeviceManager.GetAllDevices();

			// Print them 
			screen.WriteLine("Devices: ");
			foreach (IDevice device in devices) {

				screen.Write(device.Name);
				screen.Write(" [");

				switch (device.Status) {
					case DeviceStatus.Online: screen.Write("Online"); break;
					case DeviceStatus.Available: screen.Write("Available"); break;
					case DeviceStatus.Initializing: screen.Write("Initializing"); break;
					case DeviceStatus.NotFound: screen.Write("Not Found"); break;
					case DeviceStatus.Error: screen.Write("Error"); break;
				}
				screen.Write("]");

				if (device.Parent != null) {
					screen.Write(" - Parent: ");
					screen.Write(device.Parent.Name);
				}
				screen.WriteLine();

				if (device is PCIDevice) {
					PCIDevice pciDevice = (device as PCIDevice);

					screen.Write("  Vendor:0x");
					screen.Write(pciDevice.VendorID.ToString("X"));
					screen.Write(" Device:0x");
					screen.Write(pciDevice.DeviceID.ToString("X"));
					screen.Write(" Class:0x");
					screen.Write(pciDevice.ClassCode.ToString("X"));
					screen.Write(" Rev:0x");
					screen.Write(pciDevice.RevisionID.ToString("X"));
					screen.WriteLine();

					foreach (PCIBaseAddress address in pciDevice.BaseAddresses) {
						if (address.Address != 0) {
							screen.Write("    ");

							if (address.Region == PCIAddressRegion.IO)
								screen.Write("I/O Port at 0x");
							else
								screen.Write("Memory at 0x");

							screen.Write(address.Address.ToString("X"));

							screen.Write(" [size=");

							if ((address.Size & 0xFFFFF) == 0) {
								screen.Write((address.Size >> 20).ToString());
								screen.Write("M");
							}
							else if ((address.Size & 0x3FF) == 0) {
								screen.Write((address.Size >> 10).ToString());
								screen.Write("K");
							}
							else
								screen.Write(address.Size.ToString());

							screen.Write("]");

							if (address.Prefetchable)
								screen.Write("(prefetchable)");

							screen.WriteLine();
						}
					}

					if (pciDevice.IRQ != 0) {
						screen.Write("    ");
						screen.Write("IRQ at ");
						screen.Write(pciDevice.IRQ.ToString());
						screen.WriteLine();
					}
				}
			}

			Key key = keyboard.GetKeyPressed();

			return;
		}
	}
}
