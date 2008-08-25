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
		ulong Address { get; }
		ulong Size { get; }

		byte this[ulong index] { get; set; }

		byte Read8(ulong index);
		void Write8(ulong index, byte value);

		// ushort Read16(ulong index);
		// void Write16(ulong index, ushort value);
		// uint Read32(ulong index);
		// void Write32(ulong index, uint value);
	}

}
