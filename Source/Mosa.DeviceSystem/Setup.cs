/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem.PCI;
using System.Collections.Generic;

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
		static private PCIControllerManager pciControllerManager;

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

			// Create the PCI Controller Manager
			pciControllerManager = new PCIControllerManager(deviceManager);
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			// Find all drviers
			deviceDriverRegistry.RegisterBuiltInDeviceDrivers();

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
			pciControllerManager.CreatePCIDevices();

			foreach (var device in deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable()))
			{
				StartDevice(device as IPCIDevice);
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

			var hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

			StartDevice(pciDevice, deviceDriver, hardwareDevice);
		}

		static private void StartDevice(IPCIDevice pciDevice, DeviceDriver deviceDriver, IHardwareDevice hardwareDevice)
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
					var memory = HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
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
		static public void StartDevice(DeviceDriver deviceDriver)
		{
			var driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

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
						IMemory memory = HAL.AllocateMemory(memoryAttribute.MemorySize, memoryAttribute.MemoryAlignment);
						memoryRegions.AddLast(new MemoryRegion(memory.Address, memory.Size));
					}

				var hardwareResources = new HardwareResources(resourceManager, ioPortRegions.ToArray(), memoryRegions.ToArray(), new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

				hardwareDevice.Setup(hardwareResources);

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
