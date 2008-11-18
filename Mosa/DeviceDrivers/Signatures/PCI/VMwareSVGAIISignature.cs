/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.DeviceDrivers.PCI.VideoCard;

namespace Mosa.DeviceDrivers.Signatures.PCI
{
	/// <summary>
	/// VMware SVGA II Device Driver Signature
	/// </summary>
	//[DeviceSignature(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class VMwareSVGAIISignature : PCIDeviceDriverSignature, IPCIDeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VMwareSVGAII"/> class.
		/// </summary>
		public VMwareSVGAIISignature()
		{
			Platforms = PlatformArchitecture.Both_x86_and_x64;
			VendorID = 0x15AD;
			DeviceID = 0x0405;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new VMwareSVGAII();
		}	
	}
}
