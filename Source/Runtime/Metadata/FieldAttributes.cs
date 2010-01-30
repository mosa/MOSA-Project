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
    [Flags]
    public enum FieldAttributes
    {
        /// <summary>
        /// 
        /// </summary>
        FieldAccessMask = 0x0007,
        /// <summary>
        /// 
        /// </summary>
        CompilerControlled = 0x0000,
        /// <summary>
        /// 
        /// </summary>
        Private = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        FamAndAssem = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        Assembly = 0x0003,
        /// <summary>
        /// 
        /// </summary>
        Family = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        FamOrAssem = 0x0005,
        /// <summary>
        /// 
        /// </summary>
        Public = 0x0006,
        /// <summary>
        /// 
        /// </summary>
        Static = 0x0010,
        /// <summary>
        /// 
        /// </summary>
        InitOnly = 0x0020,
        /// <summary>
        /// 
        /// </summary>
        Literal = 0x0040,
        /// <summary>
        /// 
        /// </summary>
        NotSerialized = 0x0080,
        /// <summary>
        /// 
        /// </summary>
        SpecialName = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        PInvokeImpl = 0x2000,
        /// <summary>
        /// 
        /// </summary>
        RTSpecialName = 0x0400,
        /// <summary>
        /// 
        /// </summary>
        HasFieldMarshal = 0x1000,
        /// <summary>
        /// 
        /// </summary>
        HasDefault = 0x8000,
        /// <summary>
        /// 
        /// </summary>
        HasFieldRVA = 0x0100
    }
}
