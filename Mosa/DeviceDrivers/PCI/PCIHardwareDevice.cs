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
 * PCIHardwareDevice.cs: Base class for a PCI device driver
*/

namespace Mosa.DeviceDrivers.PCI
{

    public abstract class PCIHardwareDevice : HardwareDevice
    {
        protected PCIBusResources pciBusResources;

        private PCIHardwareDevice() : base() { }
        public PCIHardwareDevice(PCIDevice pciDevice) : base() { base.parent = (IDevice)pciDevice; }

        public void AssignResources(PCIBusResources pciBusResources)
        {
            this.pciBusResources = pciBusResources;
        }

    }
}
