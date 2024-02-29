﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem;

/// <summary>
/// Hardware Resources
/// </summary>
public sealed class HardwareResources
{
	/// <summary>
	/// The irq
	/// </summary>
	public byte IRQ { get; }

	/// <summary>
	/// Gets the IO point region count.
	/// </summary>
	/// <value>The IO point region count.</value>
	public byte IOPointRegionCount => (byte)ioPortRegions.Count;

	/// <summary>
	/// Gets the memory region count.
	/// </summary>
	/// <value>The memory region count.</value>
	public byte AddressRegionCount => (byte)addressRegions.Count;

	/// <summary>
	/// The address regions
	/// </summary>
	private readonly List<AddressRegion> addressRegions;

	/// <summary>
	/// The io port regions
	/// </summary>
	private readonly List<IOPortRegion> ioPortRegions;

	/// <summary>
	/// Initializes a new instance of the <see cref="HardwareResources" /> class.
	/// </summary>
	/// <param name="ioPortRegions">The io port regions.</param>
	/// <param name="addressRegions">The memory regions.</param>
	public HardwareResources(List<IOPortRegion> ioPortRegions, List<AddressRegion> addressRegions, byte irq = 0)
	{
		IRQ = irq;

		this.ioPortRegions = ioPortRegions;
		this.addressRegions = addressRegions;
	}

	/// <summary>
	/// Gets the IO port region.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public IOPortRegion GetIOPortRegion(byte index) => ioPortRegions[index];

	/// <summary>
	/// Gets the memory region.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public AddressRegion GetMemoryRegion(byte index) => addressRegions[index];

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortReadWrite GetIOPortReadWrite(byte index, ushort offset) =>
		new IOPortReadWrite((ushort)(ioPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortRead GetIOPortRead(byte index, ushort offset) =>
		new IOPortRead((ushort)(ioPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortWrite GetIOPortWrite(byte index, ushort offset) =>
		new IOPortWrite((ushort)(ioPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the memory.
	/// </summary>
	/// <param name="region">The region.</param>
	/// <returns></returns>
	public ConstrainedPointer GetMemory(byte region) =>
		HAL.GetPhysicalMemory(addressRegions[region].Address, addressRegions[region].Size);
}
