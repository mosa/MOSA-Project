/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata
{
    /// <summary>
    /// 
    /// </summary>
    public enum PInvokeAttributes
    {
        /// <summary>
        /// 
        /// </summary>
        NoMangle = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        CharSetMask = 0x0006,
        /// <summary>
        /// 
        /// </summary>
        CharSetNotSpec = 0x0000,
        /// <summary>
        /// 
        /// </summary>
        CharSetAnsi = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        CharSetUnicode = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        CharSetAuto = 0x0006,
        /// <summary>
        /// 
        /// </summary>
        SupportsLastError = 0x0040,

        /// <summary>
        /// 
        /// </summary>
        CallConvMask = 0x0700,
        /// <summary>
        /// 
        /// </summary>
        CallConvWinapi = 0x0100,
        /// <summary>
        /// 
        /// </summary>
        CallConvCdecl = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        CallConvStdcall = 0x0300,
        /// <summary>
        /// 
        /// </summary>
        CallConvThiscall = 0x0400,
        /// <summary>
        /// 
        /// </summary>
        CallConvFastCall = 0x0500
    }
}
