/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.Linker.PE 
{
    /// <summary>
    /// Holds CLI image flags.
    /// </summary>
	[Flags]
	public enum RuntimeImageFlags : uint 
    {
        /// <summary>
        /// Always 1.
        /// </summary>
		ILOnly = 0x0000001,

        /// <summary>
        /// Indicates that the image can only be loaded in 32-bit systems.
        /// </summary>
		F32BitsRequired = 0x0000002,

        /// <summary>
        /// Image is signed with a strong name.
        /// </summary>
		StrongNameSigned = 0x0000008,

        /// <summary>
        /// Must never be set.
        /// </summary>
		TrackDebugData = 0x00010000
	}
}