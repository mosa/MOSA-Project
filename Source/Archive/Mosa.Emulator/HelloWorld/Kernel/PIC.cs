/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Programmable Interrupt Controller (PIC)
	/// </summary>
	public static class PIC
	{
		private const byte ICW1_ICW4 = 0x01;
		private const byte ICW1_SingleCascadeMode = 0x02;
		private const byte ICW1_Interval4 = 0x04;
		private const byte ICW1_LevelTriggeredEdgeMode = 0x08;
		private const byte ICW1_Initialization = 0x10;
		private const byte ICW2_MasterOffset = 0x20;
		private const byte ICW2_SlaveOffset = 0x28;
		private const byte ICW4_8086 = 0x01;
		private const byte ICW4_AutoEndOfInterrupt = 0x02;
		private const byte ICW4_BufferedSlaveMode = 0x08;
		private const byte ICW4_BufferedMasterMode = 0x0C;
		private const byte ICW4_SpecialFullyNested = 0x10;

		private const byte PIC1_Command = 0x20;
		private const byte PIC2_Command = 0xA0;
		private const byte PIC1_Data = 0x21;
		private const byte PIC2_Data = 0xA1;

		private const byte EOI = 0x20;

		public static void Setup()
		{
			byte masterMask = Native.In8(PIC1_Data);
			byte slaveMask = Native.In8(PIC2_Data);

			// ICW1 - Set Initialize Controller & Expect ICW4
			Native.Out8(PIC1_Command, ICW1_Initialization + ICW1_ICW4);

			// ICW2 - interrupt offset
			Native.Out8(PIC1_Data, ICW2_MasterOffset);

			// ICW3
			Native.Out8(PIC1_Data, 4);

			// ICW4 - Set 8086 Mode
			Native.Out8(PIC1_Data, ICW4_8086);

			// OCW1
			Native.Out8(PIC1_Data, masterMask);

			// ICW1 - Set Initialize Controller & Expect ICW4
			Native.Out8(PIC2_Command, ICW1_Initialization + ICW1_ICW4);

			// ICW2 - interrupt offset
			Native.Out8(PIC2_Data, ICW2_SlaveOffset);

			// ICW3
			Native.Out8(PIC2_Data, 2);

			// ICW4 - Set 8086 Mode
			Native.Out8(PIC2_Data, ICW4_8086);

			// OCW1
			Native.Out8(PIC2_Data, slaveMask);
		}

		/// <summary>
		/// Sends the end of interrupt.
		/// </summary>
		/// <param name="irq">The irq.</param>
		public static void SendEndOfInterrupt(byte irq)
		{
			if (irq >= 40) // or untranslated IRQ >= 8
				Native.Out8(PIC2_Command, EOI);

			Native.Out8(PIC1_Command, EOI);
		}
	}
}