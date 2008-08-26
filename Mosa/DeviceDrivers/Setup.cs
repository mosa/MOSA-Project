/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers
{
	public static class Setup
	{
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;

		static public IDeviceManager DeviceManager { get { return deviceManager; } }
		static public IResourceManager ResourceManager { get { return resourceManager; } }

		static public void Initialize()
		{
			// Create Resource Manager
			resourceManager = new ResourceManager();
	
			// Create Device Manager
			deviceManager = new DeviceManager();

			// Setup ISA Driver Registry
			ISA.ISARegistry isaDeviceDrivers = new Mosa.DeviceDrivers.ISA.ISARegistry(PlatformArchitecture.x86);
			// Load registry with build-in drivers
			isaDeviceDrivers.RegisterBuildInDeviceDrivers();
			// Start drivers for devices
			isaDeviceDrivers.StartDrivers(deviceManager, resourceManager);

			// Setup PCI Driver Registry
			PCI.PCIRegistry pciDeviceDrivers = new Mosa.DeviceDrivers.PCI.PCIRegistry(PlatformArchitecture.x86);
			// Load registry with build-in drivers
			pciDeviceDrivers.RegisterBuildInDeviceDrivers();
			// Start drivers for devices
			pciDeviceDrivers.StartDrivers(deviceManager, resourceManager);
		}

	}
}
