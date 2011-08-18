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
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.ISA;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Setup for the Device Driver System.
	/// </summary>
	public static class Setup2
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

		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize()
		{
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
			//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
			ISADeviceDriverAttribute keyboardDeviceAttributes = new ISADeviceDriverAttribute();
			keyboardDeviceAttributes.AutoLoad = true;
			keyboardDeviceAttributes.BasePort = 0x60;
			keyboardDeviceAttributes.PortRange = 1;
			keyboardDeviceAttributes.AltBasePort = 0x64;
			keyboardDeviceAttributes.IRQ = 1;
			keyboardDeviceAttributes.Platforms = PlatformArchitecture.X86AndX64;

			StandardKeyboard keyboard = new StandardKeyboard();

			StartDevice(keyboardDeviceAttributes, keyboard);
		}

		/// <summary>
		/// Starts the device.
		/// </summary>
		/// <param name="deviceDriver">The device driver.</param>
		static public void StartDevice(ISADeviceDriverAttribute driverAtttribute, IHardwareDevice hardwareDevice)
		{
			IIOPortRegion[] ioPortRegions = new IIOPortRegion[driverAtttribute.AltBasePort != 0x00 ? 1 : 2];
			IMemoryRegion[] memoryRegions = new IMemoryRegion[driverAtttribute.BaseAddress != 0x00 ? 0 : 1];

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
