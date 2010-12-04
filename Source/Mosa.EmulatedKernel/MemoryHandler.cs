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
		/// <summary>
		/// 
		/// </summary>
		public uint Address;
		/// <summary>
		/// 
		/// </summary>
		public uint Size;
		/// <summary>
		/// 
		/// </summary>
		public uint Type;

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
		/// <param name="type">The type.</param>
		/// <param name="read8">The read8.</param>
		/// <param name="write8">The write8.</param>
		public MemoryHandler(uint address, uint size, uint type, MemoryDispatch.MemoryRead8 read8, MemoryDispatch.MemoryWrite8 write8)
		{
			Address = address;
			Size = size;
			Type = type;
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
		public bool Contains(ulong address)
		{
			return ((address >= this.Address) && (address < (this.Address + this.Size)));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("Memory: {0:X10}-{1:X10} Length: {2:X10}", Address, Address + Size, Size);
		}
	}

}
