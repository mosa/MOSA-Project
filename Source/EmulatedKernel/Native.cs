/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Native
	{

		/// <summary>
		/// Sets the cr0.
		/// </summary>
		/// <param name="state">The state.</param>
		public static void SetCR0(uint state)
		{
			EmulatedKernel.MemoryDispatch.CR0 = state;
		}

		/// <summary>
		/// Sets the cr3.
		/// </summary>
		/// <param name="pagetable">The pagetable.</param>
		public static void SetCR3(uint pagetable)
		{
			EmulatedKernel.MemoryDispatch.CR3 = pagetable;
		}

		/// <summary>
		/// Outs the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Out(byte address, byte value)
		{
			EmulatedKernel.IOPortDispatch.Write8(address, value);
		}

		/// <summary>
		/// Outs the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Out8(byte address, byte value)
		{
			EmulatedKernel.IOPortDispatch.Write8(address, value);
		}

		/// <summary>
		/// Ins the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static byte In(byte address)
		{
			return EmulatedKernel.IOPortDispatch.Read8(address);
		}

		/// <summary>
		/// Ins the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static byte In8(byte address)
		{
			return EmulatedKernel.IOPortDispatch.Read8(address);
		}

		/// <summary>
		/// Nop
		/// </summary>
		public static void Nop()
		{
			return; // Nothing to do
		}

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		public static int CpuIdEax(uint function)
		{
			switch (function) {
				case 1: return 0x01020304;
				case 2147483650: return 0x41534F4D;
				case 2147483651: return 0x0;
				case 2147483652: return 0x0;
				default: return 0x0;
			}
		}

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		public static int CpuIdEbx(uint function)
		{
			return 0x0;
		}

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		public static int CpuIdEcx(uint function)
		{
			return 0x0;
		}

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		public static int CpuIdEdx(uint function)
		{
			return 0x0;
		}

		/// <summary>
		/// Invlpgs the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		public static void Invlpg(uint address)
		{
			return; // Nothing to do, emulator doesn't emulate or cache TLB entries
		}

		/// <summary>
		/// Loads the GDT
		/// </summary>
		/// <param name="address">The address.</param>
		public static void Lgdt(uint address)
		{
			return; // Nothing to do, emulator doesn't emulator the GDT
		}

		/// <summary>
		/// Loads the IDT
		/// </summary>
		/// <param name="address">The address.</param>
		public static void Lidt(uint address)
		{
			// TODO
		}

		/// <summary>
		/// Wraps the x86 pop instruction to pop a value from the stack
		/// </summary>
		public static byte Pop8() { return 0; }

		/// <summary>
		/// Wraps the x86 pop instruction to pop a value from the stack
		/// </summary>
		public static ushort Pop16() { return 0; }

		/// <summary>
		/// Wraps the x86 pop instruction to pop a value from the stack
		/// </summary>
		public static uint Pop32() { return 0; }

		/// <summary>
		/// Wraps the x86 popad instruction to pop all GPR from the stack
		/// </summary>
		public static void Popad() { return; }

		/// <summary>
		/// Wraps the x86 push instruction to push a value on the stack
		/// </summary>
		public static void Push8(byte value) { return; }

		/// <summary>
		/// Wraps the x86 push instruction to push a value on the stack
		/// </summary>
		public static void Push16(ushort value) { return; }

		/// <summary>
		/// Wraps the x86 push instruction to push a value on the stack
		/// </summary>
		public static void Push32(uint value) { return; }

		/// <summary>
		/// Wraps the x86 pushad instruction to push all GPR to the stack
		/// </summary>
		public static void Pushad() { return; }

		/// <summary>
		/// Wraps the x86 sti instruction to enable interrupts
		/// </summary>
		public static void Sti() { return; }

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		public static void Cli() { return; }

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		public static void IRetd() { return; }

		/// <summary>
		/// Jumps the global interrupt handler.
		/// </summary>
		/// <returns></returns>
		public static void JumpProcessInterrupt() { }

		/// <summary>
		/// Sets the control register.
		/// </summary>
		/// <param name="register">The control register</param>
		/// <param name="status">The status.</param>
		public static void SetControlRegister(byte register, uint status)
		{
			switch (register) {
				case 0: Mosa.EmulatedKernel.MemoryDispatch.CR0 = status; return;
				case 3: Mosa.EmulatedKernel.MemoryDispatch.CR3 = status; return;
				default: return;
			}
		}

		/// <summary>
		/// Gets the control register.
		/// </summary>
		/// <param name="register">The pagedirectory.</param>
		/// <returns></returns>
		public static uint GetControlRegister(byte register)
		{
			switch (register) {
				case 0: return Mosa.EmulatedKernel.MemoryDispatch.CR0;
				case 3: return Mosa.EmulatedKernel.MemoryDispatch.CR3;
				default: return 0;
			}
		}

		/// <summary>
		/// Gets the IDT jump location.
		/// </summary>
		/// <param name="irq">The irq.</param>
		/// <returns></returns>
		public static uint GetIDTJumpLocation(uint irq)
		{
			return 0;
		}

	}
}
