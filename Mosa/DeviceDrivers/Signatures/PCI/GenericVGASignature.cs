/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceDrivers.PCI.VideoCard;

namespace Mosa.DeviceDrivers.Signatures.PCI
{
	/// <summary>
	/// Generic VGA Device Driver Signature
	/// </summary>
	//[DeviceSignature(ClassCode = 0x03, SubClassCode = 0x00, ProgIF = 0x00, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class GenericVGASignature : PCIDeviceDriverSignature, IPCIDeviceDriverSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericVGASignature"/> class.
		/// </summary>
		public GenericVGASignature()
		{
			Platforms = PlatformArchitecture.x86; 
			ClassCode = 0x03;
			SubClassCode = 0x00;
			ProgIF = 0x00;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns></returns>
		public IHardwareDevice CreateInstance()
		{			
			return new GenericVGA();
		}	
	}
}
