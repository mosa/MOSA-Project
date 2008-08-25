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
	public class InterruptHandler : IInterruptHandler
	{
		protected InterruptManager interruptManager;
		protected byte irq;
		protected IHardwareDevice hardwareDevice;

		public byte IRQ { get { return irq; } }

		public InterruptHandler(InterruptManager interruptManager, byte irq, IHardwareDevice hardwareDevice)
		{
			this.interruptManager = interruptManager;
			this.irq = irq;
			this.hardwareDevice = hardwareDevice;
		}

		public void Enable()
		{
			interruptManager.AddInterruptHandler(irq, hardwareDevice);
		}

		public void Disable()
		{
			interruptManager.ReleaseInterruptHandler(irq, hardwareDevice);
		}
	}
}
