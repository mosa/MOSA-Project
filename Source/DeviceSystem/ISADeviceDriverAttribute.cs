/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.All | System.AttributeTargets.Property, AllowMultiple = true)]
	public class ISADeviceDriverAttribute : System.Attribute, IDeviceDriver
	{
		/// <summary>
		/// 
		/// </summary>
		protected PlatformArchitecture platforms;
		/// <summary>
		/// </summary>
		/// <value></value>
		public PlatformArchitecture Platforms { get { return platforms; } set { platforms = value; } }
		/// <summary>
		/// 
		/// </summary>
		public ushort BasePort = 0x00;
		/// <summary>
		/// 
		/// </summary>
		public ushort PortRange = 0x00;
		/// <summary>
		/// 
		/// </summary>
		public ushort AltBasePort = 0x00;
		/// <summary>
		/// 
		/// </summary>
		public ushort AltPortRange = 0x00;
		/// <summary>
		/// 
		/// </summary>
		public bool AutoLoad = false;   // (For built-in drivers only) Set to true if device is expected and has a relatively safe probe method.
		/// <summary>
		/// 
		/// </summary>
		public string ForceOption = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public byte IRQ = 0xFF; // 0xFF means unused
		/// <summary>
		/// 
		/// </summary>
		public uint BaseAddress = 0x00;
		/// <summary>
		/// 
		/// </summary>
		public uint AddressRange = 0x00;
	}
}
