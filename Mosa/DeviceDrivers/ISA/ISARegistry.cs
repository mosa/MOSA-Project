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

namespace Mosa.DeviceDrivers.ISA
{
	public class ISARegistry
	{
		protected PlatformArchitecture platformArchitecture;

		protected LinkedList<Pair<ISADeviceSignatureAttribute, Type>> drivers;

		public ISARegistry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			drivers = new LinkedList<Pair<ISADeviceSignatureAttribute, Type>>();
		}

		public void AddDeviceDriver(ISADeviceSignatureAttribute deviceDriverSignature, Type type)
		{
			drivers.Add(new Pair<ISADeviceSignatureAttribute, Type>(deviceDriverSignature, type));
		}

		public void RegisterBuildInDeviceDrivers()
		{
			Assembly assemblyInfo = typeof(ISARegistry).Module.Assembly;
			RegisterDeviceDrivers(assemblyInfo);
		}

		public void RegisterDeviceDrivers(Assembly assemblyInfo)
		{
			Type[] types = assemblyInfo.GetTypes();

			foreach (Type type in types) {
				object[] attributes = type.GetCustomAttributes(typeof(ISADeviceSignatureAttribute), false);

				foreach (object attribute in attributes)
					if (((attribute as ISADeviceSignatureAttribute).Platforms & platformArchitecture) != 0)
						AddDeviceDriver(attribute as ISADeviceSignatureAttribute, type);
			}
		}

		public void StartDrivers(IDeviceManager deviceManager, IResourceManager resourceManager)
		{
			foreach (Pair<ISADeviceSignatureAttribute, Type> entry in drivers) {
				if (entry.First.AutoLoad) {
					IIOPortRegion[] ioPortRegions;
					IMemoryRegion[] memoryRegion;

					if (entry.First.BasePort != 0x0) {
						ioPortRegions = new IOPortRegion[1];
						ioPortRegions[0] = new IOPortRegion(entry.First.BasePort, entry.First.PortRange);
					}
					else {
						ioPortRegions = new IOPortRegion[0];
					}

					if (entry.First.BaseAddress != 0x0) {
						memoryRegion = new MemoryRegion[1];
						memoryRegion[0] = new MemoryRegion(entry.First.BaseAddress, entry.First.AddressRange);
					}
					else {
						memoryRegion = new MemoryRegion[0];
					}

					ISAHardwareDevice isaHardwareDevice = Activator.CreateInstance(entry.Second) as ISAHardwareDevice;
					IBusResources busResources = new BusResources(resourceManager, ioPortRegions, memoryRegion, new InterruptHandler(resourceManager.InterruptManager, entry.First.IRQ, isaHardwareDevice));

					isaHardwareDevice.AssignBusResources(busResources);

					deviceManager.Add(isaHardwareDevice);

					isaHardwareDevice.Activate(deviceManager);
				}
			}
		}
	}
}
