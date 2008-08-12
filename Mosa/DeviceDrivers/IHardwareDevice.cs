/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{
	public interface IHardwareDevice
	{
		bool Setup();
		bool Probe();
		bool Start();
		LinkedList<IDevice> CreateSubDevices();
		bool OnInterrupt();
		bool Activate(DeviceManager deviceManager);
	}
}
