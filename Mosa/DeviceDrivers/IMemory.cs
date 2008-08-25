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

		byte Read8(uint index);
		void Write8(uint index, byte value);

		// ushort Read16(uint index);
		// void Write16(uint index, ushort value);
		// uint Read32(uint index);
		// void Write32(uint index, uint value);
	}

}
