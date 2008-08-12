/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
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
