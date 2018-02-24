// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interrupt Handler
	/// </summary>
	public sealed class InterruptHandler
	{
		public InterruptManager InterruptManager { get; private set; }

		public Device HardwareDevice { get; private set; }

		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		public byte IRQ { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="InterruptHandler" /> class.
		/// </summary>
		/// <param name="interruptManager">The interrupt manager.</param>
		/// <param name="irq">The irq.</param>
		/// <param name="hardwareDevice">The hardware device.</param>
		public InterruptHandler(InterruptManager interruptManager, byte irq, Device hardwareDevice)
		{
			InterruptManager = interruptManager;
			HardwareDevice = hardwareDevice;
			IRQ = irq;
		}

		/// <summary>
		/// Enables this instance.
		/// </summary>
		public void Enable()
		{
			if (IRQ != 0xFF)
			{
				InterruptManager.AddInterruptHandler(IRQ, HardwareDevice.DeviceDriver as IHardwareDevice);
			}
		}

		/// <summary>
		/// Disables this instance.
		/// </summary>
		public void Disable()
		{
			if (IRQ != 0xFF)
			{
				InterruptManager.ReleaseInterruptHandler(IRQ, HardwareDevice.DeviceDriver as IHardwareDevice);
			}
		}
	}
}
