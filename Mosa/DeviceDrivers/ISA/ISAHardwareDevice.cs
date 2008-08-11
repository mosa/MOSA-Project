/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers.ISA
{

    public abstract class ISAHardwareDevice : HardwareDevice
    {
        protected ISABusResources isaBusResources;

        public ISAHardwareDevice() : base() { }

        public void AssignResources(ISABusResources isaBusResources)
        {
            this.isaBusResources = isaBusResources;
        }

    }
}
