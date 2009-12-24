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
using CPUx86 = Mosa.Platforms.x86.CPUx86;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Provides stub methods for selected x86 native assembly instructions.
    /// </summary>
    public static class Native
    {
        #region Instrinsics
        /// <summary>
        /// Wraps the x86 ldit instruction to load the interrupt descriptor table.
        /// </summary>
        /// <param name="idt">A pointer to the interrupt descriptor table.</param>
		[Intrinsic(typeof(Architecture), typeof(CPUx86.LidtInstruction))]
        public static void Ldit(IntPtr idt) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 cli instruction to disable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CliInstruction))]
        public static void Cli() { ThrowPlatformNotSupported(); return;  }

        /// <summary>
        /// Wraps the x86 cmpxchg instruction to disable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CmpXchgInstruction))]
        public static void CmpXchg16(ref short first, short second) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 cmpxchg instruction to disable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CmpXchgInstruction))]
        public static void CmpXchg32(ref int first, int second) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 lgdt instruction to load global descriptor table
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.LgdtInstruction))]
        public static void Lgdt(IntPtr gdt) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 pop instruction to pop a value from the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PopInstruction))]
        public static short Pop16() { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 pop instruction to pop a value from the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PopInstruction))]
        public static int Pop32() { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 popad instruction to pop all GPR from the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PopadInstruction))]
        public static void Popad() { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 push instruction to push a value on the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PushInstruction))]
        public static void Push16(short value) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 push instruction to push a value on the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PushInstruction))]
        public static void Push32(int value) { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 pushad instruction to push all GPR to the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.PushadInstruction))]
        public static void Pushad() { ThrowPlatformNotSupported(); return;  }

        /// <summary>
        /// Wraps the x86 sti instruction to enable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.StiInstruction))]
        public static void Sti() { ThrowPlatformNotSupported(); return; }

        /// <summary>
        /// Wraps the x86 in instruction to read from an 8-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.InInstruction))]
        public static unsafe byte In(byte address) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 in instruction to read from an 8-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.InInstruction))]
        public static unsafe byte In8(byte address) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 in instruction to read from a 16-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.InInstruction))]
        public static unsafe ushort In16(ushort address) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 in instruction to read from a 32-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.InInstruction))]
        public static unsafe uint In32(uint address) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 out instruction to write to an 8-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.OutInstruction))]
        public static unsafe void Out8(byte address, byte value) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 out instruction to write to an 8-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.OutInstruction))]
        public static unsafe void Out(byte address, byte value) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 out instruction to write to a 16-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.OutInstruction))]
        public static unsafe void Out16(ushort address, ushort value) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 out instruction to write to a 32-bit port.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.OutInstruction))]
        public static unsafe void Out32(uint address, uint value) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Wraps the x86 out instruction to write to a nop instruction.
		/// </summary>
		[Intrinsic(typeof(Architecture), typeof(CPUx86.NopInstruction))]
		public static unsafe void Nop() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 hlt instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.HltInstruction))]
        public static void Hlt() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// 
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.InvlpgInstruction))]
		public static void Invlpg(uint address) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 CPUID instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CpuIdInstruction))]
        public static unsafe byte* CpuId(uint function) { ThrowPlatformNotSupported(); return null; }

        /// <summary>
        /// Wraps the x86 CPUID instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CpuIdEaxInstruction))]
        public static int CpuIdEax(uint function) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 CPUID instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CpuIdEbxInstruction))]
        public static int CpuIdEbx(uint function) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 CPUID instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CpuIdEcxInstruction))]
        public static int CpuIdEcx(uint function) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// Wraps the x86 CPUID instruction.
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.CpuIdEdxInstruction))]
        public static int CpuIdEdx(uint function) { ThrowPlatformNotSupported(); return 0; }

        /// <summary>
        /// 
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(CPUx86.BochsDebug))]
        public static void BochsDebug() { ThrowPlatformNotSupported(); return; }

		/// <summary>
		/// Sets the cr0.
		/// </summary>
		/// <param name="status">The status.</param>
		[Intrinsic(typeof(Architecture), typeof(CPUx86.SetCRInstruction))]
		public static void SetControlRegister(uint status) { ThrowPlatformNotSupported(); }

		/// <summary>
		/// Sets the cr3.
		/// </summary>
		/// <param name="pagedirectory">The pagedirectory.</param>
		[Intrinsic(typeof(Architecture), typeof(CPUx86.GetCRInstruction))]
		public static void GetControlRegister(uint pagedirectory) { ThrowPlatformNotSupported(); }

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
