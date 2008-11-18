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
	/// PIC Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = false, BasePort = 0x00, PortRange = 32, AltBasePort = 0x80, AltPortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class DMA8BitSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DMA8BitSignature"/> class.
		/// </summary>
		public DMA8BitSignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x00;
			PortRange = 32;
			AltBasePort = 0x80;
			AltPortRange = 8;
			AutoLoad = false;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new DMA8Bit();
		}

	}
}
