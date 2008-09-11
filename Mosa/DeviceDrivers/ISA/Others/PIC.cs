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

using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.Kernel;
using Mosa.DeviceDrivers.PCI;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0020, PortRange = 2, AltBasePort = 0x00A0, AltPortRange = 2, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PIC : ISAHardwareDevice, IDevice, IHardwareDevice
	{
		#region Definitions

		protected const byte IRQBaseSize = 0x08;
		protected const byte MasterIRQBase = 0x20;
		protected const byte SlaveIRQBase = MasterIRQBase + IRQBaseSize;
		protected const byte EOI = 0x20;

		#endregion

		protected IReadWriteIOPort masterCommandPort;
		protected IReadWriteIOPort masterDataPort;

		protected IReadWriteIOPort slaveCommandPort;
		protected IReadWriteIOPort slaveDataPort;

		// Interrupt masks must be tracked via the driver
		protected byte masterInterruptMask;
		protected byte slaveInterruptMask;

		public PIC() { }

		public override bool Setup()
		{
			base.name = "PIC_0x" + base.busResources.GetIOPort(0, 0).Address.ToString("X");

			masterCommandPort = base.busResources.GetIOPort(0, 0);
			masterDataPort = base.busResources.GetIOPort(0, 1);

			slaveCommandPort = base.busResources.GetIOPort(1, 0);
			slaveDataPort = base.busResources.GetIOPort(1, 1);

			return true;
		}

		public override bool Probe() { return true; }

		public override bool Start()
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

			return true;
		}

		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		public override bool OnInterrupt() { return false; }

		public void SendEndOfInterrupt(byte irq)
		{
			if (irq >= 8)
				slaveCommandPort.Write8(EOI);

			masterCommandPort.Write8(EOI);
		}

		public void DisableIRQs()
		{
			masterInterruptMask = 0xFF;

			masterDataPort.Write8(masterInterruptMask);

			slaveInterruptMask = 0xFF;
			slaveDataPort.Write8(slaveInterruptMask);
		}

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

		protected void EnableSlaveIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Clear bit
			slaveInterruptMask = (byte)(slaveInterruptMask & (~(1 << value)));
			slaveDataPort.Write8(masterInterruptMask);
		}

		public void EnableIRQ(byte value)
		{
			if ((value >= MasterIRQBase) && (value < SlaveIRQBase + IRQBaseSize)) {
				if (value < SlaveIRQBase)
					EnableMasterIRQ((byte)(value - MasterIRQBase));
				else
					EnableSlaveIRQ((byte)(value - SlaveIRQBase));
			}
		}

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

		protected void DisableSlaveIRQ(byte value)
		{
			// Mask out all but the last three bits
			value = (byte)(value & 0x07);

			// Set bit
			slaveInterruptMask = (byte)(slaveInterruptMask | (1 << value));
			slaveDataPort.Write8(masterInterruptMask);
		}


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

