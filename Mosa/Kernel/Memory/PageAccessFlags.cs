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

namespace Mosa.Kernel.Memory
{
    /// <summary>
    /// Specifies memory protection flags.
    /// </summary>
    [Flags]
    public enum PageProtectionFlags
    {
        /// <summary>
        /// No access is allowed to the page.
        /// </summary>
        NoAccess = 0,

        /// <summary>
        /// The page can only be read.
        /// </summary>
        Read = 1,

        /// <summary>
        /// The page can only be written.
        /// </summary>
        Write = 2,

        /// <summary>
        /// The page can be executed.
        /// </summary>
        Execute = 4,

        /// <summary>
        /// The page is a guard page.
        /// </summary>
        Guard = 0x10000000,

        /// <summary>
        /// The processor is allowed to defer writes to the page to full cache lines.
        /// </summary>
        WriteCombine = 0x20000000,

        /// <summary>
        /// The page may not be cached by the processor.
        /// </summary>
        NoCache = 0x40000000
    }
}
