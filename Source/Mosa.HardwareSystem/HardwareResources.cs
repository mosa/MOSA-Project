// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem.PCI;

namespace Mosa.HardwareSystem
{
	/// <summary>
	///
	/// </summary>
	public sealed class HardwareResources
	{
		/// <summary>
		///
		/// </summary>
		private ResourceManager resourceManager;

		/// <summary>
		///
		/// </summary>
		private IOPortRegion[] ioPortRegions;

		/// <summary>
		///
		/// </summary>
		private MemoryRegion[] memoryRegions;

		/// <summary>
		///
		/// </summary>
		private InterruptHandler interruptHandler;

		/// <summary>
		/// Gets the PCI device resource.
		/// </summary>
		/// <value>The PCI device resource.</value>
		public IPCIDeviceResource DeviceResource { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources"/> class.
		/// </summary>
		/// <param name="resourceManager">The resource manager.</param>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		/// <param name="interruptHandler">The interrupt handler.</param>
		public HardwareResources(ResourceManager resourceManager, IOPortRegion[] ioPortRegions, MemoryRegion[] memoryRegions, InterruptHandler interruptHandler)
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
		public HardwareResources(ResourceManager resourceManager, IOPortRegion[] ioPortRegions, MemoryRegion[] memoryRegions, InterruptHandler interruptHandler, IPCIDeviceResource deviceResource)
		{
			this.resourceManager = resourceManager;
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.interruptHandler = interruptHandler;
			this.DeviceResource = DeviceResource;
		}

		/// <summary>
		/// Gets the IO port region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IOPortRegion GetIOPortRegion(byte index)
		{
			return ioPortRegions[index];
		}

		/// <summary>
		/// Gets the memory region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public MemoryRegion GetMemoryRegion(byte index)
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
			return resourceManager.IOPortResources.GetIOPort(ioPortRegions[region].BaseIOPort, index);
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
					return 0xFF;    // 0xFF means unused
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
	}
}
