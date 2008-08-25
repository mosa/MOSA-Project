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
	public class BusResources : IBusResources
	{
		protected IResourceManager resourceManager;

		protected IIOPortRegion[] ioPortRegions;
		protected IMemoryRegion[] memoryRegions;
		protected IInterruptHandler interruptHandler;

		public BusResources(IResourceManager resourceManager, IIOPortRegion[] ioPortRegions, IMemoryRegion[] memoryRegions, IInterruptHandler interruptHandler)
		{
			this.resourceManager = resourceManager;
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.interruptHandler = interruptHandler;
		}

		public IIOPortRegion GetIOPortRegion(byte index)
		{
			return ioPortRegions[index];
		}

		public IMemoryRegion GetMemoryRegion(byte index)
		{
			return memoryRegions[index];
		}

		public IReadWriteIOPort GetIOPort(byte region, ushort index)
		{
			return resourceManager.IOPortResources.GetIOPort(ioPortRegions[region].BaseIOPort, index);
		}

		public IMemory GetMemory(byte region)
		{
			return resourceManager.MemoryResources.GetMemory(memoryRegions[region].BaseAddress, memoryRegions[region].Size);
		}

		public byte IRQ
		{
			get
			{
				if (interruptHandler == null)
					return 0;

				return interruptHandler.IRQ;
			}
		}

		public void EnableIRQ()
		{
			interruptHandler.Enable();
		}

		public void DisableIRQ()
		{
			interruptHandler.Enable();
		}

	}

}
