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
	public class InterruptManager
	{
		// TODO: array of devices for each IRQ 

		public InterruptManager() { }

		public void ProcessInterrupt(byte irq)
		{
		}

		// TODO: register an IRQ to a device
		// TODO: unregister a device from an IRQ
	}
}
