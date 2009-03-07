/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.FileSystem;
using Mosa.FileSystem.FAT;
using Mosa.EmulatedDevices.Synthetic;

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
		[STAThread]
		public static void Main(string[] args)
		{
			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new Mosa.EmulatedKernel.HardwareAbstraction();

			// Set device driver system to the emulator port and memory methods
			Mosa.DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Start the emulated devices
			Mosa.EmulatedDevices.Setup.Initialize();

			// Initialize the driver system
			Mosa.DeviceSystem.Setup.Initialize();

			// Registry device drivers
			Mosa.DeviceSystem.Setup.DeviceDriverRegistry.RegisterBuiltInDeviceDrivers();
			Mosa.DeviceSystem.Setup.DeviceDriverRegistry.RegisterDeviceDrivers(typeof(Mosa.DeviceDrivers.ISA.CMOS).Module.Assembly);

			// Set the interrupt handler
			Mosa.DeviceSystem.HAL.SetInterruptHandler(Mosa.DeviceSystem.Setup.ResourceManager.InterruptManager.ProcessInterrupt);

			// Start the driver system
			Mosa.DeviceSystem.Setup.Start();

			// Create pci controller manager
			PCIControllerManager pciControllerManager = new PCIControllerManager(Mosa.DeviceSystem.Setup.DeviceManager);

			// Create pci controller devices
			pciControllerManager.CreatePartitionDevices();

			// Create synthetic keyboard device
			Mosa.EmulatedDevices.Synthetic.Keyboard keyboard = new Mosa.EmulatedDevices.Synthetic.Keyboard();

			// Add the emulated keyboard device to the device drivers
			Mosa.DeviceSystem.Setup.DeviceManager.Add(keyboard);

			// Create synthetic graphic pixel device
			Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice pixelGraphicDevice = new Mosa.EmulatedDevices.Synthetic.PixelGraphicDevice(500, 500);

			// Added the synthetic graphic device to the device drivers
			Mosa.DeviceSystem.Setup.DeviceManager.Add(pixelGraphicDevice);

			// Create synthetic ram disk device
			Mosa.EmulatedDevices.Synthetic.RamDiskDevice ramDiskDevice = new Mosa.EmulatedDevices.Synthetic.RamDiskDevice(1024 * 1024 * 10 / 512);

			// Add emulated ram disk device to the device drivers
			Mosa.DeviceSystem.Setup.DeviceManager.Add(ramDiskDevice);

			// Create disk controller manager
			DiskControllerManager diskControllerManager = new DiskControllerManager(Mosa.DeviceSystem.Setup.DeviceManager);
	
			// Create disk devices from disk controller devices
			diskControllerManager.CreateDiskDevices();

			// Get the text VGA device
			LinkedList<IDevice> devices = Mosa.DeviceSystem.Setup.DeviceManager.GetDevices(new FindDevice.WithName("VGAText"));

			// Create a screen interface to the text VGA device
			ITextScreen screen = new TextScreen((ITextDevice)devices.First.value);

			// Create master boot block record
			MasterBootBlock mbr = new MasterBootBlock(ramDiskDevice);
			mbr.DiskSignature = 0x12345678;
			mbr.Partitions[0].Bootable = true;
			mbr.Partitions[0].StartLBA = 17;
			mbr.Partitions[0].TotalBlocks = ramDiskDevice.TotalBlocks - 17;
			mbr.Partitions[0].PartitionType = PartitionType.FAT12;
			mbr.Write();

			// Create partition device 
			PartitionDevice partitionDevice = new PartitionDevice(ramDiskDevice, mbr.Partitions[0], false);

			// Set FAT settings
			FatSettings fatSettings = new FatSettings();

			fatSettings.FATType = FatType.FAT12;
			fatSettings.FloppyMedia = false;
			fatSettings.VolumeLabel = "MOSADISK";
			fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };

			// Create FAT file system
			FatFileSystem fat12 = new FatFileSystem(partitionDevice);
			fat12.Format(fatSettings);

			// Create partition manager
			PartitionManager partitionManager = new PartitionManager(Mosa.DeviceSystem.Setup.DeviceManager);

			// Create partition devices
			partitionManager.CreatePartitionDevices();

			// Get a list of all devices
			devices = Mosa.DeviceSystem.Setup.DeviceManager.GetAllDevices();

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

				if (device is IPartitionDevice) {
					FileSystem.FAT.FatFileSystem fat = new Mosa.FileSystem.FAT.FatFileSystem(device as IPartitionDevice);

					screen.Write("  File System: ");
					if (fat.IsValid) {
						switch (fat.FATType) {
							case FatType.FAT12: screen.WriteLine("FAT12"); break;
							case FatType.FAT16: screen.WriteLine("FAT16"); break;
							case FatType.FAT32: screen.WriteLine("FAT32"); break;
							default: screen.WriteLine("Unknown"); break;
						}
						screen.WriteLine("  Volume Name: " + fat.VolumeLabel);
					}
					else
						screen.WriteLine("Unknown");
				}

				if (device is PCIDevice) {
					PCIDevice pciDevice = (device as PCIDevice);

					screen.Write("  Vendor:0x");
					screen.Write(pciDevice.VendorID.ToString("X"));
					screen.Write(" [");
					screen.Write(DeviceTable.Lookup(pciDevice.VendorID));
					screen.WriteLine("]");

					screen.Write("  Device:0x");
					screen.Write(pciDevice.DeviceID.ToString("X"));
					screen.Write(" Rev:0x");
					screen.Write(pciDevice.RevisionID.ToString("X"));
					screen.Write(" [");
					screen.Write(DeviceTable.Lookup(pciDevice.VendorID, pciDevice.DeviceID));
					screen.WriteLine("]");

					screen.Write("  Class:0x");
					screen.Write(pciDevice.ClassCode.ToString("X"));
					screen.Write(" [");
					screen.Write(ClassCodeTable.Lookup(pciDevice.ClassCode));
					screen.WriteLine("]");

					screen.Write("  SubClass:0x");
					screen.Write(pciDevice.SubClassCode.ToString("X"));
					screen.Write(" [");
					screen.Write(SubClassCodeTable.Lookup(pciDevice.ClassCode, pciDevice.SubClassCode, pciDevice.ProgIF));
					screen.WriteLine("]");

					//					screen.Write("  ");
					//					screen.WriteLine(DeviceTable.Lookup(pciDevice.VendorID, pciDevice.DeviceID, pciDevice.SubDeviceID, pciDevice.SubVendorID));

					foreach (PCIBaseAddress address in pciDevice.PCIBaseAddresses) {
						if (address == null)
							continue;

						if (address.Address == 0)
							continue;

						screen.Write("    ");

						if (address.Region == PCIAddressType.IO)
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

					if (pciDevice.IRQ != 0) {
						screen.Write("    ");
						screen.Write("IRQ at ");
						screen.Write(pciDevice.IRQ.ToString());
						screen.WriteLine();
					}
				}
			}


			//Key key = keyboard.GetKeyPressed();

			//return;
		}

	}
}
