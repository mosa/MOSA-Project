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
	/// PIT Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PITSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PITSignature"/> class.
		/// </summary>
		public PITSignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x40;
			PortRange = 4;
			IRQ = 0;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new PIT();
		}

	}
}
