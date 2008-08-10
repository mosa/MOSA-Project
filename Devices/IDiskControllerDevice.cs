/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Devices
{
    public interface IDiskControllerDevice
    {
        bool Open(uint drive);
        bool Release(uint drive);

        bool ReadBlock(uint drive, uint block, uint count, byte[] data);
        bool WriteBlock(uint drive, uint block, uint count, byte[] data);

        uint GetSectorSize(uint drive);
        uint GetTotalSectors(uint drive);
        bool CanWrite(uint drive);
    }
}
