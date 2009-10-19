/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Determines features provided by an architecture.
    /// </summary>
    [ Flags ]
    public enum ArchitectureFeatureFlags
    {
        /// <summary>
        /// Auto detect architecture features using the current processor. Not available for cross compilation.
        /// </summary>
        AutoDetect = 0,

        /// <summary>
        /// The x86 architecture supports MMX.
        /// </summary>
        MMX = 1,

        /// <summary>
        /// The x86 architecture supports the first revision of SSE.
        /// </summary>
        SSE = 2,

        /// <summary>
        /// The x86 architecture supports the second revision of SSE.
        /// </summary>
        SSE2 = 4,

        /// <summary>
        /// The x86 architecture supports the third revision of SSE.
        /// </summary>
        SSE3 = 8,

        /// <summary>
        /// The x86 architecture supports the fourth revision of SSE.
        /// </summary>
        SSE4 = 16,

        /// <summary>
        /// The x86 architecture supports 3DNow.
        /// </summary>
        ThreeDNow,

        // FIXME: Add more instructions set specific flags
    }
}
