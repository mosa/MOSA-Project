/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers
{
	/// <summary>
	/// Setup for the Device Drivers.
	/// </summary>
	public static class Setup
	{
		/// <summary>
		/// Initializes the Device Driver System.
		/// </summary>
		static public void Initialize(DeviceDriverRegistry registry)
		{
			Mosa.DeviceDrivers.Signatures.Setup.Initialize(registry);
		}
	}
}
