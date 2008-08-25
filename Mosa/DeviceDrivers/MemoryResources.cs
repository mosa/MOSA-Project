/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	public class MemoryResources
	{
		public MemoryResources() { }

		public IMemory GetMemory(uint address, uint size)
		{
			return Kernel.HAL.RequestMemory(address, size);
		}
	}
}
