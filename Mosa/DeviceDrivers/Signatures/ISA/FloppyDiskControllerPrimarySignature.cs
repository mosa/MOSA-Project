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
	/// Floppy Controller Primary Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x03F0, PortRange = 8, IRQ = 6, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class FloppyDiskControllerPrimarySignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FloppyDiskControllerPrimarySignature"/> class.
		/// </summary>
		public FloppyDiskControllerPrimarySignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x03F0;
			PortRange = 8;
			IRQ = 6;
			AutoLoad = false;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new FloppyDiskController();
		}

	}
}
