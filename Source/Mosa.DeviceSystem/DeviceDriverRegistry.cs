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
	///
	/// </summary>
	public class DeviceDriverRegistry
	{
		/// <summary>
		///
		/// </summary>
		protected PlatformArchitecture platformArchitecture;

		/// <summary>
		///
		/// </summary>
		protected LinkedList<DeviceDriver> deviceDrivers;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriverRegistry"/> class.
		/// </summary>
		/// <param name="platformArchitecture">The platform architecture.</param>
		public DeviceDriverRegistry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			deviceDrivers = new LinkedList<DeviceDriver>();
		}

		/// <summary>
		/// Finds the driver.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		/// <returns></returns>
		public DeviceDriver FindDriver(IPCIDevice pciDevice)
		{
			DeviceDriver bestDeviceDriver = null;
			int bestPriority = System.Int32.MaxValue;

			foreach (DeviceDriver deviceDriver in deviceDrivers)
			{
				if (deviceDriver.Attribute is PCIDeviceDriverAttribute)
				{
					PCIDeviceDriverAttribute pciDeviceDriverAttribute = deviceDriver.Attribute as PCIDeviceDriverAttribute;
					if ((pciDeviceDriverAttribute.Priority != 0) && (pciDeviceDriverAttribute.Priority < bestPriority))
					{
						if (pciDeviceDriverAttribute.CompareTo(pciDevice))
						{
							bestDeviceDriver = deviceDriver;
							bestPriority = pciDeviceDriverAttribute.Priority;
						}
					}
				}
			}

			return bestDeviceDriver;
		}

		/// <summary>
		/// Gets the ISA device drivers.
		/// </summary>
		/// <returns></returns>
		public LinkedList<DeviceDriver> GetISADeviceDrivers()
		{
			LinkedList<DeviceDriver> isaDeviceDrivers = new LinkedList<DeviceDriver>();

			foreach (DeviceDriver deviceDriver in deviceDrivers)
				if (deviceDriver.Attribute is ISADeviceDriverAttribute)
					isaDeviceDrivers.Add(deviceDriver);

			return isaDeviceDrivers;
		}

		/// <summary>
		/// Registers the build in device drivers.
		/// </summary>
		public void RegisterBuiltInDeviceDrivers()
		{
			//System.Reflection.Assembly assemblyInfo = typeof(DeviceDriverRegistry).Module.Assembly;
			//RegisterDeviceDrivers(assemblyInfo);
		}

		/// <summary>
		/// Registers the device drivers.
		/// </summary>
		/// <param name="assemblyInfo">The assembly info.</param>
		public void RegisterDeviceDrivers(System.Reflection.Assembly assemblyInfo)
		{
			/*System.Type[] types = assemblyInfo.GetTypes();

			foreach (System.Type type in types)
			{
				object[] attributes = type.GetCustomAttributes(typeof(IDeviceDriver), false);

				foreach (object attribute in attributes)
				{
					if (((attribute as IDeviceDriver).Platforms & platformArchitecture) != 0)
					{
						DeviceDriver deviceDriver = new DeviceDriver(attribute as IDeviceDriver, type);

						object[] memAttributes = type.GetCustomAttributes(typeof(DeviceDriverPhysicalMemoryAttribute), false);

						foreach (object memAttribute in memAttributes)
							deviceDriver.Add(memAttribute as DeviceDriverPhysicalMemoryAttribute);

						deviceDrivers.Add(deviceDriver);
					}
				}
			}*/
		}
	}
}