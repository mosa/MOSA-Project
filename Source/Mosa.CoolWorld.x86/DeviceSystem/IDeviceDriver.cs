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
	public interface IDeviceDriver
	{
		/// <summary>
		/// 
		/// </summary>
		PlatformArchitecture Platforms { get; }
	}

}
