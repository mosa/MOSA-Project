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
using Mosa.DeviceDrivers.Signatures;

namespace Mosa.DeviceDrivers.Signatures.ISA
{
	/// <summary>
	/// IDE Controller Primary Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x1F0, PortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class IDEControllerPrimarySignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IDEControllerPrimarySignature"/> class.
		/// </summary>
		public IDEControllerPrimarySignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x1F0;
			PortRange = 8;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new IDEController();
		}

	}
}
