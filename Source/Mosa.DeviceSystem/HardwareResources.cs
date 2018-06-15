// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Hardware Resources
	/// </summary>
	public sealed class HardwareResources
	{
		/// <summary>
		/// The io port regions
		/// </summary>
		private readonly List<IOPortRegion> ioPortRegions;

		/// <summary>
		/// The memory regions
		/// </summary>
		private readonly List<MemoryRegion> memoryRegions;

		/// <summary>
		/// The irq
		/// </summary>
		public byte IRQ { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareResources" /> class.
		/// </summary>
		/// <param name="ioPortRegions">The io port regions.</param>
		/// <param name="memoryRegions">The memory regions.</param>
		public HardwareResources(List<IOPortRegion> ioPortRegions, List<MemoryRegion> memoryRegions, byte irq = 0)
		{
			this.ioPortRegions = ioPortRegions;
			this.memoryRegions = memoryRegions;
			this.IRQ = irq;
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
		public IOPortReadWrite GetIOPortReadWrite(byte region, ushort index)
		{
			return HAL.RequestReadWriteIOPort((ushort)(ioPortRegions[region].BaseIOPort + index));
		}

		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IOPortRead GetIOPortRead(byte region, ushort index)
		{
			return HAL.RequestReadIOPort((ushort)(ioPortRegions[region].BaseIOPort + index));
		}

		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public IOPortWrite GetIOPortWrite(byte region, ushort index)
		{
			return HAL.RequestWriteIOPort((ushort)(ioPortRegions[region].BaseIOPort + index));
		}

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <returns></returns>
		public Memory GetMemory(byte region)
		{
			return HAL.RequestPhysicalMemory(memoryRegions[region].BaseAddress, memoryRegions[region].Size);
		}
	}
}
