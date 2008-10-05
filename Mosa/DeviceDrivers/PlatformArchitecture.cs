/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.DeviceDrivers
{
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum PlatformArchitecture
	{
        /// <summary>
        /// 
        /// </summary>
		None = 0,

        /// <summary>
        /// 
        /// </summary>
		x86 = 1,
        /// <summary>
        /// 
        /// </summary>
		x64 = 2,

        /// <summary>
        /// 
        /// </summary>
		Both_x86_and_x64 = PlatformArchitecture.x86 | PlatformArchitecture.x64,
	}
}
