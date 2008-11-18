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
	/// CMOS Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x70, PortRange = 2, Platforms = PlatformArchitecture.x86)]
	public class CMOSSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CMOSSignature"/> class.
		/// </summary>
		public CMOSSignature()
		{
			platforms = PlatformArchitecture.x86;
			BasePort = 0x70;
			PortRange = 2;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new CMOS();
		}

	}
}
