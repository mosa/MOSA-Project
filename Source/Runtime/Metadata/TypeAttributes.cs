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
    public enum TypeAttributes
    {
        /// <summary>
        /// 
        /// </summary>
        VisibilityMask = 0x00000007,
        /// <summary>
        /// 
        /// </summary>
        NotPublic = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        Public = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        NestedPublic = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        NestedPrivate = 0x00000003,
        /// <summary>
        /// 
        /// </summary>
        NestedFamily = 0x00000004,
        /// <summary>
        /// 
        /// </summary>
        NestedAssembly = 0x00000005,
        /// <summary>
        /// 
        /// </summary>
        NestedFamAndAssem = 0x0000006,
        /// <summary>
        /// 
        /// </summary>
        NestedFamOrAssem = 0x00000007,

        /// <summary>
        /// 
        /// </summary>
        LayoutMask = 0x00000018,
        /// <summary>
        /// 
        /// </summary>
        AutoLayout = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        SequentialLayout = 0x00000008,
        /// <summary>
        /// 
        /// </summary>
        ExplicitLayout = 0x00000010,

        /// <summary>
        /// 
        /// </summary>
        ClassSemanticsMask = 0x00000020,
        /// <summary>
        /// 
        /// </summary>
        Class = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        Interface = 0x00000020,

        /// <summary>
        /// 
        /// </summary>
        Abstract = 0x00000080,
        /// <summary>
        /// 
        /// </summary>
        Sealed = 0x00000100,
        /// <summary>
        /// 
        /// </summary>
        SpecialName = 0x00000400,

        /// <summary>
        /// 
        /// </summary>
        Import = 0x00001000,
        /// <summary>
        /// 
        /// </summary>
        Serializable = 0x00002000,

        /// <summary>
        /// 
        /// </summary>
        StringFormatMask = 0x00030000,
        /// <summary>
        /// 
        /// </summary>
        AnsiClass = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        UnicodeClass = 0x00010000,
        /// <summary>
        /// 
        /// </summary>
        AutoClass = 0x00020000,
        /// <summary>
        /// 
        /// </summary>
        CustomFormatClass = 0x00030000,
        /// <summary>
        /// 
        /// </summary>
        CustomStringFormatMask = 0x00C00000,

        /// <summary>
        /// 
        /// </summary>
        BeforeFieldInit = 0x00100000,
        /// <summary>
        /// 
        /// </summary>
        RTSpecialName = 0x00000800,
        /// <summary>
        /// 
        /// </summary>
        HasSecurity = 0x00040000
    }
}
