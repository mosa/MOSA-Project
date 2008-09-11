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
	public interface IReadWriteIOPort : IReadOnlyIOPort, IWriteOnlyIOPort
	{
		new ushort Address { get; }

		new byte Read8();
        new void Write8(byte data);
        new ushort Read16();
        new void Write16(ushort data);
        new uint Read32();
        new void Write32(uint data);
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
