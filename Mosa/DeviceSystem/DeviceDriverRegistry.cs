/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem.PCI;

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
		protected LinkedList<IDeviceDriverSignature> deviceDrivers;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriverRegistry"/> class.
		/// </summary>
		/// <param name="platformArchitecture">The platform architecture.</param>
		public DeviceDriverRegistry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			deviceDrivers = new LinkedList<IDeviceDriverSignature>();
		}

		/// <summary>
		/// Adds the specified device driver attribute.
		/// </summary>
		/// <param name="deviceDriverAttribute">The device driver attribute.</param>
		public void Add(IDeviceDriverSignature deviceDriverAttribute)
		{
			deviceDrivers.Add(deviceDriverAttribute);
		}

		/// <summary>
		/// Finds the driver.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		/// <returns></returns>
		public IPCIDeviceDriverSignature FindDriver(PCIDevice pciDevice)
		{
			IPCIDeviceDriverSignature bestDeviceDriverAttribute = null;
			int bestPriority = System.Int32.MaxValue;

			foreach (IDeviceDriverSignature deviceDriverAttribute in deviceDrivers)
			{
				if (deviceDriverAttribute is IPCIDeviceDriverSignature)
				{
					IPCIDeviceDriverSignature pciDeviceDriverAttribute = deviceDriverAttribute as IPCIDeviceDriverSignature;
					if ((pciDeviceDriverAttribute.Priority != 0) && (pciDeviceDriverAttribute.Priority < bestPriority))
					{
						if (pciDeviceDriverAttribute.CompareTo(pciDevice))
						{
							bestDeviceDriverAttribute = pciDeviceDriverAttribute;
							bestPriority = pciDeviceDriverAttribute.Priority;
						}
					}
				}
			}

			return bestDeviceDriverAttribute;
		}

		/// <summary>
		/// Gets the ISA device drivers.
		/// </summary>
		/// <returns></returns>
		public LinkedList<IISADeviceDriverSignature> GetISADeviceDrivers()
		{
			LinkedList<IISADeviceDriverSignature> isaDeviceDrivers = new LinkedList<IISADeviceDriverSignature>();

			foreach (IDeviceDriverSignature deviceDriverAttribute in deviceDrivers)
				if (deviceDriverAttribute is IISADeviceDriverSignature)
					isaDeviceDrivers.Add(deviceDriverAttribute as IISADeviceDriverSignature);

			return isaDeviceDrivers;
		}
	}

}


//public void StartDrivers(IDeviceManager deviceManager, IResourceManager resourceManager)
//{
//    foreach (IDevice device in deviceManager.GetDevices(new FindDevice.IsPCIDevice(), new FindDevice.IsAvailable())) {
//        PCIDevice pciDevice = device as PCIDevice;
//        PCIHardwareDevice pciHardwareDevice = CreateDevice(pciDevice);
//        if (pciHardwareDevice != null)
//            pciDevice.Start(deviceManager, resourceManager, pciHardwareDevice);
//        else
//            pciDevice.SetNoDriverFound();
//    }
//}