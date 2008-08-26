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
	[Flags]
	public enum PlatformArchitecture
	{
		None = 0,	

		x86 = 1,
		x64 = 2,
		
		Both_x86_and_x64 = PlatformArchitecture.x86 | PlatformArchitecture.x64,
	}
}
