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

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// 
    /// </summary>
	public enum AssemblyHashAlgorithm {
        /// <summary>
        /// 
        /// </summary>
		None,
        /// <summary>
        /// 
        /// </summary>
		ReservedMD5 = 0x8003,
        /// <summary>
        /// 
        /// </summary>
		SHA1 = 0x8004
	}
}
