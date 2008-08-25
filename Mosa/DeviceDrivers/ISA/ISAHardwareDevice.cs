/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers.ISA
{

	public abstract class ISAHardwareDevice : HardwareDevice
	{
		protected BusResources busResources;

		public ISAHardwareDevice() : base() { }

		public void AssignResources(BusResources busResources)
		{
			this.busResources = busResources;
		}

	}
}
