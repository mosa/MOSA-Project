/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Platforms.x86
{
    public enum Registers
    {
        Undefined = 0,

        ///////////////////////////////////////////////////////////////////////
        // General purpose registers
        ///////////////////////////////////////////////////////////////////////
        EAX = 1,
        ECX = 2,
        EDX = 3,
        EBX = 4,
        ESP = 5,
        EBP = 6,
        ESI = 7,
        EDI = 8,

        ///////////////////////////////////////////////////////////////////////
        // MMX registers
        ///////////////////////////////////////////////////////////////////////
        MMX0 = MMX + 1,
        MMX1 = MMX + 2,
        MMX2 = MMX + 3,
        MMX3 = MMX + 4,
        MMX4 = MMX + 5,
        MMX5 = MMX + 6,
        MMX6 = MMX + 7,
        MMX7 = MMX + 8,

        ///////////////////////////////////////////////////////////////////////
        // XMM SSE registers
        ///////////////////////////////////////////////////////////////////////
        XMM0 = XMM + 1,
        XMM1 = XMM + 2,
        XMM2 = XMM + 3,
        XMM3 = XMM + 4,
        XMM4 = XMM + 5,
        XMM5 = XMM + 6,
        XMM6 = XMM + 7,
        XMM7 = XMM + 8,

        ///////////////////////////////////////////////////////////////////////
        // FPU registers
        ///////////////////////////////////////////////////////////////////////
        FP0 = FP + 1,
        FP1 = FP + 2,
        FP2 = FP + 3,
        FP3 = FP + 4,
        FP4 = FP + 5,
        FP5 = FP + 6,
        FP6 = FP + 7,
        FP7 = FP + 8,

        ///////////////////////////////////////////////////////////////////////
        // Offsets
        ///////////////////////////////////////////////////////////////////////
        GPR = 1,
        MMX = 17,
        XMM = 25,
        FP  = 41,
        Total = 49
    }
}
