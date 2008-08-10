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
	public enum AssemblyFlags {
		SideBySideCompatible = 0x0000,
		PublicKey = 0x0001,
		Reserved = 0x0030,
		Retargetable = 0x0100,
		EnableJITcompileTracking = 0x8000,
		DisableJITcompileOptimizer = 0x4000
	}
}
