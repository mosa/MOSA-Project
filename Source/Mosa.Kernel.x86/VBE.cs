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
        /// Struct instance of a Real Mode Emulator State
        /// </summary>
        private static RealEmulator.State state;

        /// <summary>
        /// 
        /// </summary>
        public unsafe static void Setup()
        {
            // Setup our state
            RealEmulator.CreateState(out state);
        }
    }
}
