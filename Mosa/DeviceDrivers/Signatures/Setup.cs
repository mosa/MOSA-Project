/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.DeviceDrivers.Signatures.ISA;
using Mosa.DeviceDrivers.Signatures.PCI;

namespace Mosa.DeviceDrivers.Signatures
{
	/// <summary>
	/// Setup for the Device Drivers Attributes.
	/// </summary>
	public static class Setup
	{
		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize(DeviceDriverRegistry registry)
		{
			registry.Add(new CMOSSignature());
			registry.Add(new PICSignature());
			registry.Add(new PITSignature());
			registry.Add(new StandardKeyboardSignature());
			registry.Add(new PCIControllerSignature());
			registry.Add(new VGATextSignature());
			registry.Add(new GenericVGASignature());
			registry.Add(new DMA8BitSignature());
			registry.Add(new IDEControllerPrimarySignature());
			registry.Add(new IDEControllerSecondarySignature());
			registry.Add(new FloppyDiskControllerSecondarySignature());
			registry.Add(new FloppyDiskControllerPrimarySignature());
			registry.Add(new Serial1Signature());
			registry.Add(new Serial2Signature());
			registry.Add(new Serial3Signature());
			registry.Add(new Serial4Signature());
		}
	}
}
