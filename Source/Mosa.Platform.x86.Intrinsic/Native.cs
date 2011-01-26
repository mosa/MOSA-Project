/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
*/

using System;

using Mosa.Intrinsic;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		/// <summary>
		/// Wraps the x86 ldit instruction to load the interrupt descriptor table.
		/// </summary>
		/// <param name="address">The address.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Lidt, Mosa.Platform.x86")]
		public static void Lidt(uint address) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Cli, Mosa.Platform.x86")]
		public static void Cli() { ThrowPlatformNotSupported(); return; }

		///// <summary>
		///// Wraps the x86 cmpxchg instruction 
		///// </summary>
		//[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CmpXchg, Mosa.Platform.x86")]
		//public static void CmpXchg16(ref short first, short second) { ThrowPlatformNotSupported(); return; }

		///// <summary>
		///// Wraps the x86 cmpxchg instruction 
		///// </summary>
		//[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CmpXchg, Mosa.Platform.x86")]
		//public static void CmpXchg32(ref int first, int second) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 lgdt instruction to load global descriptor table
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Lgdt, Mosa.Platform.x86")]
		public static void Lgdt(uint address) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 sti instruction to enable interrupts
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Sti, Mosa.Platform.x86")]
		public static void Sti() { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public static byte In(byte address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public static byte In8(byte address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 16-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public static ushort In16(ushort address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 32-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.In, Mosa.Platform.x86")]
		public static uint In32(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public static void Out8(byte address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public static void Out(byte address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 16-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public static void Out16(ushort address, ushort value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 32-bit port.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Out, Mosa.Platform.x86")]
		public static void Out32(uint address, uint value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a nop instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Nop, Mosa.Platform.x86")]
		public static void Nop() { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 hlt instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Hlt, Mosa.Platform.x86")]
		public static void Hlt() { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Invlpg, Mosa.Platform.x86")]
		public static void Invlpg(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CpuId, Mosa.Platform.x86")]
		public static byte CpuId(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CpuIdEax, Mosa.Platform.x86")]
		public static int CpuIdEax(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CpuIdEbx, Mosa.Platform.x86")]
		public static int CpuIdEbx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CpuIdEcx, Mosa.Platform.x86")]
		public static int CpuIdEcx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CpuIdEdx, Mosa.Platform.x86")]
		public static int CpuIdEdx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.BochsDebug, Mosa.Platform.x86")]
		public static void BochsDebug() { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Sets the control register 0.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SetCR0, Mosa.Platform.x86")]
		public static void SetCR0(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 2.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SetCR2, Mosa.Platform.x86")]
		public static void SetCR2(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 3.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SetCR3, Mosa.Platform.x86")]
		public static void SetCR3(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 4.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SetCR4, Mosa.Platform.x86")]
		public static void SetCR4(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Gets the control register 0.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetCR0, Mosa.Platform.x86")]
		public static uint GetCR0() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 2.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetCR2, Mosa.Platform.x86")]
		public static uint GetCR2() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 3.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetCR3, Mosa.Platform.x86")]
		public static uint GetCR3() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 4.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetCR4, Mosa.Platform.x86")]
		public static uint GetCR4() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Jumps the global interrupt handler.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetIDTJumpLocation, Mosa.Platform.x86")]
		public static uint GetIDTJumpLocation(uint irq) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Lock
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SpinLock, Mosa.Platform.x86")]
		public static void SpinLock(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Unlock
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SpinUnlock, Mosa.Platform.x86")]
		public static void SpinUnlock(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public static byte Get8(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public static ushort Get16(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public static uint Get32(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Get, Mosa.Platform.x86")]
		public static ulong Get64(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public static void Set8(uint address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public static void Set16(uint address, ushort value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public static void Set32(uint address, uint value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.Set, Mosa.Platform.x86")]
		public static void Set64(uint address, ulong value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Switches the task.
		/// </summary>
		/// <param name="esp">The esp.</param>
		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.SwitchTask, Mosa.Platform.x86")]
		public static void SwitchTask(uint esp) { ThrowPlatformNotSupported(); }

		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.RestoreContext, Mosa.Platform.x86")]
		public static void RestoreContext() { ThrowPlatformNotSupported(); }

		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.GetEip, Mosa.Platform.x86")]
		public static uint GetEip() { ThrowPlatformNotSupported(); return 0;  }

		[Intrinsic(@"Mosa.Platform.x86.Intrinsic.CallFilter, Mosa.Platform.x86")]
		public static void CallFilter() { ThrowPlatformNotSupported(); }

		#endregion

		/// <summary>
		/// Throws a <see cref="System.PlatformNotSupportedException"/>.
		/// </summary>
		/// <remarks>
		/// This function is used by the MSIL implementations of the x86 assembly language 
		/// routines above in order to throw the <see cref="System.PlatformNotSupportedException"/> 
		/// in non-x86 compilation scenarios.
		/// </remarks>
		private static void ThrowPlatformNotSupported()
		{
			throw new PlatformNotSupportedException(@"This operation requires compilation for the x86 architecture.");
		}
	}
}
