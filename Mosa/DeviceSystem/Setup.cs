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
			deviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.x86);

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
			foreach (IDevice device in deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable())) {
				PCIDevice pciDevice = device as PCIDevice;

				DeviceDriver deviceDriver = deviceDriverRegistry.FindDriver(pciDevice);

				if (deviceDriver != null) {
					IHardwareDevice hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

					if (hardwareDevice != null)
						pciDevice.Start(hardwareDevice, deviceManager, resourceManager);
					else
						pciDevice.SetNoDriverFound();
				}
			}
		}

		/// <summary>
		/// Starts the ISA devices.
		/// </summary>
		static public void StartISADevices()
		{
			LinkedList<DeviceDriver> deviceDrivers = deviceDriverRegistry.GetISADeviceDrivers();

			foreach (DeviceDriver deviceDriver in deviceDrivers) {
				ISADeviceDriverAttribute driverAtttribute = deviceDriver.Attribute as ISADeviceDriverAttribute;

				if (driverAtttribute.AutoLoad) {
					IIOPortRegion[] ioPortRegions = new IIOPortRegion[(driverAtttribute.AltBasePort != 0x00) ? 2 : 1];
					IMemoryRegion[] memoryRegion = new IMemoryRegion[(driverAtttribute.BaseAddress != 0x00) ? 1 : 0];

					ioPortRegions[0] = new IOPortRegion(driverAtttribute.BasePort, driverAtttribute.PortRange);

					if (driverAtttribute.AltBasePort != 0x00)
						ioPortRegions[1] = new IOPortRegion(driverAtttribute.AltBasePort, driverAtttribute.AltPortRange);

					if (driverAtttribute.BaseAddress != 0x00)
						memoryRegion[0] = new MemoryRegion(driverAtttribute.BaseAddress, driverAtttribute.AddressRange);

					IHardwareDevice hardwareDevice = System.Activator.CreateInstance(deviceDriver.DriverType) as IHardwareDevice;

					IHardwareResources hardwareResources = new HardwareResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, driverAtttribute.IRQ, hardwareDevice));

					hardwareDevice.Setup(hardwareResources);

					if (resourceManager.ClaimResources(hardwareResources)) {
						hardwareResources.EnableIRQ();
						if (hardwareDevice.Start() == DeviceDriverStartStatus.Started) {
							deviceManager.Add(hardwareDevice);

							//LinkedList<IDevice> devices = hardwareDevice.CreateSubDevices();

							//if (devices != null)
							//    foreach (IDevice device in devices)
							//        deviceManager.Add(device);
						}
						else {
							hardwareResources.DisableIRQ();
							resourceManager.ReleaseResources(hardwareResources);
						}
					}
				}
			}
		}

	}
}
