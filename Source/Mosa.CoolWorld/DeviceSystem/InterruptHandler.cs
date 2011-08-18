/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class InterruptHandler : IInterruptHandler
	{
		/// <summary>
		/// 
		/// </summary>
		protected InterruptManager interruptManager;
		/// <summary>
		/// 
		/// </summary>
		protected byte irq;
		/// <summary>
		/// 
		/// </summary>
		protected IHardwareDevice hardwareDevice;

		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ { get { return irq; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="InterruptHandler"/> class.
		/// </summary>
		/// <param name="interruptManager">The interrupt manager.</param>
		/// <param name="irq">The irq.</param>
		/// <param name="hardwareDevice">The hardware device.</param>
		public InterruptHandler(InterruptManager interruptManager, byte irq, IHardwareDevice hardwareDevice)
		{
			this.interruptManager = interruptManager;
			this.irq = irq;
			this.hardwareDevice = hardwareDevice;
		}

		/// <summary>
		/// Enables this instance.
		/// </summary>
		public void Enable()
		{
			if (irq != 0xFF)
				interruptManager.AddInterruptHandler(irq, hardwareDevice);
		}

		/// <summary>
		/// Disables this instance.
		/// </summary>
		public void Disable()
		{
			if (irq != 0xFF)
				interruptManager.ReleaseInterruptHandler(irq, hardwareDevice);
		}
	}
}
