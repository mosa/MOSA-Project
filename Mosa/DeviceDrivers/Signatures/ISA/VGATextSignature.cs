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
	/// VGA Text Driver Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class VGATextSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VGATextSignature"/> class.
		/// </summary>
		public VGATextSignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x03B0;
			PortRange = 0x1F;
			BaseAddress = 0xB0000;
			AddressRange = 0x10000;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new VGAText();
		}

	}
}
