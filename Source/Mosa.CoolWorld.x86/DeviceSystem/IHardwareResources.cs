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
	/// Interface to hardware resources
	/// </summary>
	public interface IHardwareResources
	{
		/// <summary>
		/// Gets the IO port region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IIOPortRegion GetIOPortRegion(byte index);

		/// <summary>
		/// Gets the memory region.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IMemoryRegion GetMemoryRegion(byte index);

		/// <summary>
		/// Gets the IO point region count.
		/// </summary>
		/// <value>The IO point region count.</value>
		byte IOPointRegionCount { get; }

		/// <summary>
		/// Gets the memory region count.
		/// </summary>
		/// <value>The memory region count.</value>
		byte MemoryRegionCount { get; }

		/// <summary>
		/// Gets the memory.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <returns></returns>
		IMemory GetMemory(byte region);
		/// <summary>
		/// Gets the IO port.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IReadWriteIOPort GetIOPort(byte region, ushort index);

		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		byte IRQ { get; }

		/// <summary>
		/// Enables the IRQ.
		/// </summary>
		void EnableIRQ();

		/// <summary>
		/// Disables the IRQ.
		/// </summary>
		void DisableIRQ();

		/// <summary>
		/// Gets the PCI device resource.
		/// </summary>
		/// <value>The PCI device resource.</value>
		IDeviceResource DeviceResource { get; }
	}

}
