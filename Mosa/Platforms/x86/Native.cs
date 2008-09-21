/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (<mailto:phil@thinkedge.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
*/

using System;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;

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
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.LidtInstruction))]
        public static void Ldit(IntPtr idt) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 cli instruction to disable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.CliInstruction))]
        public static void Cli() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 cmpxchg instruction to disable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.CmpXchgInstruction))]
        public static void CmpXchg() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 lgdt instruction to load global descriptor table
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.LgdtInstruction))]
        public static void Lgdt(IntPtr gdt) { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 pop instruction to pop a value from the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.PopInstruction))]
        public static void Pop() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 popad instruction to pop all GPR from the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.PopadInstruction))]
        public static void Popad() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 push instruction to push a value on the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.PushInstruction))]
        public static void Push() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 pushad instruction to push all GPR to the stack
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.PushadInstruction))]
        public static void Pushad() { ThrowPlatformNotSupported(); }

        /// <summary>
        /// Wraps the x86 sti instruction to enable interrupts
        /// </summary>
        [Intrinsic(typeof(Architecture), typeof(Instructions.Intrinsics.StiInstruction))]
        public static void Sti() { ThrowPlatformNotSupported(); }
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
