/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	public interface IDevice
	{
		string Name { get; }
		IDevice Parent { get; }
		DeviceStatus Status { get; }
	}
}
