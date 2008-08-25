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
		public const ushort MaxInterrupts = 16;

		protected LinkedList<IHardwareDevice>[] interruptHandlers;

		protected SpinLock spinLock;

		public InterruptManager()
		{
			interruptHandlers = new LinkedList<IHardwareDevice>[MaxInterrupts];

			for (int i = 0; i < MaxInterrupts; i++)
				interruptHandlers[i] = new LinkedList<IHardwareDevice>();
		}

		public void ProcessInterrupt(byte irq)
		{
			if (irq == 0) return;

			try {
				spinLock.Enter();
				foreach (IHardwareDevice hardwareDevice in interruptHandlers[irq])
					hardwareDevice.OnInterrupt();
			}
			finally {
				spinLock.Exit();
			}
		}

		public void AddInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
			if (irq == 0) return;

			try {
				spinLock.Enter();
				interruptHandlers[irq].Add(hardwareDevice);
			}
			finally {
				spinLock.Exit();
			}
		}

		public void ReleaseInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
			if (irq == 0) return;

			try {
				spinLock.Enter();
				interruptHandlers[irq].Remove(hardwareDevice);
			}
			finally {
				spinLock.Exit();
			}
		}

	}
}
