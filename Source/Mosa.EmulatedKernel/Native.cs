/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platform.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Native
	{

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
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set32(uint location, uint value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write32(location, value);
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set16(uint location, ushort value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write16(location, value);
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set8(uint location, byte value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write8(location, value);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static byte Get8(uint location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read8(location);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static ushort Get16(uint location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read16(location);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static uint Get32(uint location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read32(location);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static ulong Get64(ulong location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read64((uint)location);
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
			switch (function)
			{
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
		public static void Lidt(uint address) { return; }

		/// <summary>
		/// Wraps the x86 hlt instruction
		/// </summary>
		public static void Hlt() { return; }

		/// <summary>
		/// Wraps the x86 sti instruction to enable interrupts
		/// </summary>
		public static void Sti() { return; }

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		public static void Cli() { return; }

		/// <summary>
		/// Jumps the global interrupt handler.
		/// </summary>
		/// <returns></returns>
		public static void JumpProcessInterrupt() { }

		/// <summary>
		/// Sets the control register.
		/// </summary>
		/// <param name="status">The status.</param>
		public static void SetCR0(uint status)
		{
			Mosa.EmulatedKernel.MemoryDispatch.CR0 = status;
		}

		/// <summary>
		/// Sets the control register.
		/// </summary>
		public static void SetCR2(uint status)
		{
			//Mosa.EmulatedKernel.MemoryDispatch.CR2 = status;
		}
		/// <summary>
		/// Sets the control register.
		/// </summary>
		public static void SetCR3(uint status)
		{
			Mosa.EmulatedKernel.MemoryDispatch.CR3 = status;
		}

		/// <summary>
		/// Gets the control register.
		/// </summary>
		/// <returns></returns>
		public static uint GetCR0()
		{
			return Mosa.EmulatedKernel.MemoryDispatch.CR0;
		}

		/// <summary>
		/// Gets the control register.
		/// </summary>
		/// <returns></returns>
		public static uint GetCR2()
		{
			return 0;// Mosa.EmulatedKernel.MemoryDispatch.CR2;
		}

		/// <summary>
		/// Gets the control register.
		/// </summary>
		/// <returns></returns>
		public static uint GetCR3()
		{
			return Mosa.EmulatedKernel.MemoryDispatch.CR3;
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

		/// <summary>
		/// Switches the task.
		/// </summary>
		/// <param name="esp">The esp.</param>
		public static void SwitchTask(uint esp)
		{
		}

	}
}
