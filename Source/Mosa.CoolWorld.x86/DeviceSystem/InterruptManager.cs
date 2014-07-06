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
	/// The Interrupt Manager dispatches interrupts to the appropriate hardware device drivers
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
			{
				interruptHandlers[i] = new LinkedList<IHardwareDevice>();
			}
		}

		/// <summary>
		/// Processes the interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		/// <param name="error">The error.</param>
		public void ProcessInterrupt(uint irq, uint error)
		{
			var handlers = interruptHandlers[irq];

			//Mosa.Kernel.x86.Debug.Trace("Enter InterruptManager.ProcessInterrupt");

			//spinLock.Enter();

			//Mosa.Kernel.x86.Screen.Goto(14, 0);
			//Mosa.Kernel.x86.Screen.Write("::");
			//Mosa.Kernel.x86.Screen.Write(irq);

			//uint i = 0;
			foreach (var hardwareDevice in handlers)
			{
				//Mosa.Kernel.x86.Screen.Write(i++);
				hardwareDevice.OnInterrupt();
				//Mosa.Kernel.x86.Screen.Write(':');
			}

			//spinLock.Exit();
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