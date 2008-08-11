/*
 * (c) 2008 The Ensemble OS Project
 * http://www.ensemble-os.org
 * All Rights Reserved
 *
 * This code is covered by the New BSD License, found in license.txt
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 * IPCIController.cs: Represents a PCI controller device
*/

namespace Mosa.DeviceDrivers.PCI
{
    public interface IPCIController
    {
        uint ReadConfig(uint bus, uint slot, uint function, uint register);
        void WriteConfig(uint bus, uint slot, uint function, uint register, uint value);
    }
}
