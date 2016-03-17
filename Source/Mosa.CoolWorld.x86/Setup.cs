// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using Mosa.HardwareSystem;
using Mosa.HardwareSystem.PCI;
using System.Collections.Generic;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private DeviceDriverRegistry deviceDriverRegistry;
		static private DeviceManager deviceManager;
		static private PCIControllerManager pciControllerManager;
		static private PartitionManager partitionManager;
		static private InterruptManager interruptManager;

		/// <summary>
		/// Gets the device driver library
		/// </summary>
		static public DeviceDriverRegistry DeviceDriverRegistry { get { return deviceDriverRegistry; } }

		/// <summary>
		/// Gets the device manager.
		/// </summary>
		static public DeviceManager DeviceManager { get { return deviceManager; } }

		/// <summary>
		/// Gets the interrupt manager.
		/// </summary>
		static public InterruptManager InterruptManager { get { return interruptManager; } }

		/// <summary>
		/// Gets the PCI Controller Manager
		/// </summary>
		static public PCIControllerManager PCIControllerManager { get { return pciControllerManager; } }

		static public PartitionManager PartitionManager { get { return partitionManager; } }

		static public StandardKeyboard StandardKeyboard = null;

		static public CMOS CMOS = null;

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create Device Manager
			deviceManager = new DeviceManager();

			// Create Interrupt Manager
			interruptManager = new InterruptManager();

			// Create the Device Driver Manager
			deviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.X86);

			// Create the PCI Controller Manager
			pciControllerManager = new PCIControllerManager(deviceManager);

			// Setup hardware abstraction interface
			var hardware = new Mosa.CoolWorld.x86.HAL.Hardware();

			// Set device driver system to the hardware HAL
			Mosa.HardwareSystem.HAL.SetHardwareAbstraction(hardware);

			// Set the interrupt handler
			Mosa.HardwareSystem.HAL.SetInterruptHandler(InterruptManager.ProcessInterrupt);

			partitionManager = new PartitionManager(deviceManager);
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			// Find all drivers
			Boot.Console.Write("Finding Drivers...");
			deviceDriverRegistry.RegisterBuiltInDeviceDrivers();
			var count = deviceDriverRegistry.GetPCIDeviceDrivers().Count + deviceDriverRegistry.GetISADeviceDrivers().Count;
			Boot.Console.WriteLine("[Completed: " + count.ToString() + " found]");

			// Start drivers for ISA devices
			StartISADevices();

			// Start drivers for PCI devices
			StartPCIDevices();

			// Get CMOS, StandardKeyboard, and PIC driver instances
			CMOS = (CMOS)deviceManager.GetDevices(new WithName("CMOS")).First.Value;
			StandardKeyboard = (StandardKeyboard)deviceManager.GetDevices(new WithName("StandardKeyboard")).First.Value;

			Boot.Console.Write("Finding disk controllers...");
			var diskcontroller = new DiskControllerManager(Setup.DeviceManager);
			diskcontroller.CreateDiskDevices();

			var diskcontrollers = deviceManager.GetDevices(new IsDiskControllerDevice());
			Boot.Console.WriteLine("[Completed: " + diskcontrollers.Count.ToString() + " found]");
			foreach (var device in diskcontrollers)
			{
				Boot.Console.Write("Found controller ");
				Boot.InBrackets(device.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.WriteLine();
			}

			Boot.Console.Write("Finding disks...");
			var disks = deviceManager.GetDevices(new IsDiskDevice());
			Boot.Console.WriteLine("[Completed: " + disks.Count.ToString() + " found]");
			foreach (var disk in disks)
			{
				Boot.Console.Write("Spinning up disk ");
				Boot.InBrackets(disk.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.Write(" " + (disk as IDiskDevice).TotalBlocks.ToString() + " blocks");
				Boot.Console.WriteLine();
			}

			partitionManager.CreatePartitionDevices();

			Boot.Console.Write("Finding partitions...");
			var partitions = deviceManager.GetDevices(new IsPartitionDevice());
			Boot.Console.WriteLine("[Completed: " + partitions.Count.ToString() + " found]");
			foreach (var partition in partitions)
			{
				Boot.Console.Write("Opening partition: ");
				Boot.InBrackets(partition.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.Write(" " + (partition as IPartitionDevice).BlockCount.ToString() + " blocks");
				Boot.Console.WriteLine();
			}

			Boot.Console.Write("Finding file systems...");
			var filesystem = deviceManager.GetDevices(new IsPartitionDevice());

			//Boot.Console.WriteLine("[Completed: " + filesystem.Count.ToString() + " found]");
			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition as IPartitionDevice);

				if (fat.IsValid)
				{
					Boot.Console.WriteLine("Found a FAT file system!");

					var filename = "TEST.TXT";

					var location = fat.FindEntry(filename);

					if (location.IsValid)
					{
						Boot.Console.WriteLine("Found: " + filename);

						var fatFileStream = new FatFileStream(fat, location);

						uint len = (uint)fatFileStream.Length;

						Boot.Console.WriteLine("Length: " + len.ToString());

						Boot.Console.WriteLine("Reading File:");

						for (;;)
						{
							int i = fatFileStream.ReadByte();

							if (i < 0)
								break;

							Boot.Console.Write((char)i);
						}
					}
				}
			}
		}

		/// <summary>
		/// Starts the PCI devices.
		/// </summary>
		static public void StartPCIDevices()
		{
			Boot.Console.Write("Probing PCI devices...");

			PCIControllerManager.CreatePCIDevices();

			Boot.Console.WriteLine("[Completed]");

			Boot.Console.Write("Starting PCI devices... ");

			var devices = deviceManager.GetDevices(new IsPCIDevice(), new IsAvailable());

			Boot.Console.Write(devices.Count.ToString());
			Boot.Console.WriteLine(" Devices");

			foreach (var device in devices)
			{
				var pciDevice = device as IPCIDevice;

				Boot.Console.WriteLine(device.Name + ": " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.Function.ToString("x") + ":" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubVendorID.ToString("x"));

				StartDevice(pciDevice);
			}
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		static public void StartDevice(IPCIDevice pciDevice)
		{
			var deviceDriver = deviceDriverRegistry.FindDriver(pciDevice);

			if (deviceDriver == null)
			{
				pciDevice.SetNoDriverFound();
				return;
			}

			var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType);

			if (hardwareDevice == null)
			{
				Mosa.Kernel.x86.Panic.Error("ERROR: hardwareDevice == null");
				return;
			}

			var iHardwareDevice = hardwareDevice as IHardwareDevice;

			if (iHardwareDevice == null)
			{
				Mosa.Kernel.x86.Panic.Error("ERROR: iHardwareDevice == null");
				return;
			}

			//Boot.Console.WriteLine("Found Driver");

			StartDevice(pciDevice, deviceDriver, iHardwareDevice);
		}

		private static void StartDevice(IPCIDevice pciDevice, Mosa.HardwareSystem.DeviceDriver deviceDriver, IHardwareDevice hardwareDevice)
		{
			var ioPortRegions = new LinkedList<IOPortRegion>();
			var memoryRegions = new LinkedList<MemoryRegion>();

			foreach (var pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioPortRegions.AddLast(new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size)); break;
					case AddressType.Memory: memoryRegions.AddLast(new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size)); break;
					default: break;
				}
			}

			foreach (var memoryAttribute in deviceDriver.MemoryAttributes)
			{
				if (memoryAttribute.MemorySize > 0)
				{
					var memory = Mosa.HardwareSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
				}
			}

			foreach (var ioportregion in ioPortRegions)
			{
				Boot.Console.WriteLine("  I/O: 0x" + ioportregion.BaseIOPort.ToString("X") + " [" + ioportregion.Size.ToString("X") + "]");
			}

			foreach (var memoryregion in memoryRegions)
			{
				Boot.Console.WriteLine("  Memory: 0x" + memoryregion.BaseAddress.ToString("X") + " [" + memoryregion.Size.ToString("X") + "]");
			}

			//Boot.Console.WriteLine("  Command: 0x" + hardwareDevice...ToString("X"));

			var hardwareResources = new HardwareResources(
				ioPortRegions.ToArray(),
				memoryRegions.ToArray(),
				new InterruptHandler(InterruptManager, pciDevice.IRQ, hardwareDevice),
				pciDevice as IPCIDeviceResource
			);

			hardwareDevice.Setup(hardwareResources);

			deviceManager.Add(hardwareDevice);

			hardwareResources.EnableIRQ();

			if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
			{
				pciDevice.SetDeviceOnline();
			}
		}

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			var deviceDrivers = deviceDriverRegistry.GetISADeviceDrivers();

			foreach (var deviceDriver in deviceDrivers)
			{
				StartDevice(deviceDriver);
			}
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(Mosa.HardwareSystem.DeviceDriver deviceDriver)
		{
			var driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

			// TEMP: Don't load the VGAText and PIC drivers
			if (driverAtttribute.BasePort == 0x03B0 || driverAtttribute.BasePort == 0x20)
				return;

			if (!driverAtttribute.AutoLoad)
				return;

			var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

			var ioPortRegions = new LinkedList<IOPortRegion>();
			var memoryRegions = new LinkedList<MemoryRegion>();

			ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange));

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange));
			}

			if (driverAtttribute.BaseAddress != 0x00)
			{
				memoryRegions.AddLast(new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange));
			}

			foreach (var memoryAttribute in deviceDriver.MemoryAttributes)
			{
				if (memoryAttribute.MemorySize > 0)
				{
					var memory = Mosa.HardwareSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
				}
			}

			var hardwareResources = new HardwareResources(
				ioPortRegions.ToArray(),
				memoryRegions.ToArray(),
				new InterruptHandler(InterruptManager, driverAtttribute.IRQ, hardwareDevice)
			);

			hardwareDevice.Setup(hardwareResources);

			Boot.Console.Write("Adding device ");
			Boot.InBrackets(hardwareDevice.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
			Boot.Console.WriteLine();

			deviceManager.Add(hardwareDevice);

			hardwareResources.EnableIRQ();

			hardwareDevice.Start();
		}
	}
}
