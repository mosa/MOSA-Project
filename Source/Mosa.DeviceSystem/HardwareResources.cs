// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem;

/// <summary>
/// Hardware Resources
/// </summary>
public sealed class HardwareResources
{
	/// <summary>
	/// The address regions
	/// </summary>
	public List<AddressRegion> AddressRegions { get; }

	/// <summary>
	/// The io port regions
	/// </summary>
	public List<IOPortRegion> IOPortRegions { get; }

	/// <summary>
	/// The irq
	/// </summary>
	public byte IRQ { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="HardwareResources" /> class.
	/// </summary>
	/// <param name="ioPortRegions">The io port regions.</param>
	/// <param name="addressRegions">The memory regions.</param>
	public HardwareResources(List<IOPortRegion> ioPortRegions, List<AddressRegion> addressRegions, byte irq = 0)
	{
		IOPortRegions = ioPortRegions;
		AddressRegions = addressRegions;
		IRQ = irq;
	}

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortReadWrite GetIOPortReadWrite(byte index, ushort offset) =>
		new IOPortReadWrite((ushort)(IOPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortRead GetIOPortRead(byte index, ushort offset) =>
		new IOPortRead((ushort)(IOPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the IO port.
	/// </summary>
	/// <param name="index">The region.</param>
	/// <param name="offset">The index.</param>
	/// <returns></returns>
	public IOPortWrite GetIOPortWrite(byte index, ushort offset) =>
		new IOPortWrite((ushort)(IOPortRegions[index].BaseIOPort + offset));

	/// <summary>
	/// Gets the memory.
	/// </summary>
	/// <param name="region">The region.</param>
	/// <returns></returns>
	public ConstrainedPointer GetMemory(byte region) =>
		HAL.GetPhysicalMemory(AddressRegions[region].Address, AddressRegions[region].Size);
}
