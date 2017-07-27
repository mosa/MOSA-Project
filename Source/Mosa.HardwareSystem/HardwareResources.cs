// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem.PCI;
using System.Collections.Generic;

namespace Mosa.HardwareSystem
{
	/// <summary>
	///
	/// </summary>
	public sealed class HardwareResources
	{
		/// <summary>
		/// The io port regions
		/// </summary>
		private List<IOPortRegion> ioPortRegions;

		/// <summary>
		/// The memory regions
		/// </summary>
		private List<MemoryRegion> memoryRegions;

		/// <summary>
		/// The interrupt handler
		/// </summary>
		private InterruptHandler interruptHandler;

		/// <summary>
		/// Gets the PCI device resource.
		/// </summary>
		/// <value>The PCI device resource.</value>
		public IPCIDeviceResource DeviceResource { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources" /> class.
		/// </summary>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		/// <param name="interruptHandler">The interrupt handler.</param>
		public HardwareResources(List<IOPortRegion> ioPortRegions, List<MemoryRegion> memoryRegions, InterruptHandler interruptHandler)
		{
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.interruptHandler = interruptHandler;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources" /> class.
		/// </summary>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		/// <param name="interruptHandler">The interrupt handler.</param>
		/// <param name="deviceResource">The device resource.</param>
		public HardwareResources(List<IOPortRegion> ioPortRegions, List<MemoryRegion> memoryRegions, InterruptHandler interruptHandler, IPCIDeviceResource deviceResource)
		{
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
		public byte IOPointRegionCount { get { return (byte)ioPortRegions.Count; } }

		/// <summary>
		/// Gets the memory region count.
		/// </summary>
		/// <value>The memory region count.</value>
		public byte MemoryRegionCount { get { return (byte)memoryRegions.Count; } }

		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IReadWriteIOPort GetIOPort(byte region, ushort index)
		{
			return HAL.RequestIOPort((ushort)(ioPortRegions[region].BaseIOPort + index));
		}

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <returns></returns>
		public IMemory GetMemory(byte region)
		{
			return HAL.RequestPhysicalMemory(memoryRegions[region].BaseAddress, memoryRegions[region].Size);
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
