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
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum FlowControl
    {
        /// <summary>
        /// 
        /// </summary>
        Next = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Call = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Meta = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Phi  = 0x04,

        /// <summary>
        /// 
        /// </summary>
        Branch = 0x10,
        /// <summary>
        /// 
        /// </summary>
        ConditionalBranch = 0x20,
        /// <summary>
        /// 
        /// </summary>
        Break = 0x40,
        /// <summary>
        /// 
        /// </summary>
        Return = 0x80,
        /// <summary>
        /// 
        /// </summary>
        Throw = 0x100,

        /// <summary>
        /// 
        /// </summary>
        BlockEndMask = 0x1F0
    }
}
