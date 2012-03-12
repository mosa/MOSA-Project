/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class HardwareResources : IHardwareResources
	{
		/// <summary>
		/// 
		/// </summary>
		protected IResourceManager resourceManager;
		/// <summary>
		/// 
		/// </summary>
		protected IIOPortRegion[] ioPortRegions;
		/// <summary>
		/// 
		/// </summary>
		protected IMemoryRegion[] memoryRegions;
		/// <summary>
		/// 
		/// </summary>
		protected IInterruptHandler interruptHandler;

		/// <summary>
		/// 
		/// </summary>
		protected IDeviceResource deviceResource;

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources"/> class.
		/// </summary>
		/// <param name="resourceManager">The resource manager.</param>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		/// <param name="interruptHandler">The interrupt handler.</param>
		public HardwareResources(IResourceManager resourceManager, IIOPortRegion[] ioPortRegions, IMemoryRegion[] memoryRegions, IInterruptHandler interruptHandler)
		{
			this.resourceManager = resourceManager;
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.interruptHandler = interruptHandler;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources"/> class.
		/// </summary>
		/// <param name="resourceManager">The resource manager.</param>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		/// <param name="interruptHandler">The interrupt handler.</param>
		/// <param name="deviceResource">The device resource.</param>
		public HardwareResources(IResourceManager resourceManager, IIOPortRegion[] ioPortRegions, IMemoryRegion[] memoryRegions, IInterruptHandler interruptHandler, IDeviceResource deviceResource)
		{
			this.resourceManager = resourceManager;
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.interruptHandler = interruptHandler;
			this.deviceResource = DeviceResource;
		}

		/// <summary>
		/// Gets the IO port region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IIOPortRegion GetIOPortRegion(byte index)
		{
			return ioPortRegions[index];
		}

		/// <summary>
		/// Gets the memory region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IMemoryRegion GetMemoryRegion(byte index)
		{
			return memoryRegions[index];
		}

		/// <summary>
		/// Gets the IO point region count.
		/// </summary>
		/// <value>The IO point region count.</value>
		public byte IOPointRegionCount { get { return (byte)ioPortRegions.Length; } }

		/// <summary>
		/// Gets the memory region count.
		/// </summary>
		/// <value>The memory region count.</value>
		public byte MemoryRegionCount { get { return (byte)memoryRegions.Length; } }

		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IReadWriteIOPort GetIOPort(byte region, ushort index)
		{
			ushort basePort = ioPortRegions[region].BaseIOPort;
			IReadWriteIOPort port = resourceManager.IOPortResources.GetIOPort(basePort, index);
			return port;
		}

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <returns></returns>
		public IMemory GetMemory(byte region)
		{
			return resourceManager.MemoryResources.GetMemory(memoryRegions[region].BaseAddress, memoryRegions[region].Size);
		}

		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ
		{
			get
			{
				if (interruptHandler == null)
					return 0xFF;	// 0xFF means unused
				else
					return interruptHandler.IRQ;
			}
		}

		/// <summary>
		/// Enables the IRQ.
		/// </summary>
		public void EnableIRQ()
		{
			interruptHandler.Enable();
		}

		/// <summary>
		/// Disables the IRQ.
		/// </summary>
		public void DisableIRQ()
		{
			interruptHandler.Disable();
		}

		/// <summary>
		/// Gets the PCI device resource.
		/// </summary>
		/// <value>The PCI device resource.</value>
		public IDeviceResource DeviceResource { get { return deviceResource; } }

	}

}