/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// Intel® 82093AA I/O Advanced Programmable Interrupt Controller (I/O APIC) Datasheet
// http://www.intel.com/design/chipsets/datashts/29056601.pdf

// 8259A Programmable Interrupt Controller:
// http://pdos.csail.mit.edu/6.828/2005/readings/hardware/8259A.pdf

using Mosa.ClassLib;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
	/// Programmable Interrupt Controller (PIC) Device Driver
    /// </summary>
	[ISADeviceDriver(AutoLoad = true, BasePort = 0x20, PortRange = 2, AltBasePort = 0xA0, AltPortRange = 2, Platforms = PlatformArchitecture.X86AndX64)]
	public class PIC : HardwareDevice, IDevice, IHardwareDevice
	{
		#region Definitions

        /// <summary>
        /// 
        /// </summary>
		protected const byte IRQBaseSize = 0x08;
        /// <summary>
        /// 
        /// </summary>
		protected const byte MasterIRQBase = 0x20;
        /// <summary>
        /// 
        /// </summary>
		protected const byte SlaveIRQBase = MasterIRQBase + IRQBaseSize;
        /// <summary>
        /// 
        /// </summary>
		protected const byte EOI = 0x20;

		#endregion

        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort masterCommandPort;
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort masterDataPort;

        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort slaveCommandPort;
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort slaveDataPort;

		// Interrupt masks must be tracked via the driver
        /// <summary>
        /// 
        /// </summary>
		protected byte masterInterruptMask;
        /// <summary>
        /// 
        /// </summary>
		protected byte slaveInterruptMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="PIC"/> class.
        /// </summary>
		public PIC() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <param name="hardwareResources"></param>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;				
			base.name = "PIC_0x" + base.hardwareResources.GetIOPort(0, 0).Address.ToString("X");

			masterCommandPort = base.hardwareResources.GetIOPort(0, 0);
			masterDataPort = base.hardwareResources.GetIOPort(0, 1);

			slaveCommandPort = base.hardwareResources.GetIOPort(1, 0);
			slaveDataPort = base.hardwareResources.GetIOPort(1, 1);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			byte masterMask;
			byte slaveMask;

			// Save Masks
			masterMask = masterDataPort.Read8();
			slaveMask = slaveDataPort.Read8();

			// ICW1 - Set Initialize Controller & Expect ICW4
			masterCommandPort.Write8(0x11);
			// ICW2 - interrupt offset 
			masterDataPort.Write8(MasterIRQBase);
			// ICW3
			masterDataPort.Write8(0x02);
			// ICW4 - Set 8086 Mode
			masterDataPort.Write8(0x01);

			// ICW1 - Set Initialize Controller & Expect ICW4
			slaveCommandPort.Write8(0x11);
			// ICW2 - interrupt offset 
			slaveDataPort.Write8(SlaveIRQBase);
			// ICW3
			slaveDataPort.Write8(0x02);
			// ICW4 - Set 8086 Mode
			slaveDataPort.Write8(0x01);

			// Restore Masks
			masterDataPort.Write8(masterMask);
			slaveCommandPort.Write8(slaveMask);

			DisableIRQs();

			base.deviceStatus = DeviceStatus.Online;
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

        /// <summary>
        /// Sends the end of interrupt.
        /// </summary>
        /// <param name="irq">The irq.</param>
		public void SendEndOfInterrupt(byte irq)
		{
			if (irq >= 8)
				slaveCommandPort.Write8(EOI);

			masterCommandPort.Write8(EOI);
		}

        /// <summary>
        /// Disables the IR qs.
        /// </summary>
		public void DisableIRQs()
		{
			masterInterruptMask = 0xFF;

			masterDataPort.Write8(masterInterruptMask);

			slaveInterruptMask = 0xFF;
			slaveDataPort.Write8(slaveInterruptMask);
		}

        /// <summary>
        /// Enables the master IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		protected void EnableMasterIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Cannot mask IRQ2
			if (value == 2)
				return;

			// Clear bit
			masterInterruptMask = (byte)(masterInterruptMask & (~(1 << value)));
			masterDataPort.Write8(masterInterruptMask);
		}

        /// <summary>
        /// Enables the slave IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		protected void EnableSlaveIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Clear bit
			slaveInterruptMask = (byte)(slaveInterruptMask & (~(1 << value)));
			slaveDataPort.Write8(masterInterruptMask);
		}

        /// <summary>
        /// Enables the IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		public void EnableIRQ(byte value)
		{
			if ((value >= MasterIRQBase) && (value < SlaveIRQBase + IRQBaseSize)) {
				if (value < SlaveIRQBase)
					EnableMasterIRQ((byte)(value - MasterIRQBase));
				else
					EnableSlaveIRQ((byte)(value - SlaveIRQBase));
			}
		}

        /// <summary>
        /// Disables the master IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		protected void DisableMasterIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Cannot mask IRQ2
			if (value == 2)
				return;

			// Set bit
			masterInterruptMask = (byte)(masterInterruptMask | (1 << value));
			masterDataPort.Write8(masterInterruptMask);
		}

        /// <summary>
        /// Disables the slave IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		protected void DisableSlaveIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Set bit
			slaveInterruptMask = (byte)(slaveInterruptMask | (1 << value));
			slaveDataPort.Write8(masterInterruptMask);
		}


        /// <summary>
        /// Disables the IRQ.
        /// </summary>
        /// <param name="value">The value.</param>
		public void DisableIRQ(byte value)
		{
			if ((value >= MasterIRQBase) && (value < SlaveIRQBase + IRQBaseSize)) {
				if (value < SlaveIRQBase)
					DisableMasterIRQ((byte)(value - MasterIRQBase));
				else
					DisableSlaveIRQ((byte)(value - SlaveIRQBase));
			}
		}
	}
}

