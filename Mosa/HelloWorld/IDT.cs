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
		private static void GlobalInterruptHandler(byte irq, byte errorcode)
		{
			Native.Pushad();// Push edi,esi,ebp,esp,ebx,edx,ecx,eax
			//Native.Jmp(CommonInterruptHandler);
			Native.Popad(); // Pop edi,esi,ebp,esp,ebx,edx,ecx,eax
			Native.Sti();
			Native.IRetd();
		}

		#region IRQ x 256

		// TODO: Build this w/o any calling convention
		private static void IRQ0() 
		{
			Native.Cli();	// Disable Interrupts
			Native.JumpGlobalInterruptHandler();
		}

		#endregion

	}
}
