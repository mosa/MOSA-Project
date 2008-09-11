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
    public enum MethodDataSectionType
    {
        /// <summary>
        /// 
        /// </summary>
        EHTable = 0x01,
        /// <summary>
        /// 
        /// </summary>
        OptIL = 0x02,
        /// <summary>
        /// 
        /// </summary>
        FatFormat = 0x40,
        /// <summary>
        /// 
        /// </summary>
        MoreSections = 0x80
    }
}
