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
    /// <summary>
    /// 
    /// </summary>
	public class InterruptManager
	{
        /// <summary>
        /// 
        /// </summary>
		public const ushort MaxInterrupts = 16;

        /// <summary>
        /// 
        /// </summary>
		protected LinkedList<IHardwareDevice>[] interruptHandlers;

        /// <summary>
        /// 
        /// </summary>
		protected SpinLock spinLock;

        /// <summary>
        /// 
        /// </summary>
		public InterruptManager()
		{
			interruptHandlers = new LinkedList<IHardwareDevice>[MaxInterrupts];

			for (int i = 0; i < MaxInterrupts; i++)
				interruptHandlers[i] = new LinkedList<IHardwareDevice>();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="irq"></param>
		public void ProcessInterrupt(byte irq)
		{
			try {
				spinLock.Enter();
				foreach (IHardwareDevice hardwareDevice in interruptHandlers[irq])
					hardwareDevice.OnInterrupt();
			}
			finally {
				spinLock.Exit();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="irq"></param>
        /// <param name="hardwareDevice"></param>
		public void AddInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
			try {
				spinLock.Enter();
				interruptHandlers[irq].Add(hardwareDevice);
			}
			finally {
				spinLock.Exit();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="irq"></param>
        /// <param name="hardwareDevice"></param>
		public void ReleaseInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
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
