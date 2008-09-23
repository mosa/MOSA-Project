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
    /// <summary>
    /// 
    /// </summary>
	public class MemoryResources
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryResources"/> class.
        /// </summary>
		public MemoryResources() { }

        /// <summary>
        /// Gets the memory.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
		public IMemory GetMemory(uint address, uint size)
		{
			return HAL.RequestMemory(address, size);
		}
	}
}
