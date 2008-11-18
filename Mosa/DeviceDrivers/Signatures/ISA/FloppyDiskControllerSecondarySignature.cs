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
	/// Floppy Disk Controller Secondary Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = false, BasePort = 0x0370, PortRange = 8, IRQ = 5, ForceOption = "fdc2", Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class FloppyDiskControllerSecondarySignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FloppyDiskControllerSecondarySignature"/> class.
		/// </summary>
		public FloppyDiskControllerSecondarySignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x0370;
			PortRange = 8;
			IRQ = 5;
			ForceOption = "fdc2";
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
