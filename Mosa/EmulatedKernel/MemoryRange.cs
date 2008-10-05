/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.EmulatedKernel;

namespace Mosa.EmulatedKernel
{
    /// <summary>
    /// 
    /// </summary>
	public class MemoryRange
	{
        /// <summary>
        /// 
        /// </summary>
		public uint address;

        /// <summary>
        /// 
        /// </summary>
		public uint size;

        /// <summary>
        /// 
        /// </summary>
		public MemoryRead8 read8;

        /// <summary>
        /// 
        /// </summary>
		public MemoryWrite8 write8;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="read8"></param>
        /// <param name="write8"></param>
		public MemoryRange(uint address, uint size, MemoryRead8 read8, MemoryWrite8 write8)
		{
			this.address = address;
			this.size = size;
			this.read8 = read8;
			this.write8 = write8;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
		public bool Contains(uint address)
		{
			return ((address >= this.address) && (address < (this.address + this.size)));
		}
	}

}
