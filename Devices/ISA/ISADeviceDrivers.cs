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

namespace Mosa.Devices.ISA
{
    public class ISADeviceDrivers
    {
        protected LinkedList<Pair<ISADeviceSignatureAttribute, Type>> drivers;

        public ISADeviceDrivers()
        {
            drivers = new LinkedList<Pair<ISADeviceSignatureAttribute, Type>>();
        }

        public void AddDeviceDriver(ISADeviceSignatureAttribute deviceDriverSignature, Type type)
        {
            drivers.Add(new Pair<ISADeviceSignatureAttribute, Type>(deviceDriverSignature, type));
        }

        public void RegisterBuildInDeviceDrivers()
        {
            Assembly assemblyInfo = typeof(ISADeviceDrivers).Module.Assembly;
            RegisterDeviceDrivers(assemblyInfo);
        }

        public void RegisterDeviceDrivers(Assembly assemblyInfo)
        {
            Type[] types = assemblyInfo.GetTypes();

            foreach (Type type in types) {
                object[] attributes = type.GetCustomAttributes(typeof(ISADeviceSignatureAttribute), false);

                foreach (object attribute in attributes)
                    AddDeviceDriver((ISADeviceSignatureAttribute)attribute, type);
            }
        }

        public void StartDrivers(DeviceManager deviceManager, PortIOSpace portIOSpace, MemorySpace memorySpace)
        {
            foreach (Pair<ISADeviceSignatureAttribute, Type> entry in drivers) {
                if (entry.First.AutoLoad) {
                    IOPortRegion[] ioPortRegions = new IOPortRegion[1];
                    MemoryRegion[] memoryRegion = new MemoryRegion[1];

                    if (entry.First.BasePort != 0x0)
                        ioPortRegions[0] = new IOPortRegion(portIOSpace, entry.First.BasePort, entry.First.PortRange);

                    if (entry.First.BaseAddress != 0x0)
                        memoryRegion[0] = new MemoryRegion(memorySpace, entry.First.BaseAddress, entry.First.AddressRange);

                    ISABusResources isaBusResources = new ISABusResources(ioPortRegions, memoryRegion);
                    ISAHardwareDevice isaHardwareDevice = (ISAHardwareDevice)Activator.CreateInstance(entry.Second);

                    isaHardwareDevice.AssignResources(isaBusResources);

                    deviceManager.Add(isaHardwareDevice);

                    isaHardwareDevice.Activate(deviceManager);
                }
            }
        }
    }
}
