/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
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
	public enum AssemblyFlags {
        /// <summary>
        /// 
        /// </summary>
		SideBySideCompatible = 0x0000,
        /// <summary>
        /// 
        /// </summary>
		PublicKey = 0x0001,
        /// <summary>
        /// 
        /// </summary>
		Reserved = 0x0030,
        /// <summary>
        /// 
        /// </summary>
		Retargetable = 0x0100,
        /// <summary>
        /// 
        /// </summary>
		EnableJITcompileTracking = 0x8000,
        /// <summary>
        /// 
        /// </summary>
		DisableJITcompileOptimizer = 0x4000
	}
}
