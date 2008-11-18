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
	/// StandardKeyboard Signature
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class StandardKeyboardSignature : ISADeviceDriverSignature, IISADeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKeyboardSignature"/> class.
		/// </summary>
		public StandardKeyboardSignature()
		{
			platforms = PlatformArchitecture.Both_x86_and_x64;
			BasePort = 0x60;
			PortRange = 1;
			AltBasePort = 0x64;
			AltPortRange = 1;
			IRQ = 1;
			AutoLoad = true;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{
			return new PIC();
		}

	}
}
