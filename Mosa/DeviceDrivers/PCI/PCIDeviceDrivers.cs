/*
 * (c) 2008 The Ensemble OS Project
 * http://www.ensemble-os.org
 * All Rights Reserved
 *
 * This code is covered by the New BSD License, found in license.txt
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 * PCIDeviceDrivers.cs: Container for all the PCI device drivers
*/

using System;
using System.Reflection;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.PCI
{

    public class PCIDeviceDrivers
    {
        protected LinkedList<Pair<PCIDeviceSignatureAttribute, Type>> drivers;

        public PCIDeviceDrivers()
        {
            drivers = new LinkedList<Pair<PCIDeviceSignatureAttribute, Type>>();
        }

        public void AddDeviceDriver(PCIDeviceSignatureAttribute deviceDriverSignature, Type type)
        {
            drivers.Add(new Pair<PCIDeviceSignatureAttribute, Type>(deviceDriverSignature, type));
        }

        public PCIHardwareDevice CreateDevice(PCIDevice pciDevice)
        {
            Type deviceType = FindDriver(pciDevice);

            if (deviceType == null)
                return null;

            return (PCIHardwareDevice)Activator.CreateInstance(deviceType, pciDevice);
        }

        public Type FindDriver(PCIDevice pciDevice)
        {
            Type deviceType = null;
            int bestPriority = Int32.MaxValue;

            foreach (Pair<PCIDeviceSignatureAttribute, Type> entry in drivers) {
                if ((entry.First.Priority != 0) && (entry.First.Priority < bestPriority)) {
                    if (entry.First.CompareTo(pciDevice)) {
                        deviceType = entry.Second;
                        bestPriority = entry.First.Priority;
                    }
                }
            }

            return deviceType;
        }

        public void RegisterBuildInDeviceDrivers()
        {
            Assembly assemblyInfo = typeof(PCIDeviceDrivers).Module.Assembly;
            RegisterDeviceDrivers(assemblyInfo);
        }

        public void RegisterDeviceDrivers(Assembly assemblyInfo)
        {
            Type[] types = assemblyInfo.GetTypes();

            foreach (Type type in types) {
                object[] attributes = type.GetCustomAttributes(typeof(PCIDeviceSignatureAttribute), false);

                foreach (object attribute in attributes)
                    AddDeviceDriver((PCIDeviceSignatureAttribute)attribute, type);
            }
        }

        public class IsPCIDevice : IFindDevice
        {
            public IsPCIDevice() { }

            public bool IsMatch(IDevice device)
            {
                return device is PCIDevice;
            }
        }

        public void StartDrivers(DeviceManager deviceManager, PortIOSpace portIOSpace, MemorySpace memorySpace)
        {
            foreach (IDevice device in deviceManager.GetDevices(new IsPCIDevice(), new DeviceManager.Available())) {
                PCIDevice pciDevice = (PCIDevice)device;
                PCIHardwareDevice pciHardwareDevice = CreateDevice(pciDevice);
                if (pciHardwareDevice != null)
                    pciDevice.Start(deviceManager, pciHardwareDevice);
                else
                    pciDevice.SetNoDeviceFound();
            }

        }
    }
}
