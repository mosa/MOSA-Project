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

namespace Mosa.DeviceDrivers.Signatures
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class ISADeviceDriverSignature
	{
		/// <summary>
		/// 
		/// </summary>
		protected PlatformArchitecture platforms;
		/// <summary>
		/// 
		/// </summary>
		protected ushort basePort;
		/// <summary>
		/// 
		/// </summary>
		protected ushort portRange;
		/// <summary>
		/// 
		/// </summary>
		protected ushort altBasePort;
		/// <summary>
		/// 
		/// </summary>
		protected ushort altPortRange;
		/// <summary>
		/// 
		/// </summary>
		protected bool autoLoad;
		/// <summary>
		/// 
		/// </summary>
		protected string forceOption;
		/// <summary>
		/// 
		/// </summary>
		protected byte irq;
		/// <summary>
		/// 
		/// </summary>
		protected uint baseAddress;
		/// <summary>
		/// 
		/// </summary>
		protected uint addressRange;

		/// <summary>
		/// 
		/// </summary>
		public PlatformArchitecture Platforms { get { return platforms; } set { platforms = value; } }
		/// <summary>
		/// 
		/// </summary>
		public ushort BasePort { get { return basePort; } set { basePort = value; } }
		/// <summary>
		/// 
		/// </summary>
		public ushort PortRange { get { return portRange; } set { portRange = value; } }
		/// <summary>
		/// 
		/// </summary>
		public ushort AltBasePort { get { return altBasePort; } set { altBasePort = value; } }
		/// <summary>
		/// 
		/// </summary>
		public ushort AltPortRange { get { return altPortRange; } set { altPortRange = value; } }
		/// <summary>
		/// 
		/// </summary>
		public bool AutoLoad { get { return autoLoad; } set { autoLoad = value; } }
		/// <summary>
		/// 
		/// </summary>
		public string ForceOption { get { return forceOption; } set { forceOption = value; } }
		/// <summary>
		/// 
		/// </summary>
		public byte IRQ { get { return irq; } set { irq = value; } }
		/// <summary>
		/// 
		/// </summary>
		public uint BaseAddress { get { return baseAddress; } set { baseAddress = value; } }
		/// <summary>
		/// 
		/// </summary>
		public uint AddressRange { get { return addressRange; } set { addressRange = value; } }

	}
}
