/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.DeviceDrivers.ISA
{
	[AttributeUsage(AttributeTargets.All | AttributeTargets.Property, AllowMultiple = true)]
	public class ISADeviceSignatureAttribute : System.Attribute
	{
		public PlatformArchitecture Platforms = PlatformArchitecture.None;
		public ushort BasePort = 0x00;
		public ushort PortRange = 0;
		public bool AutoLoad = false;   // (For built-in drivers only) Set to true if device is expected and has a relatively safe probe method.
		public string ForceOption = string.Empty;
		public byte IRQ = 0;
		public uint BaseAddress = 0x00;
		public uint AddressRange = 0x00;
	}
}
