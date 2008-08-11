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
    public interface IPartitionDevice
    {
        uint BlockCount { get; }
        uint BlockSize { get; }
        bool CanWrite { get; }

        byte[] ReadBlock(uint block, uint count);
        bool ReadBlock(uint block, uint count, byte[] data);
        bool WriteBlock(uint block, uint count, byte[] data);
    }
}
