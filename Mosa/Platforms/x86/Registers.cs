/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Enumeration of all standard registers accessible in standard 32/bit x86 CPUs
    /// </summary>
    public enum Registers
    {
        /// <summary>
        /// 
        /// </summary>
        Undefined = 0,

        //---------------------------------------------------------------------
        // General purpose registers
        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        EAX = 1,
        /// <summary>
        /// 
        /// </summary>
        ECX = 2,
        /// <summary>
        /// 
        /// </summary>
        EDX = 3,
        /// <summary>
        /// 
        /// </summary>
        EBX = 4,
        /// <summary>
        /// 
        /// </summary>
        ESP = 5,
        /// <summary>
        /// 
        /// </summary>
        EBP = 6,
        /// <summary>
        /// 
        /// </summary>
        ESI = 7,
        /// <summary>
        /// 
        /// </summary>
        EDI = 8,

        //---------------------------------------------------------------------
        // MMX registers
        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        MMX0 = MMX + 1,
        /// <summary>
        /// 
        /// </summary>
        MMX1 = MMX + 2,
        /// <summary>
        /// 
        /// </summary>
        MMX2 = MMX + 3,
        /// <summary>
        /// 
        /// </summary>
        MMX3 = MMX + 4,
        /// <summary>
        /// 
        /// </summary>
        MMX4 = MMX + 5,
        /// <summary>
        /// 
        /// </summary>
        MMX5 = MMX + 6,
        /// <summary>
        /// 
        /// </summary>
        MMX6 = MMX + 7,
        /// <summary>
        /// 
        /// </summary>
        MMX7 = MMX + 8,

        //---------------------------------------------------------------------
        // XMM SSE registers
        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        XMM0 = XMM + 1,
        /// <summary>
        /// 
        /// </summary>
        XMM1 = XMM + 2,
        /// <summary>
        /// 
        /// </summary>
        XMM2 = XMM + 3,
        /// <summary>
        /// 
        /// </summary>
        XMM3 = XMM + 4,
        /// <summary>
        /// 
        /// </summary>
        XMM4 = XMM + 5,
        /// <summary>
        /// 
        /// </summary>
        XMM5 = XMM + 6,
        /// <summary>
        /// 
        /// </summary>
        XMM6 = XMM + 7,
        /// <summary>
        /// 
        /// </summary>
        XMM7 = XMM + 8,

        //---------------------------------------------------------------------
        // FPU registers
        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        FP0 = FP + 1,
        /// <summary>
        /// 
        /// </summary>
        FP1 = FP + 2,
        /// <summary>
        /// 
        /// </summary>
        FP2 = FP + 3,
        /// <summary>
        /// 
        /// </summary>
        FP3 = FP + 4,
        /// <summary>
        /// 
        /// </summary>
        FP4 = FP + 5,
        /// <summary>
        /// 
        /// </summary>
        FP5 = FP + 6,
        /// <summary>
        /// 
        /// </summary>
        FP6 = FP + 7,
        /// <summary>
        /// 
        /// </summary>
        FP7 = FP + 8,

        //---------------------------------------------------------------------
        // Offsets
        //---------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        GPR = 1,
        /// <summary>
        /// 
        /// </summary>
        MMX = 17,
        /// <summary>
        /// 
        /// </summary>
        XMM = 25,
        /// <summary>
        /// 
        /// </summary>
        FP  = 41,
        /// <summary>
        /// 
        /// </summary>
        Total = 49
    }
}
