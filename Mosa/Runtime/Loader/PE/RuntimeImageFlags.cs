/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.Loader.PE {
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum RuntimeImageFlags : uint {
        /// <summary>
        /// 
        /// </summary>
		ILOnly = 0x0000001,
        /// <summary>
        /// 
        /// </summary>
		F32BitsRequired = 0x0000002,
        /// <summary>
        /// 
        /// </summary>
		StrongNameSigned = 0x0000008,
        /// <summary>
        /// 
        /// </summary>
		TrackDebugData = 0x00010000
	}
}