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
	public interface IMemory
	{
		uint Address { get; }
		uint Size { get; }

		byte this[uint index] { get; set; }
		byte this[long index] { get; set; }
	}

}
