/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMemoryRegion
	{
		/// <summary>
		/// Gets the base address.
		/// </summary>
		/// <value>The base address.</value>
		uint BaseAddress { get; }
		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		uint Size { get; }

		/// <summary>
		/// Determines whether [contains] [the specified address].
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		bool Contains(uint address);
	}

}
