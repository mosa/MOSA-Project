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
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum MethodFlags : ushort
    {
        /// <summary>
        /// 
        /// </summary>
        TinyFormat = 0x02,
        /// <summary>
        /// 
        /// </summary>
        FatFormat = 0x03,
        /// <summary>
        /// 
        /// </summary>
        MoreSections = 0x08,
        /// <summary>
        /// 
        /// </summary>
        InitLocals = 0x10,
        /// <summary>
        /// 
        /// </summary>
        TinyCodeSizeMask = 0xFC,
        /// <summary>
        /// 
        /// </summary>
        HeaderSizeMask = 0xF000,
        /// <summary>
        /// 
        /// </summary>
        ValidHeader = 0x3000,
        /// <summary>
        /// 
        /// </summary>
        HeaderMask = 0x0003
    }
}
