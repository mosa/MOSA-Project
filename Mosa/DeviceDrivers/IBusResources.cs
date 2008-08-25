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
	public interface IBusResources
	{
		IIOPortRegion GetIOPortRegion(byte index);
		IMemoryRegion GetMemoryRegion(byte index);

		IMemory GetMemory(byte region);
		IReadWriteIOPort GetIOPort(byte region, ushort index);
		
		byte IRQ { get; }
		void EnableIRQ();
		void DisableIRQ();
	}

}
