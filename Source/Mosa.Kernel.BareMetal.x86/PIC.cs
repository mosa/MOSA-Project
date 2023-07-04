// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Programmable Interrupt Controller (PIC)
/// </summary>
public static class PIC
{
	#region PIC Constants

	internal struct PICConstants
	{
		internal const byte ICW1_ICW4 = 0x01;
		internal const byte ICW1_SingleCascadeMode = 0x02;
		internal const byte ICW1_Interval4 = 0x04;
		internal const byte ICW1_LevelTriggeredEdgeMode = 0x08;
		internal const byte ICW1_Initialization = 0x10;
		internal const byte ICW2_MasterOffset = 0x20;
		internal const byte ICW2_SlaveOffset = 0x28;
		internal const byte ICW4_8086 = 0x01;
		internal const byte ICW4_AutoEndOfInterrupt = 0x02;
		internal const byte ICW4_BufferedSlaveMode = 0x08;
		internal const byte ICW4_BufferedMasterMode = 0x0C;
		internal const byte ICW4_SpecialFullyNested = 0x10;

		internal const byte PIC1_Command = 0x20;
		internal const byte PIC2_Command = 0xA0;
		internal const byte PIC1_Data = 0x21;
		internal const byte PIC2_Data = 0xA1;

		internal const byte EOI = 0x20;
	}

	#endregion PIC Constants

	public static void Setup()
	{
		//Debug.WriteLine("PIC::Setup()");

		byte masterMask = Native.In8(PICConstants.PIC1_Data);
		byte slaveMask = Native.In8(PICConstants.PIC2_Data);

		// ICW1 - Set Initialize Controller & Expect ICW4
		Native.Out8(PICConstants.PIC1_Command, PICConstants.ICW1_Initialization + PICConstants.ICW1_ICW4);

		// ICW2 - interrupt offset
		Native.Out8(PICConstants.PIC1_Data, PICConstants.ICW2_MasterOffset);

		// ICW3
		Native.Out8(PICConstants.PIC1_Data, 4);

		// ICW4 - Set 8086 Mode
		Native.Out8(PICConstants.PIC1_Data, PICConstants.ICW4_8086);

		// OCW1
		Native.Out8(PICConstants.PIC1_Data, masterMask);

		// ICW1 - Set Initialize Controller & Expect ICW4
		Native.Out8(PICConstants.PIC2_Command, PICConstants.ICW1_Initialization + PICConstants.ICW1_ICW4);

		// ICW2 - interrupt offset
		Native.Out8(PICConstants.PIC2_Data, PICConstants.ICW2_SlaveOffset);

		// ICW3
		Native.Out8(PICConstants.PIC2_Data, 2);

		// ICW4 - Set 8086 Mode
		Native.Out8(PICConstants.PIC2_Data, PICConstants.ICW4_8086);

		// OCW1
		Native.Out8(PICConstants.PIC2_Data, slaveMask);

		//Debug.WriteLine("PIC::Complete()");
	}

	/// <summary>
	/// Sends the end of interrupt.
	/// </summary>
	/// <param name="irq">The irq.</param>
	public static void SendEndOfInterrupt(uint irq)
	{
		if (irq >= 40) // or untranslated IRQ >= 8
		{
			Native.Out8(PICConstants.PIC2_Command, PICConstants.EOI);
		}

		Native.Out8(PICConstants.PIC1_Command, PICConstants.EOI);
	}
}
