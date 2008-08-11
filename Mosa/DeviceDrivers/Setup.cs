/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers
{
    public static class Setup
    {
        static private DeviceManager deviceManager;

        static public DeviceManager DeviceManager { get { return deviceManager; } }

        static public void Initialize()
        {
            deviceManager = new DeviceManager();
            PortIOSpace portIOSpace = new PortIOSpace();
            MemorySpace memorySpace = new MemorySpace();

            ISA.ISADeviceDrivers isaDeviceDrivers = new Mosa.DeviceDrivers.ISA.ISADeviceDrivers();

            isaDeviceDrivers.RegisterBuildInDeviceDrivers();

            isaDeviceDrivers.StartDrivers(deviceManager, portIOSpace, memorySpace);

            PCI.PCIDeviceDrivers pciDeviceDrivers = new Mosa.DeviceDrivers.PCI.PCIDeviceDrivers();

            pciDeviceDrivers.StartDrivers(deviceManager, portIOSpace, memorySpace);
        }

    }
}
