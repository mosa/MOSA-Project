/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem.PCI
{
    /// <summary>
    /// 
    /// </summary>
	public class Registry
	{
        /// <summary>
        /// 
        /// </summary>
		PlatformArchitecture platformArchitecture;

        /// <summary>
        /// 
        /// </summary>
		protected LinkedList<DriverEntry> drivers;

		/// <summary>
		/// Initializes a new instance of the <see cref="Registry"/> class.
		/// </summary>
		/// <param name="platformArchitecture">The platform architecture.</param>
		public Registry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			drivers = new LinkedList<DriverEntry>();
		}

        /// <summary>
        /// Adds the device driver.
        /// </summary>
        /// <param name="deviceDriverSignature">The device driver signature.</param>
        /// <param name="type">The type.</param>
		public void AddDeviceDriver(DeviceSignatureAttribute deviceDriverSignature, System.Type type)
		{
			drivers.Add(new DriverEntry(deviceDriverSignature, type));
		}

        /// <summary>
        /// Creates the device.
        /// </summary>
        /// <param name="pciDevice">The pci device.</param>
        /// <returns></returns>
		public PCIHardwareDevice CreateDevice(PCIDevice pciDevice)
		{
			System.Type deviceType = FindDriver(pciDevice);

			if (deviceType == null)
				return null;

			return System.Activator.CreateInstance(deviceType, pciDevice) as PCIHardwareDevice;
		}

        /// <summary>
        /// Finds the driver.
        /// </summary>
        /// <param name="pciDevice">The pci device.</param>
        /// <returns></returns>
		public System.Type FindDriver(PCIDevice pciDevice)
		{
			System.Type deviceType = null;
			int bestPriority = System.Int32.MaxValue;

			foreach (DriverEntry entry in drivers) {
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
			System.Reflection.Assembly assemblyInfo = typeof(Registry).Module.Assembly;
			RegisterDeviceDrivers(assemblyInfo);
		}

        /// <summary>
        /// Registers the device drivers.
        /// </summary>
        /// <param name="assemblyInfo">The assembly info.</param>
		public void RegisterDeviceDrivers(System.Reflection.Assembly assemblyInfo)
		{
			System.Type[] types = assemblyInfo.GetTypes();

			foreach (System.Type type in types) {
				object[] attributes = type.GetCustomAttributes(typeof(DeviceSignatureAttribute), false);

				foreach (object attribute in attributes)
					if (((attribute as DeviceSignatureAttribute).Platforms & platformArchitecture) != 0)
						AddDeviceDriver(attribute as DeviceSignatureAttribute, type);
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
