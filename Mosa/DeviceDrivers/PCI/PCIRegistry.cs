/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Reflection;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.PCI
{
    /// <summary>
    /// 
    /// </summary>
	public class PCIRegistry
	{
        /// <summary>
        /// 
        /// </summary>
		PlatformArchitecture platformArchitecture;

        /// <summary>
        /// 
        /// </summary>
		protected LinkedList<PCIDriverEntry> drivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PCIRegistry"/> class.
        /// </summary>
        /// <param name="platformArchitecture">The platform architecture.</param>
		public PCIRegistry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			drivers = new LinkedList<PCIDriverEntry>();
		}

        /// <summary>
        /// Adds the device driver.
        /// </summary>
        /// <param name="deviceDriverSignature">The device driver signature.</param>
        /// <param name="type">The type.</param>
		public void AddDeviceDriver(PCIDeviceSignatureAttribute deviceDriverSignature, Type type)
		{
			drivers.Add(new PCIDriverEntry(deviceDriverSignature, type));
		}

        /// <summary>
        /// Creates the device.
        /// </summary>
        /// <param name="pciDevice">The pci device.</param>
        /// <returns></returns>
		public PCIHardwareDevice CreateDevice(PCIDevice pciDevice)
		{
			Type deviceType = FindDriver(pciDevice);

			if (deviceType == null)
				return null;

			return Activator.CreateInstance(deviceType, pciDevice) as PCIHardwareDevice;
		}

        /// <summary>
        /// Finds the driver.
        /// </summary>
        /// <param name="pciDevice">The pci device.</param>
        /// <returns></returns>
		public Type FindDriver(PCIDevice pciDevice)
		{
			Type deviceType = null;
			int bestPriority = Int32.MaxValue;

			foreach (PCIDriverEntry entry in drivers) {
				if ((entry.SignatureAttribute.Priority != 0) && (entry.SignatureAttribute.Priority < bestPriority)) {
					if (entry.SignatureAttribute.CompareTo(pciDevice)) {
						deviceType = entry.DriverType;
						bestPriority = entry.SignatureAttribute.Priority;
					}
				}
			}

			return deviceType;
		}

        /// <summary>
        /// Registers the build in device drivers.
        /// </summary>
		public void RegisterBuildInDeviceDrivers()
		{
			Assembly assemblyInfo = typeof(PCIRegistry).Module.Assembly;
			RegisterDeviceDrivers(assemblyInfo);
		}

        /// <summary>
        /// Registers the device drivers.
        /// </summary>
        /// <param name="assemblyInfo">The assembly info.</param>
		public void RegisterDeviceDrivers(Assembly assemblyInfo)
		{
			Type[] types = assemblyInfo.GetTypes();

			foreach (Type type in types) {
				object[] attributes = type.GetCustomAttributes(typeof(PCIDeviceSignatureAttribute), false);

				foreach (object attribute in attributes)
					if (((attribute as PCIDeviceSignatureAttribute).Platforms & platformArchitecture) != 0)
						AddDeviceDriver(attribute as PCIDeviceSignatureAttribute, type);
			}
		}

        /// <summary>
        /// Starts the drivers.
        /// </summary>
        /// <param name="deviceManager">The device manager.</param>
        /// <param name="resourceManager">The resource manager.</param>
		public void StartDrivers(IDeviceManager deviceManager, IResourceManager resourceManager)
		{
			foreach (IDevice device in deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable())) {
				PCIDevice pciDevice = device as PCIDevice;
				PCIHardwareDevice pciHardwareDevice = CreateDevice(pciDevice);
				if (pciHardwareDevice != null)
					pciDevice.Start(deviceManager, resourceManager, pciHardwareDevice);
				else
					pciDevice.SetNoDriverFound();
			}
		}
	}
}
