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
	//[DeviceSignature(AutoLoad = false, BasePort = 0x02F8, PortRange = 8, IRQ = 3, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class Serial2Signature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Serial2Signature"/> class.
		/// </summary>
		public Serial2Signature()
		{
			platforms = PlatformArchitecture.x86;
			BasePort = 0x02F8;
			PortRange = 8;
			IRQ = 3;
			AutoLoad = false;
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
