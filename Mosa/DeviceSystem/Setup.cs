/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup
	{
		static private IDeviceManager deviceManager;
		static private IResourceManager resourceManager;

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

		static private ISA.ISARegistry isaDeviceDrivers;
		static private PCI.PCIRegistry pciDeviceDrivers;

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
			// Create Resource Manager
			resourceManager = new ResourceManager();

			// Create Device Manager
			deviceManager = new DeviceManager();

			// Setup ISA Driver Registry
			isaDeviceDrivers = new Mosa.DeviceSystem.ISA.ISARegistry(PlatformArchitecture.x86);
			// Load registry with build-in drivers
			isaDeviceDrivers.RegisterBuildInDeviceDrivers();

			// Setup PCI Driver Registry
			pciDeviceDrivers = new Mosa.DeviceSystem.PCI.PCIRegistry(PlatformArchitecture.x86);
			// Load registry with build-in drivers
			pciDeviceDrivers.RegisterBuildInDeviceDrivers();
		}

		/// <summary>
		/// Adds the driver assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		static public void AddDriverAssembly(System.Reflection.Assembly assembly)
		{
			isaDeviceDrivers.RegisterDeviceDrivers(assembly);
			pciDeviceDrivers.RegisterDeviceDrivers(assembly);
		}

		/// <summary>
		/// Start the Device Driver System.
		/// </summary>
		static public void Start()
		{
			// Start drivers for devices
			pciDeviceDrivers.StartDrivers(deviceManager, resourceManager);

			// Start drivers for devices
			isaDeviceDrivers.StartDrivers(deviceManager, resourceManager);
		}
	}
}
