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

namespace Mosa.Runtime.CompilerFramework
{
    [Flags]
    public enum FlowControl
    {
        Next = 0x00,
        Call = 0x01,
        Meta = 0x02,
        Phi  = 0x04,

        Branch = 0x10,
        ConditionalBranch = 0x20,
        Break = 0x40,
        Return = 0x80,
        Throw = 0x100,

        BlockEndMask = 0x1F0
    }
}
