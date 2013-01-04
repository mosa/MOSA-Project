/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.x86.Intrinsic;
using Mosa.Kernel.x86.RealModeEmulator;

namespace Mosa.Kernel.x86
{
    public static class VBE
    {
        /// <summary>
        /// 
        /// </summary>
        public unsafe static void Setup()
        {
            uint mem = KernelMemory.AllocateMemory(0x100000);
            uint reg = KernelMemory.AllocateMemory(0x34);
            RealEmulator.State state = RealEmulator.CreateState(mem, reg);
        }
    }
}
