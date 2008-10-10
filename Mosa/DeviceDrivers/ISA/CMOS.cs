/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.ISA;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
    /// CMOS Device Driver
    /// </summary>
    [ISADeviceSignature(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.x86)]
    public class CMOS : ISAHardwareDevice, IDevice, IHardwareDevice
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
        public CMOS() { }

		/// <summary>
		/// Setups this hardware device driver
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
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
        public override bool Start()
        {
            return true;
        }

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
        public override bool Probe()
        {
            return true;
        }

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
        public override LinkedList<IDevice> CreateSubDevices()
        {
            return null;
        }

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
        public override bool OnInterrupt()
        {
            return true;
        }

		/// <summary>
		/// Reads the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
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
