/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platforms.x86;

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class IDT
	{
		private static uint _idtTable = 0x1401000;
		private static uint _idtEntries = 0x1401000 + 6;

		internal const byte IDT_BaseLow = 0x00;
		internal const byte IDT_Select = 0x02;
		internal const byte IDT_Always0 = 0x04;
		internal const byte IDT_Flags = 0x05;
		internal const byte IDT_BaseHigh = 0x06;
		internal const byte IDT_Size = 0x06;

		public static void Setup()
		{
			Memory.Clear(_idtTable, 6);

			Memory.Set16(_idtTable, 3);
			Memory.Set32(_idtTable + 2, _idtEntries);

			Memory.Clear(_idtEntries, IDT_Size * 256);

			uint jumpTable = 0x0; // TODO

			Set(0, jumpTable, 0x08, 0x8E);
			Set(1, jumpTable, 0x08, 0x8E);
			// TODO			
			Set(31, jumpTable, 0x08, 0x8E);

			Native.Lidt(_idtTable);
		}

		private static void Set(uint index, uint address, byte select, byte flags)
		{
			Memory.Set16(_idtEntries + (index * IDT_Size) + IDT_BaseLow, (ushort)(address & 0xFFFF));
			Memory.Set16(_idtEntries + (index * IDT_Size) + IDT_BaseHigh, (ushort)((address >> 16) & 0xFF));
			Memory.Set8(_idtEntries + (index * IDT_Size) + IDT_Select, select);
			Memory.Set8(_idtEntries + (index * IDT_Size) + IDT_Always0, 0);
			Memory.Set8(_idtEntries + (index * IDT_Size) + IDT_Flags, flags);
		}

		// TODO: Build this w/o any calling convention
		private static void ExitInterrupt()
		{
			Native.Popad();		// Pop edi,esi,ebp,esp,ebx,edx,ecx,eax
			Native.Pop8();		// Pop error code
			Native.Pop8();		// Pop interrupt number
			Native.Sti();		// Enable Interrupts
			Native.IRetd();		// Return from Interrupt
		}

		#region IRQ x 256

		// All IRQ Handler follow this sequence of instructions
		// Native.Cli();		// Disable Interrupts
		// Native.Push8(ERROR);	// Push Error Code
		// Native.Push8(IRQ);	// Push Interrupt Number
		// Native.Pushad();		// Push edi,esi,ebp,esp,ebx,edx,ecx,eax
		// Native.JumpProcessInterrupt(); // Jumps to the kernel's interrupt handling function

		// Note: Only interrupts 8, 10, 11, 12, 13, and 14 push error codes onto the stack

		// TODO: Build these w/o any calling convension
		private static void IRQ0() { Native.Cli(); Native.Push8(0); Native.Push8(0); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ1() { Native.Cli(); Native.Push8(0); Native.Push8(1); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ2() { Native.Cli(); Native.Push8(0); Native.Push8(2); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ3() { Native.Cli(); Native.Push8(0); Native.Push8(3); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ4() { Native.Cli(); Native.Push8(0); Native.Push8(4); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ5() { Native.Cli(); Native.Push8(0); Native.Push8(5); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ6() { Native.Cli(); Native.Push8(0); Native.Push8(6); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ7() { Native.Cli(); Native.Push8(0); Native.Push8(7); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ8() { Native.Cli(); Native.Push8(8); Native.Pushad(); Native.JumpProcessInterrupt(); }
		private static void IRQ9() { Native.Cli(); Native.Push8(0); Native.Push8(9); Native.Pushad(); Native.JumpProcessInterrupt(); }

		// TODO: Add all 256!

		#endregion

	}
}
