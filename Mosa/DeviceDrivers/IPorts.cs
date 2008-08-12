/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers
{
	public interface IReadWriteIOPort : IReadOnlyIOPort, IWriteOnlyIOPort
	{
		ushort Address { get; }

		byte Read8();
		void Write8(byte data);
		ushort Read16();
		void Write16(ushort data);
		uint Read32();
		void Write32(uint data);
	}

	public interface IReadOnlyIOPort
	{
		ushort Address { get; }
		byte Read8();
		ushort Read16();
		uint Read32();
	}

	public interface IWriteOnlyIOPort
	{
		ushort Address { get; }
		void Write8(byte data);
		void Write16(ushort data);
		void Write32(uint data);
	}


}
