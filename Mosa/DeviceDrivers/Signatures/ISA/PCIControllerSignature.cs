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
using Mosa.DeviceDrivers.ISA;

namespace Mosa.DeviceDrivers.Signatures.ISA
{
	/// <summary>
	/// PCI Controller Driver Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PCIControllerSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCIControllerSignature"/> class.
		/// </summary>
		public PCIControllerSignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x0CF8;
			PortRange = 8;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new PCIController();
		}

	}
}
