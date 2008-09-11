/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.ClassLib;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
    /// 
    /// </summary>
    [ISADeviceSignature(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.x86)]
    public class CMOSDriver : ISAHardwareDevice, IDevice, IHardwareDevice
    {
        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort commandPort;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort dataPort;

        /// <summary>
        /// 
        /// </summary>
        protected SpinLock spinLock;

        /// <summary>
        /// 
        /// </summary>
        public CMOSDriver() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Setup()
        {
            base.name = "CMOS";

            commandPort = base.busResources.GetIOPort(0, 0);
            dataPort = base.busResources.GetIOPort(0, 4);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Probe()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override LinkedList<IDevice> CreateSubDevices()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool OnInterrupt()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte Read(byte address)
        {
            spinLock.Enter();
            commandPort.Write8(address);
            byte b = dataPort.Read8();
            spinLock.Exit();
            return b;
        }
    }
}
