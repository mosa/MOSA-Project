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

namespace Mosa.Runtime.CompilerFramework.IL
{
    [Flags]
    public enum MethodFlags : ushort
    {
        TinyFormat = 0x02,
        FatFormat = 0x03,
        MoreSections = 0x08,
        InitLocals = 0x10,
        TinyCodeSizeMask = 0xFC,
        HeaderSizeMask = 0xF000,
        ValidHeader = 0x3000,
        HeaderMask = 0x0003
    }
}
