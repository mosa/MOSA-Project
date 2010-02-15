/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.HelloWorld;

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class PIC
	{
		public const byte ICW1_ICW4 = 0x01;
		public const byte ICW1_SingleCascadeMode = 0x02;
		public const byte ICW1_Interval4 = 0x04;
		public const byte ICW1_LevelTriggeredEdgeMode = 0x08;
		public const byte ICW1_Initialization = 0x10;
		public const byte ICW4_8086 = 0x01;
		public const byte ICW4_AutoEndOfInterrupt = 0x02;
		public const byte ICW4_BufferedSlaveMode = 0x08;
		public const byte ICW4_BufferedMasterMode = 0x0C;
		public const byte ICW4_SpecialFullyNested = 0x10;

		public const byte PIC1_Command = 0x20;
		public const byte PIC2_Command = 0xA0;
		public const byte PIC1_Data = 0x21;
		public const byte PIC2_Data = 0xA1;

		public static void Setup(byte masterOffset, byte slaveOffset)
		{
			byte masterMask = Native.In8(PIC1_Data);
			byte slaveMask = Native.In8(PIC2_Data);
			byte keyboard = (byte)(Native.In8(PIC1_Data) & 0xFD);
			Native.Out8(PIC1_Command, ICW1_Initialization + ICW1_ICW4);
			Native.Out8(PIC2_Command, ICW1_Initialization + ICW1_ICW4);
			Native.Out8(PIC1_Data, masterOffset);
			Native.Out8(PIC2_Data, slaveOffset);
			Native.Out8(PIC1_Data, ICW1_Interval4);
			Native.Out8(PIC2_Data, ICW4_AutoEndOfInterrupt);
			Native.Out8(PIC1_Data, ICW4_AutoEndOfInterrupt);
			Native.Out8(PIC1_Data, ICW4_8086);
			Native.Out8(PIC2_Data, ICW4_8086);
			Native.Out8(PIC1_Data, keyboard);
			Native.Out8(PIC1_Data, masterMask);
			Native.Out8(PIC2_Data, slaveMask);
		}

	}
}
