/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers.ISA
{

    public class ISABusResources
    {
        protected IOPortRegion[] ioPortRegions;
        protected MemoryRegion[] memoryRegion;

        public ISABusResources(IOPortRegion[] ioPortRegions, MemoryRegion[] memoryRegion)
        {
            this.ioPortRegions = ioPortRegions;
            this.memoryRegion = memoryRegion;
        }

        public IOPortRegion GetIOPortRegion(byte index)
        {
            return ioPortRegions[index];
        }

        public MemoryRegion GetMemoryRegion(byte index)
        {
            return memoryRegion[index];
        }

    }

}
