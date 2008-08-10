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
    [Flags]
	public enum ParameterAttributes : ushort {
		None = 0x0000,
		In = 0x0001,
		Out = 0x0002,
		Optional = 0x0010,
		HasDefault = 0x1000,
		HasFieldMarshal = 0x2000,
		Unused = 0xcfe0
	}
}
