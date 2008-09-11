/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers.PCI
{
    /// <summary>
    /// 
    /// </summary>
	public abstract class PCIHardwareDevice : HardwareDevice
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pciDevice"></param>
		public PCIHardwareDevice(PCIDevice pciDevice) : base() { base.parent = pciDevice as IDevice; }
	}

}
