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
using Mosa.Runtime.CompilerFramework;
using Mosa.Platforms.x86.Intrinsic;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Instrinsics
		/// <summary>
		/// Wraps the x86 ldit instruction to load the interrupt descriptor table.
		/// </summary>
		/// <param name="address">The address.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Lidt))]
		public static void Lidt(uint address) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 cli instruction to disable interrupts
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Cli))]
		public static void Cli() { ThrowPlatformNotSupported(); return; }

		///// <summary>
		///// Wraps the x86 cmpxchg instruction 
		///// </summary>
		//[Intrinsic(typeof(Architecture), typeof(Intrinsic.CmpXchg))]
		//public static void CmpXchg16(ref short first, short second) { ThrowPlatformNotSupported(); return; }

		///// <summary>
		///// Wraps the x86 cmpxchg instruction 
		///// </summary>
		//[Intrinsic(typeof(Architecture), typeof(Intrinsic.CmpXchg))]
		//public static void CmpXchg32(ref int first, int second) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 lgdt instruction to load global descriptor table
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Lgdt))]
		public static void Lgdt(uint address) { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 sti instruction to enable interrupts
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Sti))]
		public static void Sti() { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.In))]
		public static byte In(byte address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.In))]
		public static byte In8(byte address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 16-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.In))]
		public static ushort In16(ushort address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 32-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.In))]
		public static uint In32(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Out))]
		public static void Out8(byte address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Out))]
		public static void Out(byte address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 16-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Out))]
		public static void Out16(ushort address, ushort value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 32-bit port.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Out))]
		public static void Out32(uint address, uint value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a nop instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Nop))]
		public static void Nop() { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 hlt instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Hlt))]
		public static void Hlt() { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Invlpg))]
		public static void Invlpg(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.CpuId))]
		public static byte CpuId(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.CpuIdEax))]
		public static int CpuIdEax(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.CpuIdEbx))]
		public static int CpuIdEbx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.CpuIdEcx))]
		public static int CpuIdEcx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Wraps the x86 CPUID instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.CpuIdEdx))]
		public static int CpuIdEdx(uint function) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.BochsDebug))]
		public static void BochsDebug() { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Sets the control register 0.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SetCR0))]
		public static void SetCR0(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 2.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SetCR2))]
		public static void SetCR2(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 3.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SetCR3))]
		public static void SetCR3(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the control register 4.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SetCR4))]
		public static void SetCR4(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Gets the control register 0.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.GetCR0))]
		public static uint GetCR0() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 2.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.GetCR2))]
		public static uint GetCR2() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 3.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.GetCR3))]
		public static uint GetCR3() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Gets the control register 4.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.GetCR4))]
		public static uint GetCR4() { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Jumps the global interrupt handler.
		/// </summary>
		/// <returns></returns>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.GetIDTJumpLocation))]
		public static uint GetIDTJumpLocation(uint irq) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// Lock
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SpinLock))]
		public static void SpinLock(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Unlock
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SpinUnlock))]
		public static void SpinUnlock(uint address) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Get))]
		public static byte Get8(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Get))]
		public static ushort Get16(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Get))]
		public static uint Get32(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Get))]
		public static ulong Get64(uint address) { ThrowPlatformNotSupported(); return 0; }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Set))]
		public static void Set8(uint address, byte value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Set))]
		public static void Set16(uint address, ushort value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Set))]
		public static void Set32(uint address, uint value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// 
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.Set))]
		public static void Set64(uint address, ulong value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Switches the task.
		/// </summary>
		/// <param name="esp">The esp.</param>
		[Intrinsic(typeof(Architecture), typeof(Intrinsic.SwitchTask))]
		public static void SwitchTask(uint esp) { ThrowPlatformNotSupported(); }

        [Intrinsic(typeof(Architecture), typeof(Intrinsic.RestoreContext))]
        public static void RestoreContext() { ThrowPlatformNotSupported(); }

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
