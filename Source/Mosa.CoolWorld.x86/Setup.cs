/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.Kernel.x86;

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

		static public StandardKeyboard Keyboard = null;
		static public PCIController PCI = null;
		static public PIC PIC = null;
		static public PIT PIT = null;
		static public VGAText VGAText = null;

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

			// Setup hardware abstraction interface
			IHardwareAbstraction hardwareAbstraction = new Mosa.CoolWorld.x86.HAL.HardwareAbstraction();

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
			// Start drivers for ISA devices
			StartISADevices();

			// Start drivers for PCI devices
			StartPCIDevices();
		}

		/// <summary>
		/// Starts the PCI devices.
		/// </summary>
		static public void StartPCIDevices()
		{
			//foreach (IDevice device in deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable()))
			//{
			//    StartDevice(device as IPCIDevice);
			//}
		}

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			//LinkedList<DeviceDriver> deviceDrivers = deviceDriverRegistry.GetISADeviceDrivers();

			//foreach (DeviceDriver deviceDriver in deviceDrivers)
			//    StartDevice(deviceDriver);

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute keyboardDeviceAttributes = new ISADeviceDriverAttribute();
			keyboardDeviceAttributes.AutoLoad = true;
			keyboardDeviceAttributes.BasePort = 0x60;
			keyboardDeviceAttributes.PortRange = 1;
			keyboardDeviceAttributes.AltBasePort = 0x64;
			keyboardDeviceAttributes.IRQ = 1;
			keyboardDeviceAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute pciAttributes = new ISADeviceDriverAttribute();
			pciAttributes.AutoLoad = true;
			pciAttributes.BasePort = 0x0CF8;
			pciAttributes.PortRange = 8;
			pciAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x20, PortRange = 2, AltBasePort = 0xA0, AltPortRange = 2, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute picAttributes = new ISADeviceDriverAttribute();
			picAttributes.AutoLoad = true;
			picAttributes.BasePort = 0x20;
			picAttributes.PortRange = 2;
			picAttributes.AltBasePort = 0xA0;
			picAttributes.AltPortRange = 2;
			picAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute pitAttributes = new ISADeviceDriverAttribute();
			pitAttributes.AutoLoad = true;
			pitAttributes.BasePort = 0x40;
			pitAttributes.PortRange = 4;
			pitAttributes.IRQ = 0;
			pitAttributes.Platforms = PlatformArchitecture.X86AndX64;

			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute vgaTextAttributes = new ISADeviceDriverAttribute();
			vgaTextAttributes.AutoLoad = true;
			vgaTextAttributes.BasePort = 0x03B0;
			vgaTextAttributes.PortRange = 0x1F;
			vgaTextAttributes.BaseAddress = 0xB0000;
			vgaTextAttributes.AddressRange = 0x10000;
			vgaTextAttributes.IRQ = 0;
			vgaTextAttributes.Platforms = PlatformArchitecture.X86AndX64;

			Keyboard = new StandardKeyboard();
			PCI = new PCIController();
			PIC = new PIC();
			PIT = new PIT();
			VGAText = new VGAText();

			//StartDevice(picAttributes, PIC);
			StartDevice(pitAttributes, PIT);
			StartDevice(pciAttributes, PCI);
			StartDevice(keyboardDeviceAttributes, Keyboard);
			//StartDevice(vgaTextAttributes, VGAText);

			PCIControllerManager pciController = new PCIControllerManager(deviceManager);

			Boot.Console.Write("Probing PCI devices...");
			//pciController.CreatePCIDevices();
			Boot.Console.WriteLine("[Completed]");
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(ISADeviceDriverAttribute driverAtttribute, IHardwareDevice hardwareDevice)
		{
			int ioRegionCount = 1;
			int memoryRegionCount = 0;

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioRegionCount++;
			}

			if (driverAtttribute.BaseAddress != 0x00)
				memoryRegionCount++;

			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioRegionCount];
			IMemoryRegion[] memoryRegions = new IMemoryRegion[memoryRegionCount];

			ioPortRegions[0] = new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange);

			if (driverAtttribute.AltBasePort != 0x00)
			{
				ioPortRegions[1] = new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange);
			}

			if (driverAtttribute.BaseAddress != 0x00)
			{
				memoryRegions[0] = new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange);
			}

			IHardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegions, new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

			hardwareDevice.Setup(hardwareResources);

			Mosa.CoolWorld.x86.Boot.BulletPoint();
			Boot.Console.Write("Adding device ");
			Boot.InBrackets(hardwareDevice.Name, Colors.White, Colors.LightGreen);
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

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		static public void StartDevice(IPCIDevice pciDevice)
		{
			DeviceDriver deviceDriver = deviceDriverRegistry.FindDriver(pciDevice);

			if (deviceDriver == null)
			{
				pciDevice.SetNoDriverFound();
				return;
			}

			IHardwareDevice hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

			int ioRegionCount = 0;
			int memoryRegionCount = 0;

			foreach (BaseAddress pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioRegionCount++; break;
					case AddressType.Memory: memoryRegionCount++; break;
					default: break;
				}
			}

			foreach (DeviceDriverPhysicalMemoryAttribute memoryAttribute in deviceDriver.MemoryAttributes)
			{
				if (memoryAttribute.MemorySize > 0)
				{
					memoryRegionCount++;
				}
			}

			IIOPortRegion[] ioPortRegions = new IIOPortRegion[ioRegionCount];
			IMemoryRegion[] memoryRegions = new IMemoryRegion[memoryRegionCount];

			int ioRegionIndex = 0;
			int memoryRegionIndex = 0;

			foreach (BaseAddress pciBaseAddress in pciDevice.BaseAddresses)
			{
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioPortRegions[ioRegionIndex++] = new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size); break;
					case AddressType.Memory: memoryRegions[memoryRegionIndex++] = new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size); break;
					default: break;
				}
			}

			foreach (DeviceDriverPhysicalMemoryAttribute memoryAttribute in deviceDriver.MemoryAttributes)
			{
				if (memoryAttribute.MemorySize > 0)
				{
					IMemory memory = Mosa.DeviceSystem.HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions[memoryRegionIndex++] = new MemoryRegion(memory.Address, memory.Size);
				}
			}

			HardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegions, new InterruptHandler(resourceManager.InterruptManager, pciDevice.IRQ, hardwareDevice), pciDevice as IDeviceResource);

			if (resourceManager.ClaimResources(hardwareResources))
			{
				hardwareResources.EnableIRQ();
				if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
					pciDevice.SetDeviceOnline();
				else
				{
					hardwareResources.DisableIRQ();
					resourceManager.ReleaseResources(hardwareResources);
				}
			}
		}
	}
}
