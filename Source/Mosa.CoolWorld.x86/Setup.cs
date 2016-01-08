// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using System.Collections.Generic;
using Mosa.FileSystem.FAT;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private DeviceDriverRegistry deviceDriverRegistry;
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;
		static private PCIControllerManager pciControllerManager;
		static private PartitionManager partitionManager;

		/// <summary>
		/// Gets the device driver library
		/// </summary>
		/// <value>The device driver library.</value>
		static public DeviceDriverRegistry DeviceDriverRegistry { get { return deviceDriverRegistry; } }

		/// <summary>
		/// Gets the device manager.
		/// </summary>
		/// <value>The device manager.</value>
		static public IDeviceManager DeviceManager { get { return deviceManager; } }

		/// <summary>
		/// Gets the resource manager.
		/// </summary>
		/// <value>The resource manager.</value>
		static public IResourceManager ResourceManager { get { return resourceManager; } }

		/// <summary>
		/// Gets the PCI Controller Manager
		/// </summary>
		static public PCIControllerManager PCIControllerManager { get { return pciControllerManager; } }

		static public PartitionManager PartitionManager { get { return partitionManager; } }

		static public StandardKeyboard Keyboard = null;

		static public CMOS CMOS = null;

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create Resource Manager
			resourceManager = new ResourceManager();

			// Create Device Manager
			deviceManager = new DeviceManager();

			// Create the Device Driver Manager
			deviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.X86);

			// Create the PCI Controller Manager
			pciControllerManager = new PCIControllerManager(deviceManager);

			partitionManager = new PartitionManager(deviceManager);

			// Setup hardware abstraction interface
			var hardwareAbstraction = new Mosa.CoolWorld.x86.HAL.HardwareAbstraction();

			// Set device driver system to the hardware HAL
			Mosa.DeviceSystem.HAL.SetHardwareAbstraction(hardwareAbstraction);

			// Set the interrupt handler
			Mosa.DeviceSystem.HAL.SetInterruptHandler(ResourceManager.InterruptManager.ProcessInterrupt);
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
			CMOS = (CMOS)deviceManager.GetDevices(new FindDevice.WithName("CMOS")).First.Value;
			Keyboard = (StandardKeyboard)deviceManager.GetDevices(new FindDevice.WithName("StandardKeyboard")).First.Value;

			Boot.Console.Write("Finding disk controllers...");
			var diskcontroller = new DiskControllerManager(Setup.DeviceManager);
			diskcontroller.CreateDiskDevices();

			var diskcontrollers = deviceManager.GetDevices(new FindDevice.IsDiskControllerDevice());
			Boot.Console.WriteLine("[Completed: " + diskcontrollers.Count.ToString() + " found]");
			foreach (var device in diskcontrollers)
			{
				Boot.Console.Write("Found controller ");
				Boot.InBrackets(device.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.WriteLine();
			}

			Boot.Console.Write("Finding disks...");
			var disks = deviceManager.GetDevices(new FindDevice.IsDiskDevice());
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
			var partitions = deviceManager.GetDevices(new FindDevice.IsPartitionDevice());
			Boot.Console.WriteLine("[Completed: " + partitions.Count.ToString() + " found]");
			foreach (var partition in partitions)
			{
				Boot.Console.Write("Opening partition: ");
				Boot.InBrackets(partition.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.Write(" " + (partition as IPartitionDevice).BlockCount.ToString() + " blocks");
				Boot.Console.WriteLine();
			}

			Boot.Console.Write("Finding file systems...");
			var filesystem = deviceManager.GetDevices(new FindDevice.IsPartitionDevice());

			//Boot.Console.WriteLine("[Completed: " + filesystem.Count.ToString() + " found]");
			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition as IPartitionDevice);

				if (fat.IsValid)
				{
					Boot.Console.WriteLine("Found a FAT file system!");
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

			var devices = deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable());

			Boot.Console.Write(devices.Count.ToString());
			Boot.Console.WriteLine(" Devices");

			foreach (var device in devices)
			{
				var pciDevice = device as IPCIDevice;

				Boot.Console.WriteLine(device.Name + ": " + pciDevice.VendorID.ToString("x") + "." + pciDevice.DeviceID.ToString("x") + "." + pciDevice.Function.ToString("x") + "." + pciDevice.ClassCode.ToString("x"));

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

			StartDevice(pciDevice, deviceDriver, hardwareDevice as IHardwareDevice);

			//if (pciDevice.VendorID == 0x15AD && pciDevice.DeviceID == 0x0405)
			//{
			//	var display = hardwareDevice as IPixelGraphicsDevice;

			//	var color = new Color(255, 0, 0);

			//	display.WritePixel(color, 100, 100);
			//}
		}

		private static void StartDevice(IPCIDevice pciDevice, Mosa.DeviceSystem.DeviceDriver deviceDriver, IHardwareDevice hardwareDevice)
		{
			var ioPortRegions = new LinkedList<IIOPortRegion>();
			var memoryRegions = new LinkedList<IMemoryRegion>();

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
					var memory = Mosa.DeviceSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
				}
			}

			var hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, pciDevice.IRQ, hardwareDevice), pciDevice as IDeviceResource);

			if (resourceManager.ClaimResources(hardwareResources))
			{
				hardwareResources.EnableIRQ();
				hardwareDevice.Setup(hardwareResources);

				if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
				{
					pciDevice.SetDeviceOnline();
				}
				else
				{
					hardwareResources.DisableIRQ();
					resourceManager.ReleaseResources(hardwareResources);
				}
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
		static public void StartDevice(Mosa.DeviceSystem.DeviceDriver deviceDriver)
		{
			var driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

			// Don't load the VGAText and PIC drivers
			if (driverAtttribute.BasePort == 0x03B0 || driverAtttribute.BasePort == 0x20)
				return;

			if (driverAtttribute.AutoLoad)
			{
				var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

				var ioPortRegions = new LinkedList<IIOPortRegion>();
				var memoryRegions = new LinkedList<IMemoryRegion>();

				ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange));

				if (driverAtttribute.AltBasePort != 0x00)
					ioPortRegions.AddLast(new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange));

				if (driverAtttribute.BaseAddress != 0x00)
					memoryRegions.AddLast(new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange));

				foreach (var memoryAttribute in deviceDriver.MemoryAttributes)
					if (memoryAttribute.MemorySize > 0)
					{
						IMemory memory = Mosa.DeviceSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
						memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
					}

				var hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

				hardwareDevice.Setup(hardwareResources);

				Boot.Console.Write("Adding device ");
				Boot.InBrackets(hardwareDevice.Name, Mosa.Kernel.x86.Colors.White, Mosa.Kernel.x86.Colors.LightGreen);
				Boot.Console.WriteLine();

				if (resourceManager.ClaimResources(hardwareResources))
				{
					hardwareResources.EnableIRQ();

					if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
					{
						deviceManager.Add(hardwareDevice);
					}
					else
					{
						hardwareResources.DisableIRQ();
						resourceManager.ReleaseResources(hardwareResources);
					}
				}
			}
		}
	}
}
