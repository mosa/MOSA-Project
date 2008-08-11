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
 * PCIBusResources.cs: Represents IO and memory resources on the PCI bus
*/

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers.PCI
{

    public class PCIBusResources
    {
        protected IOPortRegion[] ioPortRegions;
        protected MemoryRegion[] memoryRegions;

        public PCIBusResources(IOPortRegion[] ioPortRegions, MemoryRegion[] memoryRegions)
        {
            this.ioPortRegions = ioPortRegions;
            this.memoryRegions = memoryRegions;
        }

        public IOPortRegion GetIOPortRegion(byte index)
        {
            return ioPortRegions[index];
        }

        public MemoryRegion GetMemoryRegion(byte index)
        {
            return memoryRegions[index];
        }
    }

}
