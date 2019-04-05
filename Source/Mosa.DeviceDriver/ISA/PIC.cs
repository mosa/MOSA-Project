// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Intel® 82093AA I/O Advanced Programmable Interrupt Controller (I/O APIC) Datasheet
// http://www.intel.com/design/chipsets/datashts/29056601.pdf

// 8259A Programmable Interrupt Controller:
// http://pdos.csail.mit.edu/6.828/2005/readings/hardware/8259A.pdf

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// Programmable Interrupt Controller (PIC) Device Driver
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x20, PortRange = 2, AltBasePort = 0xA0, AltPortRange = 2, Platforms = PlatformArchitecture.X86AndX64)]
	public class PIC : BaseDeviceDriver
	{
		#region Definitions

		protected const byte IRQBaseSize = 0x08;

		protected const byte MasterIRQBase = 0x20;

		protected const byte SlaveIRQBase = MasterIRQBase + IRQBaseSize;

		protected const byte EOI = 0x20;

		#endregion Definitions

		protected BaseIOPortReadWrite masterCommandPort;

		protected BaseIOPortReadWrite masterDataPort;

		protected BaseIOPortReadWrite slaveCommandPort;

		protected BaseIOPortReadWrite slaveDataPort;

		protected byte masterInterruptMask;

		protected byte slaveInterruptMask;

		public override void Initialize()
		{
			Device.Name = "PIC_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			masterCommandPort = Device.Resources.GetIOPortReadWrite(0, 0);
			masterDataPort = Device.Resources.GetIOPortReadWrite(0, 1);

			slaveCommandPort = Device.Resources.GetIOPortReadWrite(1, 0);
			slaveDataPort = Device.Resources.GetIOPortReadWrite(1, 1);
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			Device.Status = DeviceStatus.Online;

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
			masterDataPort.Write8(0x04);

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
			slaveDataPort.Write8(slaveMask);

			DisableIRQs();
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			return false;
		}

		/// <summary>
		/// Sends the end of interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void SendEndOfInterrupt(byte irq)
		{
			if (irq >= 8)
			{
				slaveCommandPort.Write8(EOI);
			}

			masterCommandPort.Write8(EOI);
		}

		/// <summary>
		/// Disables the IRQs.
		/// </summary>
		public void DisableIRQs()
		{
			masterInterruptMask = 0xFF;
			masterDataPort.Write8(masterInterruptMask);

			slaveInterruptMask = 0xFF;
			slaveDataPort.Write8(slaveInterruptMask);
		}

		/// <summary>
		/// Enables the master IRQs.
		/// </summary>
		/// <param name="irq">The irq.</param>
		protected void EnableMasterIRQ(byte irq)
		{
			// Mask out all but the last three bits
			irq = (byte)(irq & 0x07);

			// Cannot mask IRQ2
			if (irq == 2)
				return;

			// Clear bit
			masterInterruptMask = (byte)(masterInterruptMask & (~(1 << irq)));
			masterDataPort.Write8(masterInterruptMask);
		}

		/// <summary>
		/// Enables the slave IRQ.
		/// </summary>
		/// <param name="irq">The irq.</param>
		protected void EnableSlaveIRQ(byte irq)
		{
			// Mask out all but the last three bits
			irq = (byte)(irq & 0x07);

			// Clear bit
			slaveInterruptMask = (byte)(slaveInterruptMask & (~(1 << irq)));
			slaveDataPort.Write8(masterInterruptMask);
		}

		/// <summary>
		/// Enables the IRQ.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void EnableIRQ(byte irq)
		{
			if ((irq >= MasterIRQBase) && (irq < SlaveIRQBase + IRQBaseSize))
			{
				if (irq < SlaveIRQBase)
					EnableMasterIRQ((byte)(irq - MasterIRQBase));
				else
					EnableSlaveIRQ((byte)(irq - SlaveIRQBase));
			}
		}

		/// <summary>
		/// Disables the master IRQ.
		/// </summary>
		/// <param name="irq">The irq.</param>
		protected void DisableMasterIRQ(byte irq)
		{
			// Mask out all but the last three bits
			irq = (byte)(irq & 0x07);

			// Cannot mask IRQ2
			if (irq == 2)
				return;

			// Set bit
			masterInterruptMask = (byte)(masterInterruptMask | (1 << irq));
			masterDataPort.Write8(masterInterruptMask);
		}

		/// <summary>
		/// Disables the slave IRQ.
		/// </summary>
		/// <param name="irq">The irq.</param>
		protected void DisableSlaveIRQ(byte irq)
		{
			// Mask out all but the last three bits
			irq = (byte)(irq & 0x07);

			// Set bit
			slaveInterruptMask = (byte)(slaveInterruptMask | (1 << irq));
			slaveDataPort.Write8(slaveInterruptMask);
		}

		/// <summary>
		/// Disables the IRQ.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public void DisableIRQ(byte irq)
		{
			if ((irq >= MasterIRQBase) && (irq < SlaveIRQBase + IRQBaseSize))
			{
				if (irq < SlaveIRQBase)
					DisableMasterIRQ((byte)(irq - MasterIRQBase));
				else
					DisableSlaveIRQ((byte)(irq - SlaveIRQBase));
			}
		}
	}
}
