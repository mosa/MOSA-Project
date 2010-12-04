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
	public interface IIOPortRegion
	{
		/// <summary>
		/// Gets the base IO port.
		/// </summary>
		/// <value>The base IO port.</value>
		ushort BaseIOPort { get; }
		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		ushort Size { get; }
	}

}
