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
 * TestDriver.cs: Test PCI Driver
*/

using Mosa.DeviceDrivers;
using Mosa.ClassLib;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.PCI
{

    [PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x1000)]
    [PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x2000)]
    [PCIDeviceSignature(ClassCode = 0xFFFF, SubClassCode = 0xFF)]
    public class TestDriver : PCIHardwareDevice, IHardwareDevice
    {
        protected IReadWriteIOPort TestPort;
        protected SpinLock spinLock;

        public TestDriver(PCIDevice pciDevice) : base(pciDevice) { }
        public void Dispose() { }

        public override bool Setup()
        {
            base.name = "TEST_0x" + pciBusResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

            TestPort = pciBusResources.GetIOPortRegion(0).GetPort(0);

            return true;
        }

        public override bool Probe() { return true; }
        public override bool Start() { return true; }
        public override bool OnInterrupt() { return false; }
        public override LinkedList<IDevice> CreateSubDevices() { return null; }

    }
}
