/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers
{
	public class BusResources
	{
		protected IIOPortRegion[] ioPortRegions;
		protected IMemoryRegion[] memoryRegions;

		public BusResources(IIOPortRegion[] ioPortRegions, IMemoryRegion[] memoryRegions)
		{
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
		}

		public IIOPortRegion GetIOPortRegion(byte index)
		{
			return ioPortRegions[index];
		}

		public IMemoryRegion GetMemoryRegion(byte index)
		{
			return memoryRegions[index];
		}
	}

}
