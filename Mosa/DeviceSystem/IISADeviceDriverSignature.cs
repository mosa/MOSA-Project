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
	public interface IISADeviceDriverSignature : IDeviceDriverSignature
	{		
		/// <summary>
		/// 
		/// </summary>
		ushort BasePort { get; }
		/// <summary>
		/// 
		/// </summary>
		ushort PortRange { get; }
		/// <summary>
		/// 
		/// </summary>
		ushort AltBasePort { get; }
		/// <summary>
		/// 
		/// </summary>
		ushort AltPortRange { get; }
		/// <summary>
		/// 
		/// </summary>
		bool AutoLoad { get; }
		/// <summary>
		/// 
		/// </summary>
		string ForceOption { get; }
		/// <summary>
		/// 
		/// </summary>
		byte IRQ { get; }
		/// <summary>
		/// 
		/// </summary>
		uint BaseAddress { get; }
		/// <summary>
		/// 
		/// </summary>
		uint AddressRange { get; }

	}

}
