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
	public class MemoryHandler
	{
		private uint address;
		private uint size;

		/// <summary>
		/// 
		/// </summary>
		public MemoryDispatch.MemoryRead8 read8;
		/// <summary>
		/// 
		/// </summary>
		public MemoryDispatch.MemoryWrite8 write8;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryHandler"/> class.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="read8">The read8.</param>
		/// <param name="write8">The write8.</param>
		public MemoryHandler(uint address, uint size, MemoryDispatch.MemoryRead8 read8, MemoryDispatch.MemoryWrite8 write8)
		{
			this.address = address;
			this.size = size;
			this.read8 = read8;
			this.write8 = write8;
		}

		/// <summary>
		/// Determines whether [contains] [the specified address].
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified address]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(uint address)
		{
			return ((address >= this.address) && (address < (this.address + this.size)));
		}
	}

}
