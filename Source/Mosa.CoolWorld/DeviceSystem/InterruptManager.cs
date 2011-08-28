/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// The Interrupt Manager dispatches interrupts to the approporate hardware device drivers
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
		/// Initializes a new instance of the <see cref="InterruptManager"/> class.
		/// </summary>
		public InterruptManager()
		{
			interruptHandlers = new LinkedList<IHardwareDevice>[MaxInterrupts];

			for (int i = 0; i < MaxInterrupts; i++)
				interruptHandlers[i] = new LinkedList<IHardwareDevice>();
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void ProcessInterrupt(byte irq, byte error)
		{
			try
			{
				spinLock.Enter();
				foreach (IHardwareDevice hardwareDevice in interruptHandlers[irq])
					hardwareDevice.OnInterrupt();
			}
			finally
			{
				spinLock.Exit();
			}
		}

		/// <summary>
		/// Adds the interrupt handler.
		/// </summary>
		/// <param name="irq">The irq.</param>
		/// <param name="hardwareDevice">The hardware device.</param>
		public void AddInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
			try
			{
				spinLock.Enter();
				interruptHandlers[irq].Add(hardwareDevice);
			}
			finally
			{
				spinLock.Exit();
			}
		}

		/// <summary>
		/// Releases the interrupt handler.
		/// </summary>
		/// <param name="irq">The irq.</param>
		/// <param name="hardwareDevice">The hardware device.</param>
		public void ReleaseInterruptHandler(byte irq, IHardwareDevice hardwareDevice)
		{
			try
			{
				spinLock.Enter();
				interruptHandlers[irq].Remove(hardwareDevice);
			}
			finally
			{
				spinLock.Exit();
			}
		}

	}
}
