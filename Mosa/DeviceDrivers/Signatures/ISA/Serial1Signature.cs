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
	//[DeviceSignature(AutoLoad = true, BasePort = 0x03F8, PortRange = 8, IRQ = 4, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class Serial1Signature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Serial1Signature"/> class.
		/// </summary>
		public Serial1Signature()
		{
			platforms = PlatformArchitecture.x86;
			BasePort = 0x03F8;
			PortRange = 8;
			IRQ = 4;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new Serial();
		}

	}
}
