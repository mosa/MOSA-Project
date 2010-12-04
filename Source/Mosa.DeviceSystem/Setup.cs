/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
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

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create the Device Driver Manager
			deviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.X86);

			// Create Resource Manager
			resourceManager = new ResourceManager();

			// Create Device Manager
			deviceManager = new DeviceManager();
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			// Start drivers for PCI devices
			StartPCIDevices();

			// Start drivers for ISA devices
			StartISADevices();
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

			// MR 07/21/09: Commenting out, causes mono xbuild to fail on MacOS X
			// PCIDeviceDriverAttribute attribute = deviceDriver.Attribute as PCIDeviceDriverAttribute;

			LinkedList<IIOPortRegion> ioPortRegions = new LinkedList<IIOPortRegion>();
			LinkedList<IMemoryRegion> memoryRegions = new LinkedList<IMemoryRegion>();

			foreach (BaseAddress pciBaseAddress in pciDevice.BaseAddresses)
				switch (pciBaseAddress.Region)
				{
					case AddressType.IO: ioPortRegions.Add(new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size)); break;
					case AddressType.Memory: memoryRegions.Add(new MemoryRegion(pciBaseAddress.Address, pciBaseAddress.Size)); break;
					default: break;
				}

			foreach (DeviceDriverPhysicalMemoryAttribute memoryAttribute in deviceDriver.MemoryAttributes)
				if (memoryAttribute.MemorySize > 0)
				{
					IMemory memory = HAL.RequestPhysicalMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
					memoryRegions.Add(new MemoryRegion(memory.Address, memory.Size));
				}

			HardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, pciDevice.IRQ, hardwareDevice), pciDevice as IDeviceResource);

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

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			LinkedList<DeviceDriver> deviceDrivers = deviceDriverRegistry.GetISADeviceDrivers();

			foreach (DeviceDriver deviceDriver in deviceDrivers)
				StartDevice(deviceDriver);
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(DeviceDriver deviceDriver)
		{
			ISADeviceDriverAttribute driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

			if (driverAtttribute.AutoLoad)
			{
				IHardwareDevice hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;
				ISADeviceDriverAttribute attribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

				LinkedList<IIOPortRegion> ioPortRegions = new LinkedList<IIOPortRegion>();
				LinkedList<IMemoryRegion> memoryRegions = new LinkedList<IMemoryRegion>();

				ioPortRegions.Add(new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange));

				if (driverAtttribute.AltBasePort != 0x00)
					ioPortRegions.Add(new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange));

				if (driverAtttribute.BaseAddress != 0x00)
					memoryRegions.Add(new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange));

				foreach (DeviceDriverPhysicalMemoryAttribute memoryAttribute in deviceDriver.MemoryAttributes)
					if (memoryAttribute.MemorySize > 0)
					{
						IMemory memory = HAL.RequestPhysicalMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
						memoryRegions.Add(new MemoryRegion(memory.Address, memory.Size));
					}

				IHardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

				hardwareDevice.Setup(hardwareResources);

				if (resourceManager.ClaimResources(hardwareResources))
				{
					hardwareResources.EnableIRQ();
					if (hardwareDevice.Start() == DeviceDriverStartStatus.Started)
						deviceManager.Add(hardwareDevice);
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
