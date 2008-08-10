/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Devices.Kernel;

namespace Mosa.Devices.Kernel
{
    public interface IReadWriteIOPort : IReadOnlyIOPort, IWriteOnlyIOPort
    {
        ushort Address { get; }

        byte Read8Bits();
        void Write8Bits(byte data);
        ushort Read16Bits();
        void Write16Bits(ushort data);
        uint Read32Bits();
        void Write32Bits(uint data);
    }

    public interface IReadOnlyIOPort
    {
        ushort Address { get; }
        byte Read8Bits();
        ushort Read16Bits();
        uint Read32Bits();
    }

    public interface IWriteOnlyIOPort
    {
        ushort Address { get; }
        void Write8Bits(byte data);
        void Write16Bits(ushort data);
        void Write32Bits(uint data);
    }


}
