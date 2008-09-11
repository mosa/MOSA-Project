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
    public enum BlockOrderFlags
    {
        /// <summary>
        /// 
        /// </summary>
        Active = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Visited = 0x02,
    }
}
